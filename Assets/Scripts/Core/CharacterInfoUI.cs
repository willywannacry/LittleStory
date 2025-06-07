using UnityEngine;
using TMPro;


public class CharacterInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _ageText;
    [SerializeField] private TextMeshProUGUI _stageText;
    [SerializeField] private Transform traitsContainer;
    [SerializeField] private GameObject traitPrefab;
    private Character _character;

    public void SetCharacter(Character character)
    {
        _character = character;
        _nameText.text = _character.Name;
        Refresh();
    }

    private void Refresh()
    {
        _ageText.text = _character.Age.ToString();
        _stageText.text = _character.LifeStage;
        
        foreach (Transform child in traitsContainer)
            Destroy(child.gameObject);

        foreach (var trait in _character.Traits)
        {
            var obj = Instantiate(traitPrefab, traitsContainer);
            obj.GetComponentInChildren<TMP_Text>().text = trait; 
        }
    }
}