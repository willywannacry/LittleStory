using UnityEngine;


public class TabsSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] _allTabs;

    private Character _character;

    public void SetCharacter(Character character)
    {
        _character = character;
    }

    public void ShowTab(GameObject tabToShow)
    {
        Debug.Log($"[TabsSwitcher] Открываем вкладку: {tabToShow.name}");

        foreach (var tab in _allTabs)
            tab.SetActive(tab == tabToShow);

        if (tabToShow.name == "CharacterPanel")
        {
            var infoUI = tabToShow.GetComponentInChildren<CharacterInfoUI>();
            if (infoUI != null)
                infoUI.SetCharacter(_character);
            else
                Debug.LogWarning("[TabsSwitcher] CharacterInfoUI не найден");
        }
    }
}