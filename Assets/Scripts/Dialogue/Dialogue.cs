using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/New Dialogue")]
public class Dialogue : ScriptableObject
{
    public string npcName;
    [TextArea(3, 10)] public string[] lines;
}