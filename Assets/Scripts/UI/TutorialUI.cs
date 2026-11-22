using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialUI : MonoBehaviour
{
    public enum TipoTutorial
    {
        Movimiento,
        Salto,
        Correr,
        Monedas,
        Vida,
        DobleSalto,
        EliminarEnemigo,
        Inventario,
        PowerUp
    }

    public TipoTutorial tipoTutorial;
    public TutorialUI tutorialPadre;
    public GameObject panel;
    private PlayerInventory playerInventory;

    [Header("Inventario")]
    [Tooltip("Opcional: panel del inventario. Si se asigna, el paso se completa cuando se abre.")]
    [SerializeField] private GameObject panelInventario;
    [Tooltip("Si no hay panel asignado, el paso se completa al apretar esta tecla.")]
    [SerializeField] private Key teclaInventario = Key.I;

    [Header("Eliminar enemigo")]
    [Tooltip("Enemigos de la escena. El paso se completa cuando alguno es destruido o desactivado.")]
    [SerializeField] private GameObject[] enemigosObjetivo;

    [Header("Power-up")]
    [Tooltip("Power-ups de la escena. El paso se completa cuando alguno es recogido (destruido o desactivado).")]
    [SerializeField] private GameObject[] powerUpsObjetivo;

    private bool tutorialTerminado = false;
    private PlayerMovement playerMovement;
    private CharacterController playerController;

    private void Start()
    {
        playerInventory = FindAnyObjectByType<PlayerInventory>();

        panel.SetActive(false);

        if (tutorialPadre == null)
        {
            empezarTutorial();
        }
        else
        {
            StartCoroutine(EsperarTutorialPadre());
        }
    }

    IEnumerator EsperarTutorialPadre()
    {
        while (!tutorialPadre.tutorialTerminado)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2);
        empezarTutorial();
    }

    public void empezarTutorial()
    {
        panel.SetActive(true);
        StartCoroutine(EsperarInput());
    }

    IEnumerator EsperarInput()
    {
        yield return new WaitForSeconds(2);

        while (!CondicionCompletada())
        {
            yield return null;
        }

        panel.SetActive(false);

        tutorialTerminado = true;
    }

    bool CondicionCompletada()
    {
        Keyboard kb = Keyboard.current;
        Gamepad pad = Gamepad.current;

        switch (tipoTutorial)
        {
            case TipoTutorial.Movimiento:
                bool teclado_Movimiento = kb != null &&
                    (kb.wKey.isPressed || kb.aKey.isPressed || kb.sKey.isPressed || kb.dKey.isPressed);
                bool joystick_Movimiento = pad != null && pad.leftStick.ReadValue().magnitude > 0.1f;

                return teclado_Movimiento || joystick_Movimiento;

            case TipoTutorial.Salto:
                bool teclado_Salto = kb != null && kb.spaceKey.isPressed;
                bool joystick_Salto = pad != null && pad.buttonSouth.isPressed;

                return teclado_Salto || joystick_Salto;

            case TipoTutorial.Correr:
                bool teclado_Correr = kb != null && kb.leftShiftKey.isPressed &&
                    (kb.wKey.isPressed || kb.aKey.isPressed || kb.sKey.isPressed || kb.dKey.isPressed);
                bool joystick_Correr = pad != null &&
                    pad.leftStick.ReadValue().magnitude > 0.1f &&
                    pad.leftStickButton.isPressed;

                return teclado_Correr || joystick_Correr;

            case TipoTutorial.Monedas:
                return playerInventory != null && playerInventory.cantFichas > 0;

            case TipoTutorial.Vida:
                return true;

            case TipoTutorial.DobleSalto:
                if (playerMovement == null)
                {
                    playerMovement = FindAnyObjectByType<PlayerMovement>();
                    if (playerMovement != null)
                        playerController = playerMovement.GetComponent<CharacterController>();
                }
                bool enElAire = playerController != null && !playerController.isGrounded;
                bool teclado_DobleSalto = kb != null && kb.spaceKey.wasPressedThisFrame && enElAire;
                bool joystick_DobleSalto = pad != null && pad.buttonSouth.wasPressedThisFrame && enElAire;

                return teclado_DobleSalto || joystick_DobleSalto;

            case TipoTutorial.EliminarEnemigo:
                return AlgunoDesaparecio(enemigosObjetivo);

            case TipoTutorial.Inventario:
                if (panelInventario != null)
                    return panelInventario.activeInHierarchy;
                return kb != null && kb[teclaInventario].wasPressedThisFrame;

            case TipoTutorial.PowerUp:
                return AlgunoDesaparecio(powerUpsObjetivo);

            default:
                return false;
        }
    }

    private bool AlgunoDesaparecio(GameObject[] objetos)
    {
        if (objetos == null || objetos.Length == 0) return false;

        foreach (GameObject obj in objetos)
        {
            if (obj == null || !obj.activeInHierarchy)
                return true; // destruido o desactivado
        }

        return false;
    }
}