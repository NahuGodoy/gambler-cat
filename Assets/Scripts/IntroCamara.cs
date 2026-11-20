using UnityEngine;
using UnityEngine.InputSystem;

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
        if (Keyboard.current.spaceKey.wasPressedThisFrame){
            if (indice < views.Length - 1){
                indice++;
                currentView = views[indice];
            }
        }
    }

    private void LateUpdate()
    {
        Vector3 destino = new Vector3(currentView.position.x, currentView.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, destino, Time.deltaTime * transitionSpeed);
    }
}
