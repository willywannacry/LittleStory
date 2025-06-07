using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameGenerator : MonoBehaviour
{
    [SerializeField] private NamePoolSO _namePool;

    private void Awake()
    {
        if (_namePool == null)
        {
            _namePool = Resources.Load<NamePoolSO>("NamePool");
            if (_namePool == null)
                Debug.LogError("[NameGenerator] Не удалось загрузить NamePool из Resources!");
        }
    }

    public List<string> GetRandomNames(Gender gender, int count = 3)
    {
        List<string> source = gender switch
        {
            Gender.Male => new List<string>(_namePool.MaleNames),
            Gender.Female => new List<string>(_namePool.FemaleNames),
            _ => new List<string>() // вдруг добавим Gender.Other
        };

        if (source == null || source.Count == 0)
        {
            Debug.LogWarning("Пул имён пуст.");
            return new List<string>();
        }

        List<string> result = new();
        List<string> temp = new(source); // копия

        for (int i = 0; i < count && temp.Count > 0; i++)
        {
            int index = Random.Range(0, temp.Count);
            result.Add(temp[index]);
            temp.RemoveAt(index);
        }

        return result;
    }
}