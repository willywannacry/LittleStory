using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<LifeEventsSO> _eventSequence;
    [SerializeField] private LifeEventsSO _birthEvent;

    [SerializeField] private EventDisplay _display;
    [SerializeField] private GameObject _eventPanel;
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _dimOverlay;

    [SerializeField] private MailManager _mailManager;
    [SerializeField] private TabsSwitcher _tabsSwitcher;
    [SerializeField] private CharacterHUD _characterHUD;
    [SerializeField] private NameGenerator _nameGenerator;

    private Character _character = new();
    private bool _firstEventShown = false;

    // 🌀 Очереди событий по категориям
    private Dictionary<EventCategory, Queue<LifeEventsSO>> _shuffledEvents = new();

    public CharacterHUD CharacterHUD => _characterHUD;

    private void Start()
    {
        _tabsSwitcher.SetCharacter(_character);
        _characterHUD.SetCharacter(_character);
        _mailManager.SetCharacter(_character);

        PrepareEventQueues();
        ShowSpecificEvent(_birthEvent);
        _firstEventShown = true;
    }

    private void PrepareEventQueues()
    {
        Dictionary<EventCategory, List<LifeEventsSO>> tempPools = new();

        foreach (var evt in _eventSequence)
        {
            if (!tempPools.ContainsKey(evt.Category))
                tempPools[evt.Category] = new List<LifeEventsSO>();

            tempPools[evt.Category].Add(evt);
        }

        foreach (var pair in tempPools)
        {
            var shuffled = pair.Value.OrderBy(_ => Random.value).ToList();
            _shuffledEvents[pair.Key] = new Queue<LifeEventsSO>(shuffled);
        }
    }

    public void OnNextEventButtonPressed()
    {
        _eventPanel.SetActive(true);
        _dimOverlay.SetActive(true);
        ShowNextEvent(GetCurrentCategory());
    }

    private void ShowNextEvent(EventCategory category)
    {
        if (!_shuffledEvents.ContainsKey(category) || _shuffledEvents[category].Count == 0)
        {
            Debug.LogWarning($"Нет событий для категории: {category}");
            return;
        }

        // Пропускаем события с неподходящим возрастом
        while (_shuffledEvents[category].Count > 0)
        {
            var next = _shuffledEvents[category].Peek();
            if (next.Event.RecomendedAge <= _character.Age)
            {
                var toShow = _shuffledEvents[category].Dequeue();
                _display.ShowEvent(toShow.Event, _character, this);
                return;
            }
            else
            {
                // Пока возраст не подходит — просто откладываем
                _shuffledEvents[category].Enqueue(_shuffledEvents[category].Dequeue());
            }
        }

        Debug.Log($"Все события в категории {category} недоступны по возрасту");
    }

    public void OnEventChoiceMade()
    {
        _eventPanel.SetActive(false);
        _dimOverlay.SetActive(false);
        _mainPanel.SetActive(true);
    }

    private EventCategory GetCurrentCategory()
    {
        int age = _character.Age;
        if (age < 7) return EventCategory.Childhood;
        if (age < 13) return EventCategory.Teenager;
        if (age < 18) return EventCategory.Youth;
        return EventCategory.Adulthood;
    }

    public void RefreshMail()
    {
        _mailManager.RefreshMail();
    }

    private void ShowSpecificEvent(LifeEventsSO eventSO)
    {
        _eventPanel.SetActive(true);
        _dimOverlay.SetActive(true);
        _display.ShowEvent(eventSO.Event, _character, this);
    }
}