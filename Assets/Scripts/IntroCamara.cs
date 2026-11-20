using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class IntroCamara : MonoBehaviour
{
    public Transform[] views;
    public float transitionSpeed;
    Transform currentView;
    int indice = -1;

    void Start()
    {
        currentView = transform;
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            AvanzarVista();
        }
    }

    public void AvanzarVista()
    {
        if (views == null || views.Length == 0)
            return;

        if (indice >= views.Length - 1)
        {
            IrASeleccion();
            return;
        }

        indice++;
        currentView = views[indice];
    }

    public void OmitirIntro()
    {
        IrASeleccion();
    }

    private void IrASeleccion()
    {
        SceneManager.LoadScene("SaveSelection");
    }

    private void LateUpdate()
    {
        Vector3 destino = new Vector3(currentView.position.x, currentView.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, destino, Time.deltaTime * transitionSpeed);
    }
}
