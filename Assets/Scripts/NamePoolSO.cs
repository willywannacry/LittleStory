using UnityEngine;

[CreateAssetMenu(fileName = "NamePool", menuName = "LifeStory/NamePool")]
public class NamePoolSO : ScriptableObject
{
    public string[] MaleNames;
    public string[] FemaleNames;
}

public enum Gender
{
    Male,
    Female
}
// Пример использования:
// var name = nameGenerator.GetRandomName(Gender.Male);