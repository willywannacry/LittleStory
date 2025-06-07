using TMPro;
using UnityEngine;

public class CharacterHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text moneyText;
    
    [SerializeField] private TMP_Text stress;
    [SerializeField] private TMP_Text beauty;
    [SerializeField] private TMP_Text health;
    [SerializeField] private TMP_Text intelligence;

    private Character _character;

    public void SetCharacter(Character character)
    {
        _character = character;
        Refresh();
    }

    public void Refresh()
    {
        if (_character == null) return;

        nameText.text = _character.Name; 
        moneyText.text = $"{_character.Money} €";
        stress.text = _character.Stress + "%";
        beauty.text = _character.Beauty + "%";
        health.text = _character.Health + "%";
        intelligence.text = _character.Intelligence + "%";
    }
}