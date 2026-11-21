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
        Vida
    }

    public TipoTutorial tipoTutorial;

    public TutorialUI tutorialPadre;
    public GameObject panel;

    [Header("Inventario")]
    [Tooltip("Opcional: panel del inventario. Si se asigna, el paso se completa cuando se abre.")]
    [SerializeField] private GameObject panelInventario;
    [Tooltip("Si no hay panel asignado, el paso se completa al apretar esta tecla.")]
    [SerializeField] private Key teclaInventario = Key.I;

    private bool tutorialTerminado = false;
    private EnemyHealth[] enemigosAlEmpezar;
    private bool saltoEnSuelo;
    private bool powerUpActivoAlEmpezar;

    private void Start()
    {
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

        yield return new WaitForSeconds(3);
        empezarTutorial();
    }

    public void empezarTutorial()
    {
        saltoEnSuelo = false;

        if (tipoTutorial == TipoTutorial.EliminarEnemigo)
            enemigosAlEmpezar = FindObjectsByType<EnemyHealth>(FindObjectsInactive.Include);

        if (tipoTutorial == TipoTutorial.PowerUp)
        {
            PlayerPowerUps p = FindAnyObjectByType<PlayerPowerUps>();
            powerUpActivoAlEmpezar = p != null && p.HayPowerUpActivo;
        }

        panel.SetActive(true);
        StartCoroutine(EsperarInput());
    }

    IEnumerator EsperarInput()
    {
        yield return new WaitForSeconds(5);

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
                bool teclado_Movimiento = Keyboard.current.wKey.isPressed || Keyboard.current.aKey.isPressed ||
                                        Keyboard.current.sKey.isPressed || Keyboard.current.dKey.isPressed;

                bool joystick_Movimiento = Gamepad.current != null &&
                                            Gamepad.current.leftStick.ReadValue().magnitude > 0.1f;

                return teclado_Movimiento || joystick_Movimiento;

            case TipoTutorial.Salto:
                bool teclado_Salto = Keyboard.current.spaceKey.isPressed;
                bool joystick_Salto = Gamepad.current != null && Gamepad.current.buttonSouth.isPressed;

                return teclado_Salto || joystick_Salto;

            case TipoTutorial.Correr:
                bool teclado_Correr = (Keyboard.current.wKey.isPressed && Keyboard.current.leftShiftKey.isPressed) ||
                                    (Keyboard.current.aKey.isPressed && Keyboard.current.leftShiftKey.isPressed) ||
                                    (Keyboard.current.sKey.isPressed && Keyboard.current.leftShiftKey.isPressed) ||
                                    (Keyboard.current.dKey.isPressed && Keyboard.current.leftShiftKey.isPressed);

                bool joystick_Correr = Gamepad.current != null &&
                                        Gamepad.current.leftStick.ReadValue().magnitude > 0.1f &&
                                        Gamepad.current.leftStickButton.isPressed;

            return teclado_Correr || joystick_Correr;

            case TipoTutorial.Monedas:
                return playerInventory != null && playerInventory.cantFichas > 0;

            case TipoTutorial.Vida:
            return true;

            default:
                return false;
}
    }
    private bool SeEliminoUnEnemigo()
    {
        if (enemigosAlEmpezar == null) return false;

        foreach (EnemyHealth enemigo in enemigosAlEmpezar)
        {
            if (enemigo == null) return true; 
        }

        return false;
    }
}