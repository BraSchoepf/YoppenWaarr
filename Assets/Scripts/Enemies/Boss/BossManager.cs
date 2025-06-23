using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using FMODUnity;
using System.Collections;
using Unity.VisualScripting;
using System.Data;

public class BossManager : MonoBehaviour
{
    public static BossManager Instance;

    public BossAI bossAI;
    public Image barraEnergia;
    public int vidaParaActivar = 80;
    private bool bossActivado = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        
    }
    public void ActualizarBarra(int vidaActual, int vidaMaxima)
    {
        if (barraEnergia != null)
        {
            float fill = (float)vidaActual / vidaMaxima;
            barraEnergia.fillAmount = Mathf.Clamp01(fill);

            if (Mathf.Abs(barraEnergia.fillAmount - fill) < 0.02f)
            {
                barraEnergia.fillAmount = fill - 0.02f;
            }
        }

    if (!bossActivado && vidaActual <= vidaParaActivar)
        {
            ActivarBoss();
        }
    }



    public void ReducirVida(int cantidad)
    {
        GetComponent<BossHealth>()?.TakePureDamage(cantidad);


    }

    private void ActivarBoss()
    {
        bossActivado = true;
        bossAI?.MoveTowardsPlayer();
        Debug.Log("Boss activado por BossManager");
    }


    public void EnableDamage() => GetComponent<BossHealth>()?.EnableDamage();
    public void DisableDamage() => GetComponent<BossHealth>()?.DisableDamage();


}


