using System;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class InkManager : MonoBehaviour
{
    [SerializeField] private TextAsset _inkJSONAsset;
    
    private Story _story;
    
    [SerializeField] private Text _dialogueText;
    [SerializeField] private Button[] _choiceButtons;

    private void Start()
    {
        _story = new Story(_inkJSONAsset.text);
        RefreshView();
    }

    void RefreshView()
    {
        _dialogueText.text = _story.Continue();

        int i = 0;

        foreach (var choice in _story.currentChoices)
        {
            _choiceButtons[i].gameObject.SetActive(true);
            _choiceButtons[i].GetComponentInChildren<Text>().text = choice.text;
            int choiceIndex = i;
            _choiceButtons[i].onClick.RemoveAllListeners();
            
            _choiceButtons[i].onClick.AddListener(() =>
            {
                _story.ChooseChoiceIndex(choiceIndex);
                RefreshView();
            });

            i++;
        }

        for (; i < _choiceButtons.Length; i++)
        {
            _choiceButtons[i].gameObject.SetActive(false);
        }
    }
}
