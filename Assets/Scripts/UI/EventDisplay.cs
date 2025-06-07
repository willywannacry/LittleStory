using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class EventDisplay : MonoBehaviour
{
    [SerializeField] private Image eventImage;
    [SerializeField] private TMP_Text eventName;
    [SerializeField] private TMP_Text eventDescription;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject buttonPrefab;

    [SerializeField] private Transform resultCardContainer;
    [SerializeField] private GameObject resultCardPrefab;

    [SerializeField] private GameObject panelChoices;

    [SerializeField] private TMP_Text resultText;
    [SerializeField] private GameObject panelResult;
    [SerializeField] private TMP_Text resultName;
    [SerializeField] private Button exitButton;
    [SerializeField] private NameGenerator _nameGenerator;

    public void ShowEvent(LifeEvent lifeEvent, Character character, GameManager manager)
    {

        if (lifeEvent.Id == "choose_name")
        {
            panelChoices.SetActive(true);
            panelResult.SetActive(false);

            eventName.text = lifeEvent.Title;
            eventDescription.text = lifeEvent.Description;

            foreach (Transform child in buttonContainer)
                Destroy(child.gameObject);

            // ✅ Случайный пол
            Gender gender = (Gender)Random.Range(0, Gender.GetValues(typeof(Gender)).Length);

            // ✅ Получение имён по полу
            var names = _nameGenerator.GetRandomNames(gender, 3);

            foreach (var name in names)
            {
                var btnObj = Instantiate(buttonPrefab, buttonContainer);
                var btn = btnObj.GetComponent<Button>();
                var txt = btnObj.GetComponentInChildren<TMP_Text>();
                txt.text = name;

                btn.onClick.AddListener(() =>
                {
                    character.SetName(name);
                    character.SetGender(gender); // ❗ убедись, что у Character есть такой метод
                    character.AddMailEntry($"Вы выбрали имя: {name}");
                    character.AddEvent($"Теперь тебя зовут {name}.");
                    manager.RefreshMail();
                    manager.CharacterHUD.Refresh();

                    panelChoices.SetActive(false);
                    panelResult.SetActive(true);
                    resultName.text = "Имя выбрано";
                    resultText.text = $"Теперь тебя зовут {name}.";

                    exitButton.onClick.RemoveAllListeners();
                    exitButton.onClick.AddListener(() =>
                    {
                        panelResult.SetActive(false);
                        resultName.text = "";
                        resultText.text = "";
                        manager.OnEventChoiceMade();
                    });
                });
            }

            return;
        }


        // ▼ обычная логика ▼
        panelChoices.SetActive(true);
        panelResult.SetActive(false);

        eventName.text = lifeEvent.Title;
        eventDescription.text = lifeEvent.Description;

        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);

        foreach (var choice in lifeEvent.Choices)
        {
            var btnObj = Instantiate(buttonPrefab, buttonContainer);
            var btn = btnObj.GetComponent<Button>();
            var txt = btnObj.GetComponentInChildren<TMP_Text>();
            txt.text = choice.Text;

            btn.onClick.AddListener(() => { ApplyChoice(choice, character, manager); });
        }
    }

    private void ApplyChoice(LifeChoice choice, Character character, GameManager manager)
    {
        panelChoices.SetActive(false);
        panelResult.SetActive(true);

        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(() =>
        {
            panelResult.SetActive(false);
            resultText.text = "";
            resultName.text = "";
            manager.OnEventChoiceMade();
        });
        
        bool isSuccess = choice.ResolveChoice(character);
        var statChanges = isSuccess ? choice.StatChangeSuccess : choice.StatChangeFail;

        // Всегда добавляем
        foreach (var trait in choice.AddTraitsAlways)
            character.AddTrait(trait);
        foreach (var flag in choice.AddFlagsAlways)
            character.AddFlag(flag);

        // По результату
        if (isSuccess)
        {
            foreach (var trait in choice.AddTraitsSuccess)
                character.AddTrait(trait);
            foreach (var flag in choice.AddFlagsSuccess)
                character.AddFlag(flag);
        }
        else
        {
            foreach (var trait in choice.AddTraitsFail)
                character.AddTrait(trait);
            foreach (var flag in choice.AddFlagsFail)
                character.AddFlag(flag);
        }

        character.IncreaseAge(choice.Age);
        
        ApplyStatChanges(statChanges, character);

        string result = isSuccess ? choice.ResultSuccess : choice.ResultFail;
        string resultTitle = isSuccess ? choice.ResultTitleSuccess : choice.ResultTitleFail;

        character.AddEvent(result);
        character.AddMailEntry(result);
        manager.RefreshMail();

        resultName.text = resultTitle;
        resultText.text = result;
    }
    
    private void ApplyStatChanges(Dictionary<StatType, int> changes, Character character)
    {
        foreach (var pair in changes)
        {
            switch (pair.Key)
            {
                case StatType.Health: character.AddHealth(pair.Value); break;
                case StatType.Stress: character.AddStress(pair.Value); break;
                case StatType.Beauty: character.AddBeauty(pair.Value); break;
                case StatType.Intelligence: character.AddIntelligence(pair.Value); break;
                case StatType.Money: character.AddMoney(pair.Value); break;
                default: Debug.LogWarning($"Unknown stat: {pair.Key}"); break;
            }
        }
    }
}