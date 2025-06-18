using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "NPC/NPCData")]
public class NPCData : ScriptableObject
{
    public string npcName;
    public Sprite portrait;
    public RuntimeAnimatorController baseController; // un Animator base con un estado "Idle"
    public AnimationClip idleClip; // clip individual de este NPC
}
