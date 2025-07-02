using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    // facon o boleadoras
    public string objectiveName; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            QuestManager questManager = FindFirstObjectByType<QuestManager>();
            if (questManager != null)
            {
                questManager.CompleteObjective(objectiveName);
            }

            Destroy(gameObject);
        }
    }
}