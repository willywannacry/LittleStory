using UnityEngine;

public enum EventCategory
{
    Childhood,
    Teenager,   
    Youth,
    Adulthood
}
[CreateAssetMenu(menuName = "Scriptable Objects/LifeEvents")]
public class LifeEventsSO : ScriptableObject
{
    [SerializeField] private EventCategory _category;
    
    [SerializeField] private int _spawnChance = 100;
    public int SpawnChance => _spawnChance;
    public EventCategory Category => _category;
    public LifeEvent Event;
}