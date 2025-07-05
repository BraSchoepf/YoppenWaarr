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

    [SerializeField] private ParticleSystem _sparks;
    [SerializeField] private Collider2D _sparksCollider;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject auraPadre;

    [SerializeField] private ParticleSystem _attackEffect;

    private int lastStep = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        if (_sparks != null)
        {
            var noise = _sparks.noise;
            noise.enabled = true;
            noise.strength = -1.8f;
            Debug.Log("Noise seteado a -3 en Start");
        }

        lastStep = 0;
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

        if (!bossActivado)
        {
            int step = (vidaMaxima - vidaActual) / 20;
            if (step > lastStep)
            {
                UpdateAuraNoise(step);
                lastStep = step;
            }
        }

    if (!bossActivado && vidaActual <= vidaParaActivar)
        {
            DesactiveAura();
            ActivarBoss();
        }
    }

    private void UpdateAuraNoise(int step)
    {
        if (_sparks == null) return;

        var noise = _sparks.noise;
        noise.enabled = true;
        noise.strength = -step * 2f;

        Debug.Log("Noise actualizado a: " + noise.strength.constant);
    }

    private void DesactiveAura()
    {
        if (auraPadre != null)
        {
            auraPadre.SetActive(false); // desactiva partícula + collider + scripts
            Debug.Log("Aura completa desactivada");
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

    public void PlayAttackEffect()
    {
        if (_attackEffect == null)
        {
            Debug.LogWarning("Falta la partícula del ataque 2.");
            return;
        }

        _attackEffect.Play();
    }


}


