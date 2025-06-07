using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MailManager : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _mailPrefab;
    
    private Character _character;

    public void SetCharacter(Character character)
    {
        _character = character;
        RefreshMail();
    }

    public void RefreshMail()
    {
        foreach (Transform child in _content)
        {
            Destroy(child.gameObject);
        }

        foreach (var result in _character.MailEntries)
        {
            GameObject card = Instantiate(_mailPrefab, _content);
            card.transform.SetSiblingIndex(0);
            var mailUI = card.GetComponent<MailUI>();
            
            if (mailUI == null)
            {
                Debug.LogWarning("MailUI not found on instantiated card.");
                return;
            }
            
            mailUI.Setup(result);
        }
        
        Debug.Log("Количество событий в истории: " + _character.EventHistory.Count);
    }
}
