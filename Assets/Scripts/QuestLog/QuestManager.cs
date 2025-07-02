using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public TextMeshProUGUI questLogText;

    private bool _faconDone = false;
    private bool _boleadorasDone = false;
    private bool _doorDone = false;
    private bool _xalpenDone = false;

    private bool _combatPhase = false;

    void Start()
    {
        UpdateQuestLog();
    }

    public void CompleteObjective(string objectiveName)
    {
        if (!_combatPhase)
        {
            if (objectiveName == "facon") _faconDone = true;
            else if (objectiveName == "boleadoras") _boleadorasDone = true;
            else if (objectiveName == "door")
            {
                _doorDone = true;
                _combatPhase = true;
            }
        }
        else
        {
            if (objectiveName == "xalpen") _xalpenDone = true;
        }

        UpdateQuestLog();
    }

    private void UpdateQuestLog()
    {
        string log = "";

        if (!_combatPhase)
        {
            log += _faconDone ? "-(LISTO) Practicar con el facon en el corral oeste.\n" : "-(Opcional) Practicar con el facon en el corral oeste.\n";
            log += _boleadorasDone ? "-(LISTO) Practicar con boleadoras en el corral este.\n" : "-(Opcional) Practicar con boleadoras en el corral este.\n";
            log += _doorDone ? "-(LISTO)Ve a la puerta sur y habla con Cara Seca para avanzar.\n" : "-Ve a la puerta sur y habla con Cara Seca para avanzar.\n";
        }
        else
        {
            log += _xalpenDone ? "-(LISTO) Derrota a Xalpen y sus grupos de -Marcados-.\n" : "-Derrota a Xalpen y sus grupos de -Marcados-.\n";
        }

        questLogText.text = log;
    }
}
