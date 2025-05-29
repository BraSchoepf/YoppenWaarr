using UnityEngine;

[CreateAssetMenu(fileName = "Character_Data_SO", menuName = "Scriptable Objects/Character_Data_SO")]
public class Character_Data_SO : ScriptableObject
{
    // Character's name (ej: Gaucho, Soldado, Boss)
    [SerializeField] private string _characterName;

    // Character's base life 
    [SerializeField] private int _health;

    // Speed of movement
    [SerializeField] private float _speed;

    // Read-only public properties
    public string CharacterName => _characterName;
    public int Health => _health;
    public float Speed => _speed;

}
