using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Pregame_Decorativescroll : MonoBehaviour
{
    [Header("Texto")]
    [SerializeField] private string[] frases;
    [SerializeField] private TextMeshProUGUI textoUI;

    [Header("Botones")]
    [SerializeField] private Button botonSiguiente;
    [SerializeField] private Button botonSaltar;
    [SerializeField] private Button botonAtras;

    [Header("Decoraciones")]
    [SerializeField] private RectTransform areaSpawn;
    [SerializeField] private GameObject[] decoracionesLaterales;
    [SerializeField] private GameObject[] decoracionesCentrales;
    [SerializeField] private GameObject prefabHuellas;
    [SerializeField] private float intervaloSpawnLaterales = 0.5f;
    [SerializeField] private float intervaloSpawnCentrales = 0.6f;
    [SerializeField] private float intervaloSpawnHuellas = 0.7f;
    [SerializeField] private float velocidadCaida = 200f;

    [Header("Configuración de escenas")]
    [SerializeField] private string escenaJuego = "SceneTutorial";
    [SerializeField] private string escenaMenu = "MainMenu";

    private int indiceFrase = 0;
    private List<GameObject> elementosEnPantalla = new List<GameObject>();

    private void Start()
    {
        botonSiguiente.onClick.AddListener(MostrarSiguienteFrase);
        botonSaltar.onClick.AddListener(() => SceneManager.LoadScene(escenaJuego));
        botonAtras.onClick.AddListener(MostrarFraseAnterior);

        textoUI.text = frases[indiceFrase];

        InvokeRepeating(nameof(SpawnDecoracionLaterales), 0f, intervaloSpawnLaterales);
        InvokeRepeating(nameof(SpawnDecoracionCentrales), 0f, intervaloSpawnCentrales);
        InvokeRepeating(nameof(SpawnHuellas), 0f, intervaloSpawnHuellas);
    }

    private void Update()
    {
        MoverDecoraciones();
    }

    private void MostrarSiguienteFrase()
    {
        indiceFrase++;
        if (indiceFrase >= frases.Length)
        {
            SceneManager.LoadScene(escenaJuego);
        }
        else
        {
            textoUI.text = frases[indiceFrase];
        }
    }

    private void MostrarFraseAnterior()
    {
        if (indiceFrase == 0)
        {
            SceneManager.LoadScene(escenaMenu);
        }
        else
        {
            indiceFrase--;
            textoUI.text = frases[indiceFrase];
        }
    }

    private void SpawnDecoracionLaterales()
    {
        // Select left or right area randomly, with some variation for more juice
        float baseX = Random.value < 0.5f ? 0.25f : 0.75f; // 0.25 (left zone), 0.75 (right zone)
        float offset = Random.Range(-0.05f, 0.05f); // Adds variation to avoid uniformity
        float posXViewport = Mathf.Clamp01(baseX + offset);
        float posYViewport = 1.2f; // Offscreen (above the camera view)

        Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(posXViewport, posYViewport, 10f));

        GameObject prefab = decoracionesLaterales[Random.Range(0, decoracionesLaterales.Length)];
        CrearElemento(prefab, spawnPosition);
    }

    private void SpawnDecoracionCentrales()
    {
        // Random X position in the central area (around the center)
        float posXViewport = Random.Range(0.35f, 0.65f);
        float posYViewport = 1.2f;

        Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(posXViewport, posYViewport, 10f));

        GameObject prefab = decoracionesCentrales[Random.Range(0, decoracionesCentrales.Length)];
        CrearElemento(prefab, spawnPosition);
    }

    private void SpawnHuellas()
    {
        // Always spawns in the middle
        float posXViewport = 0.5f;
        float posYViewport = 1.2f;

        Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(posXViewport, posYViewport, 10f));

        CrearElemento(prefabHuellas, spawnPosition);
    }

    private void CrearElemento(GameObject prefab, Vector3 posicionMundo)
    {
        GameObject instancia = Instantiate(prefab, areaSpawn);
        RectTransform rect = instancia.GetComponent<RectTransform>();

        if (rect != null)
        {
            rect.position = posicionMundo; // UI element (Image)
        }
        else
        {
            instancia.transform.position = posicionMundo; // SpriteRenderer or regular GameObject
        }

        elementosEnPantalla.Add(instancia);
    }

    private void MoverDecoraciones()
    {
        for (int i = elementosEnPantalla.Count - 1; i >= 0; i--)
        {
            GameObject obj = elementosEnPantalla[i];

            RectTransform rect = obj.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.position += Vector3.down * velocidadCaida * Time.deltaTime;

                if (rect.position.y < Camera.main.ViewportToWorldPoint(Vector3.zero).y - 2f)
                {
                    Destroy(obj);
                    elementosEnPantalla.RemoveAt(i);
                }
            }
            else
            {
                obj.transform.position += Vector3.down * velocidadCaida * Time.deltaTime * 0.01f; // Adjust if necessary
                if (obj.transform.position.y < Camera.main.ViewportToWorldPoint(Vector3.zero).y - 2f)
                {
                    Destroy(obj);
                    elementosEnPantalla.RemoveAt(i);
                }
            }
        }
    }
}