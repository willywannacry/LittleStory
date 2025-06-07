using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using System;
using Random = System.Random;

public enum StatType 
{ 
    Health, 
    Stress,
    Beauty, 
    Intelligence, 
    Money
}

public class Character
{
    public Character()
    {
        Random random = new Random();
        
        Health = random.Next(1, 100);
        Stress = 0;
        Beauty = random.Next(1, 100);
        Intelligence = random.Next(1, 100);

        if (Intelligence <= 15)
        {
            RemoveTrait("гений");
            AddTrait("глупый");
        }

        if (Intelligence >= 80)
        {
            AddTrait("гений");
            RemoveTrait("глупый");
        }

        if (Health >= 80)
        {
            AddTrait("здоровый");
            RemoveTrait("болезненный");
        }

        if (Health <= 15)
        {
            AddTrait("болезненный");
            RemoveTrait("здоровый");
        }

        if(Beauty <= 15)
        {
            AddTrait("урод");
            RemoveTrait("красивый");
        }
        
        if(Beauty >= 80)
        {
            AddTrait("красивый");
            RemoveTrait("урод");
        }
    }
    
    public string Name {get; private set;}
    public int Money {get; private set;}
    public Gender Gender {get; private set;}
    public int Stress {get; private set;}
    public int Beauty {get; private set;}
    public int Health {get; private set;}
    public int Intelligence {get; private set;}
    
    public int Age { get; private set; }

    
    

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetGender(Gender gender)
    {
        Gender = gender;
    }
    
    private List<string> _eventHistory { get; set; } = new();
    private List<string> _important = new();
    
    private HashSet<string> _traits = new();
    private HashSet<string> _flags = new();
    
    private List<string> _mailEntries = new();
    
    public List<string> MailEntries => new(_mailEntries);
    
    public IReadOnlyList<string> EventHistory => _eventHistory;
    public List<string> Important => new(_important);
    
    public string LifeStage => Age switch
    {
        <7 => "Раннее детство",
        <13 => "Юность",
        <18 => "Подросток",
        <30 => "Молодость",
        <60 => "Зрелость",
        _ => "Старость",
    };

    public void AddMailEntry(string result)
    {
        if (string.IsNullOrEmpty(result) == false)
        {
            _mailEntries.Add(result);
        }
    }
    
    public IReadOnlyCollection<string> Traits => _traits;
    public IReadOnlyCollection<string> Flags => _flags;
    
    public void IncreaseAge(int years) => Age += years;
    public void SetAge(int newAge) => Age = newAge;
    
    public void AddStress(int amount) => Stress = Mathf.Clamp(Stress + amount, 0, 100);
    public void AddMoney(int amount) => Money += amount;
    public void AddHealth(int amount) => Health = Mathf.Clamp(Health + amount, 0, 100);
    public void AddIntelligence(int amount) => Intelligence += amount;
    public void AddBeauty(int amount) => Beauty += amount;
    
    public bool HasTrait(string trait) => _traits.Contains(trait);
    public bool HasFlag(string flag) => _flags.Contains(flag);
    public void AddFlag(string flag) => _flags.Add(flag);
    public void AddTrait(string trait) => _traits.Add(trait);
    public void RemoveTrait(string trait) => _traits.Remove(trait);
    public void RemoveFlag(string flag) => _flags.Remove(flag);

    public List<string> GetAvailableTabs()
    {
        var tabs = new List<string>{"Other", "Actions"};
        
        if (Age >= 7) tabs.Add("Occupation");
        if (Age >= 4) tabs.Add("Relationship");
        if (_flags.Contains("has_property")) tabs.Add("Asset");
        
        return tabs;
    }

    public void AddEvent(string result)
    {
        _eventHistory.Add(result);
    }

    public void MarkAsImportant(string result)
    {
        if(_important.Contains(result) == false)
            _important.Add(result);
    }
}