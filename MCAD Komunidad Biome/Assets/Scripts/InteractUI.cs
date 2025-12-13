using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [Header("Interactable Icon Data")]
    [SerializeField] private GameObject interactableIcon;
    private bool isInteracted;

    public bool IsInteracted => isInteracted;

    void Start()
    {
        interactableIcon.SetActive(false);
    }

    void Update()
    {
        if (interactableIcon.activeSelf)
        {
            Vector3 direction = MainManager.instance.Controller.gameObject.transform.position - transform.position;

            if (direction.sqrMagnitude < 0.0001f)
                return;

            direction.y = 0;

            // Rotate to face the target
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }

    public void NotInteracted()
    {
        isInteracted = false;
        interactableIcon.SetActive(false);
    }

    public void SetInteracted()
    {
        interactableIcon.SetActive(false);
        isInteracted = true;
    }

    public void CurrentlyLookingAt()
    {
        if (isInteracted == false)
        {
            interactableIcon.SetActive(true);
        }
    }

    public void NotLookingAt()
    {
        interactableIcon.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null && isInteracted == false)
        {
            interactableIcon.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null && isInteracted == false)
        {
            interactableIcon.SetActive(false);
        }
    }
}