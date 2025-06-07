using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class StatChangeBlock
{
    public List<StatType> StatKeys;
    public List<int> StatValues;

    public Dictionary<StatType, int> ToDictionary()
    {
        Dictionary<StatType, int> result = new();
        for (int i = 0; i < Mathf.Min(StatKeys.Count, StatValues.Count); i++)
            result[StatKeys[i]] = StatValues[i];
        return result;
    }
}

[System.Serializable]
public class LifeEvent
{
   [SerializeField] private string _id;
   [SerializeField] private string _title;
   [SerializeField] private string _description;
   [SerializeField] private int _recomendedAge; 
    
   [SerializeField] private List<LifeChoice> _choices;
   
   public string Id => _id;
   public string Title => _title;
   public string Description => _description;
   public int RecomendedAge => _recomendedAge;
   
   public List<LifeChoice> Choices => _choices;
}

[Serializable]
public class LifeChoice
{
    [SerializeField] private int _baseSuccessChance;
    [SerializeField] private string _resultSuccess;
    [SerializeField] private string _resultFail;
    [SerializeField] private string _resultTitleSuccess;
    [SerializeField] private string _resultTitleFail;
    [SerializeField] private List<string> _requiredTraits;
    [SerializeField] private List<string> _failFlags;
    [SerializeField] private List<string> _deadFlags;
    [SerializeField] private string _text;
    [SerializeField] private int _ageOffSet;
    
    [SerializeField] private List<string> _addTraitsAlways;
    [SerializeField] private List<string> _addFlagsAlways;

    [SerializeField] private List<string> _addTraitsSuccess;
    [SerializeField] private List<string> _addFlagsSuccess;

    [SerializeField] private List<string> _addTraitsFail;
    [SerializeField] private List<string> _addFlagsFail;

    public IReadOnlyCollection<string> AddTraitsAlways => _addTraitsAlways;
    public IReadOnlyCollection<string> AddFlagsAlways => _addFlagsAlways;

    public IReadOnlyCollection<string> AddTraitsSuccess => _addTraitsSuccess;
    public IReadOnlyCollection<string> AddFlagsSuccess => _addFlagsSuccess;

    public IReadOnlyCollection<string> AddTraitsFail => _addTraitsFail;
    public IReadOnlyCollection<string> AddFlagsFail => _addFlagsFail;

    [SerializeField] private StatChangeBlock _statChangeSuccess;
    [SerializeField] private StatChangeBlock _statChangeFail;

    public int BaseSuccessChance => _baseSuccessChance;
    public string ResultSuccess => _resultSuccess;
    public string ResultFail => _resultFail;
    public string ResultTitleSuccess => _resultTitleSuccess;
    public string ResultTitleFail => _resultTitleFail;
    public IReadOnlyCollection<string> RequiredTraits => _requiredTraits;
    public IReadOnlyCollection<string> FailFlags => _failFlags;
    public string Text => _text;
    public int Age => _ageOffSet;
    public Dictionary<StatType, int> StatChangeSuccess => _statChangeSuccess.ToDictionary();
    public Dictionary<StatType, int> StatChangeFail => _statChangeFail.ToDictionary();

    public bool ResolveChoice(Character character)
    {
        int finalChance = _baseSuccessChance;

        foreach (var trait in _requiredTraits)
            if (character.HasTrait(trait)) finalChance += 20;

        foreach (var flag in _deadFlags)
            if (character.HasTrait(flag)) return false;

        foreach (var flag in _failFlags)
            if (character.HasFlag(flag)) finalChance -= 20;

        int roll = UnityEngine.Random.Range(1, 101);
        return roll <= finalChance;
    }
}