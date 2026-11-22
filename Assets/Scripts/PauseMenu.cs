using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private const string NombreEscenaMenuPrincipal = "MenuPrincipal";

    public static bool JuegoEsPausado = false;
    public static bool UIBloqueaCursor = false; 

    public GameObject pauseMenuUI;
    public GameObject opcionesUI;
    [SerializeField] private Button botonControles;
    [SerializeField] private GameObject controlesUI;
    [SerializeField] private Toggle toggleMostrarFps;
    private bool volverAOpcionesDesdeControles;
    public GameObject creditosUI;

    [SerializeField] private string saveSelectionSceneName = "SaveSelection";
    [SerializeField] private AchievementMenuUI achievementMenu;
    public static string EscenaPreviaCreditos = "";

    private bool EsMenuPrincipal()
    {
        return SceneManager.GetActiveScene().name == NombreEscenaMenuPrincipal;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.JuegoTerminado)
            return;

        if (EsMenuPrincipal())
        {
            if (Cursor.lockState != CursorLockMode.None || !Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            return;
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
    {
        if (controlesUI != null && controlesUI.activeSelf)
        {
            ControlsMenuUI controlsMenu = FindAnyObjectByType<ControlsMenuUI>(FindObjectsInactive.Include);
            if (controlsMenu != null && controlsMenu.CancelRebindingIfActive())
            {
                return;
            }

            CerrarControles();
        }
        else if (opcionesUI != null && opcionesUI.activeSelf)
        {
            CerrarOpciones();
        }
        else if (creditosUI != null && creditosUI.activeSelf)
            {
                CerrarCreditos();
            }
        else if (JuegoEsPausado)
            {
            Reanudar();
            }
        else
        {
        Pausa();
        }
}

        if (!JuegoEsPausado && !UIBloqueaCursor && Mouse.current.leftButton.wasPressedThisFrame && Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Start()
    {
        if (botonControles != null)
        {
            botonControles.onClick.AddListener(AbrirControles);
        }

        if (toggleMostrarFps != null)
        {
            bool mostrarFps = ContadorFps.EstaVisible();
            toggleMostrarFps.SetIsOnWithoutNotify(mostrarFps);
            toggleMostrarFps.onValueChanged.AddListener(CambiarVisibilidadFps);
        }

        if (controlesUI != null)
        {
            controlesUI.SetActive(false);
        }
        if (pauseMenuUI != null)
        {
            AchievementSystem achievementSystem = AchievementSystem.Instance;
            if (achievementSystem == null)
            {
                achievementSystem = FindAnyObjectByType<AchievementSystem>();
            }

            if (achievementSystem == null)
            {
                achievementSystem = gameObject.AddComponent<AchievementSystem>();
            }

            if (achievementMenu == null)
            {
                achievementMenu = GetComponent<AchievementMenuUI>();
            }

            if (achievementMenu != null)
            {
                achievementMenu.Initialize(achievementSystem, this);
            }
            pauseMenuUI.SetActive(false);
        }

        Time.timeScale = 1f;
        JuegoEsPausado = false;

        if (EsMenuPrincipal())
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void OnDestroy()
    {
        if (botonControles != null)
        {
            botonControles.onClick.RemoveListener(AbrirControles);
        }

        if (toggleMostrarFps != null)
        {
            toggleMostrarFps.onValueChanged.RemoveListener(CambiarVisibilidadFps);
        }
    }

    private void CambiarVisibilidadFps(bool mostrar)
    {
        ContadorFps.SetVisible(mostrar);
    }

    public void AbrirOpciones()
    {
        pauseMenuUI.SetActive(false);
        opcionesUI.SetActive(true);
    }
    public void AbrirControles()
    {
        if (controlesUI == null)
        {
            Debug.LogWarning("Asigna el panel de controles en PauseMenu.", this);
            return;
        }

        volverAOpcionesDesdeControles = opcionesUI != null && opcionesUI.activeSelf;
        if (volverAOpcionesDesdeControles)
        {
            opcionesUI.SetActive(false);
        }

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }

        controlesUI.SetActive(true);
    }

    public void CerrarControles()
    {
        if (controlesUI != null)
        {
            controlesUI.SetActive(false);
        }

        if (volverAOpcionesDesdeControles && opcionesUI != null)
        {
            opcionesUI.SetActive(true);
        }
        else if (pauseMenuUI != null && JuegoEsPausado)
        {
            pauseMenuUI.SetActive(true);
        }

        volverAOpcionesDesdeControles = false;
    }
    public void CerrarOpciones()
    {
        opcionesUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void AbrirLogros()
    {
        achievementMenu?.Open();
    }

    public void CerrarLogros()
    {
        achievementMenu?.Close();
    }

    public void Reanudar()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        JuegoEsPausado = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = EsMenuPrincipal();
    }

    void Pausa()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        JuegoEsPausado = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public void SalirDelJuego()
    {
        VolverASeleccion();
    }

    private void VolverASeleccion()
    {
        Time.timeScale = 1f;
        JuegoEsPausado = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(saveSelectionSceneName);
    }

public void AbrirCreditos()
{
    if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
    if (creditosUI != null) creditosUI.SetActive(true);
}

public void CerrarCreditos()
{
    if (creditosUI != null) creditosUI.SetActive(false);
    if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
}

}

public class ContadorFps : MonoBehaviour
{
    private const string MostrarFpsKey = "MostrarFPS";
    private static ContadorFps instancia;
    private static GUIStyle estiloContador;

    private bool visible;
    private int fpsActual;
    private int framesContados;
    private float tiempoTranscurrido;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ReiniciarInstancia()
    {
        instancia = null;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CrearInstancia()
    {
        if (instancia == null)
            new GameObject(nameof(ContadorFps)).AddComponent<ContadorFps>();
    }

    private void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;
        DontDestroyOnLoad(gameObject);
        visible = EstaVisible();
    }

    private void Update()
    {
        if (!visible)
            return;

        framesContados++;
        tiempoTranscurrido += Time.unscaledDeltaTime;
        if (tiempoTranscurrido >= 0.25f)
        {
            fpsActual = Mathf.RoundToInt(framesContados / tiempoTranscurrido);
            framesContados = 0;
            tiempoTranscurrido = 0f;
        }
    }

    private void OnGUI()
    {
        if (!visible)
            return;

        if (estiloContador == null)
        {
            estiloContador = new GUIStyle(GUI.skin.label)
            {
                fontSize = 24,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft
            };
        }

        string texto = $"{fpsActual} FPS";
        estiloContador.normal.textColor = Color.black;
        GUI.Label(new Rect(13f, 13f, 150f, 38f), texto, estiloContador);
        estiloContador.normal.textColor = Color.white;
        GUI.Label(new Rect(11f, 11f, 150f, 38f), texto, estiloContador);
    }

    public static bool EstaVisible()
    {
        return PlayerPrefs.GetInt(MostrarFpsKey, 0) == 1;
    }

    public static void SetVisible(bool mostrar)
    {
        if (instancia == null)
            new GameObject(nameof(ContadorFps)).AddComponent<ContadorFps>();

        instancia.visible = mostrar;
        PlayerPrefs.SetInt(MostrarFpsKey, mostrar ? 1 : 0);
        PlayerPrefs.Save();
    }
}