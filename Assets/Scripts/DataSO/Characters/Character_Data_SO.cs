using UnityEngine;

[CreateAssetMenu(fileName = "Character_Data_SO", menuName = "Scriptable Objects/Character_Data_SO")]
public class Character_Data_SO : ScriptableObject
{
    // Nombre del personaje (ej: Gaucho, Soldado, Boss)
    [SerializeField] private string _characterName;

    // Vida base del personaje
    [SerializeField] private int _health;

    // Velocidad de movimiento
    [SerializeField] private float _speed;

    // Direcci�n actual de movimiento (no editable desde el editor)
    private Vector2 _direction;

    // Propiedades p�blicas solo de lectura (readonly)
    public string CharacterName => _characterName;
    public int Health => _health;
    public float Speed => _speed;

    // Acceso a la direcci�n (esto puede cambiar din�micamente en gameplay)
    public Vector2 Direction
    {
        get => _direction;
        set => _direction = value;
    }
}
