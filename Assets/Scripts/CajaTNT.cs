using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CajaTNT : MonoBehaviour
{
    // Todas las cajas activas de la escena (para la reaccion en cadena)
    private static readonly List<CajaTNT> todas = new List<CajaTNT>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void LimpiarLista() { todas.Clear(); }

    [Header("Cuenta regresiva (CA1)")]
    [SerializeField] private float segundosCuentaRegresiva = 3f;
    [SerializeField] private TextMeshPro textoCuenta;          
    [SerializeField] private string formatoNumero = "{0}..."; 

    [Header("Deteccion de pisada")]
    [Header("Deteccion de pisada")]
    [SerializeField] private Collider colliderSolido;
    [SerializeField] private float toleranciaPies = 0.25f;
    [Tooltip("Tildado: solo si pisa desde arriba. Destildado: cualquier contacto activa la cuenta.")]
    [SerializeField] private bool soloDesdeArriba = false;

    [Header("Explosion y cadena (CA3)")]
    [SerializeField] private float radioCadena = 1.8f;         
    [SerializeField] private float retrasoCadena = 0.25f;      
    [SerializeField] private float radioDano = 3f;
    [SerializeField] private bool danaAlJugador = true;
    [SerializeField] private GameObject efectoExplosion;       

    [Header("Parpadeo de alerta")]
    [SerializeField] private Renderer[] renderersVisuales;
    [SerializeField] private Color colorAlerta = Color.red;


    private static float ultimoDanoAJugador = -10f;
    private const float CooldownDanoGlobal = 1.5f;
    private bool activada;
    private bool explotada;
    private bool esCadena;
    private float restante;
    private float total;
    private float reloj;
    private MaterialPropertyBlock bloque;
    private Transform camara;

    void OnEnable() { todas.Add(this); }
    void OnDisable() { todas.Remove(this); }

    void Awake()
    {
        if (textoCuenta != null)
            textoCuenta.gameObject.SetActive(false);
    }

    // Trigger fino sobre la cara superior de la caja
    private void OnTriggerEnter(Collider other)
    {
        if (activada || explotada) return;
        if (other.GetComponentInParent<PlayerHealth>() == null) return;

        if (soloDesdeArriba && colliderSolido != null &&
            other.bounds.min.y < colliderSolido.bounds.max.y - toleranciaPies) return;

        Activar(segundosCuentaRegresiva);
    }

    public void Activar(float segundos, bool porCadena = false)
    {
        if (explotada) return;
        if (activada && restante <= segundos) return; // ya va a explotar antes

        if (!activada)
        {
            activada = true;
            total = segundos;
            restante = segundos;
            if (textoCuenta != null)
            {
                textoCuenta.gameObject.SetActive(true);
                textoCuenta.text = string.Format(formatoNumero, Mathf.CeilToInt(segundos));
            }
        }

        restante = segundos;
        if (porCadena) esCadena = true;
    }

    void Update()
    {
        if (!activada || explotada) return;

        restante -= Time.deltaTime;
        reloj += Time.deltaTime;

        if (restante <= 0f)
        {
            Explotar();
            return;
        }

        ActualizarTexto();
        Parpadear();
    }

    void LateUpdate()
    {
        if (textoCuenta == null || !textoCuenta.gameObject.activeSelf) return;

        if (camara == null)
        {
            Camera cam = Camera.main != null ? Camera.main : FindFirstObjectByType<Camera>();
            if (cam != null) camara = cam.transform;
        }

        if (camara != null)
        {
            
            Vector3 haciaCamara = textoCuenta.transform.position - camara.position;
            if (haciaCamara.sqrMagnitude > 0.001f)
                textoCuenta.transform.rotation = Quaternion.LookRotation(haciaCamara);
        }
    }

    private void ActualizarTexto()
    {
        if (textoCuenta == null) return;
        textoCuenta.text = esCadena ? "!" : string.Format(formatoNumero, Mathf.CeilToInt(restante));
    }

    private void Parpadear()
    {
        if (renderersVisuales == null) return;

        float progreso = 1f - Mathf.Clamp01(restante / Mathf.Max(0.01f, total));
        bool encendido = Mathf.Sin(reloj * (8f + 14f * progreso)) > 0f; // parpadea cada vez mas rapido

        bloque ??= new MaterialPropertyBlock();
        foreach (Renderer r in renderersVisuales)
        {
            if (r == null) continue;
            if (encendido)
            {
                r.GetPropertyBlock(bloque);
                bloque.SetColor("_BaseColor", colorAlerta); // URP
                bloque.SetColor("_Color", colorAlerta);     // Built-in
                r.SetPropertyBlock(bloque);
            }
            else
            {
                r.SetPropertyBlock(null);
            }
        }
    }

    private void Explotar()
    {
        explotada = true;
        todas.Remove(this);

        if (efectoExplosion != null)
            Instantiate(efectoExplosion, transform.position, Quaternion.identity);

        if (danaAlJugador && Time.time >= ultimoDanoAJugador + CooldownDanoGlobal)
        {
            foreach (PlayerHealth jugador in FindObjectsByType<PlayerHealth>())
            {
                if (Vector3.Distance(jugador.transform.position, transform.position) <= radioDano)
                {
                    jugador.perderVida();
                    ultimoDanoAJugador = Time.time;
                }
            }
        }

        // Reaccion en cadena: las cajas colindantes explotan enseguida
        foreach (CajaTNT otra in todas.ToArray())
        {
            if (otra == null || otra == this) continue;
            if (Vector3.Distance(otra.transform.position, transform.position) <= radioCadena)
                otra.Activar(retrasoCadena, true);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioCadena);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDano);
    }
}