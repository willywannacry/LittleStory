using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MailUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Button _markAsImportant;

    public void Setup(string description)
    {
        _description.text = description;
        _markAsImportant.onClick.AddListener(MarkAsImportant);
    }

    private void MarkAsImportant()
    {
        Debug.Log("Помечено как важное:" + _description.text);
    }
}
