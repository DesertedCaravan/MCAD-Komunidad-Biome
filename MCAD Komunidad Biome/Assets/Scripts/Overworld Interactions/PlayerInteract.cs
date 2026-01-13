using UnityEngine;
using UnityEngine.InputSystem;

interface IInteractable
{
    public void Interact();
}

public class PlayerInteract : MonoBehaviour
{
    // Reference: https://stackoverflow.com/questions/45983775/making-an-object-detect-that-raycast-not-hitting-it-anymore

    [Header("Interactor Data")]
    [SerializeField] private Transform interactorSource;
    [SerializeField] private float interactRange;

    [Header("Key Binds")]
    [SerializeField] private Key interactKey; // SpaceKey

    [SerializeField] private InteractUI interactOfFocus = null;

    void Update()
    {
        Debug.DrawRay(interactorSource.position, interactorSource.forward * interactRange, Color.green); // forward is just 1, so needs to be multiplied by interactRange

        Ray r = new Ray(interactorSource.position, interactorSource.forward);

        if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
        {
            GameObject hitObject = hitInfo.transform.gameObject;

            // Debug.Log(hitObject.name);

            if (hitObject.GetComponent<InteractUI>() != null)
            {
                // Stop Looking at Previous Game Object
                if (interactOfFocus != null)
                {
                    interactOfFocus.NotLookingAt();
                }

                // Start Looking at Current Game Object
                interactOfFocus = hitObject.GetComponent<InteractUI>();
                interactOfFocus.CurrentlyLookingAt();
            }

            // Interact with Interact() if able // Removed Input.GetMouseButtonDown(0)
            if ((Mouse.current.leftButton.wasPressedThisFrame || Keyboard.current[interactKey].wasPressedThisFrame) && hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                interactObj.Interact();
            }
        }
        else // If the raycast hits nothing
        {
            // Stop Looking at Current Game Object if out of range
            if (interactOfFocus != null)
            {
                interactOfFocus.NotLookingAt();
                interactOfFocus = null; // to avoid calling this line repeatedly
            }
        }
    }
}