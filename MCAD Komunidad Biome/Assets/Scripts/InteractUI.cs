using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [Header("Interactable Icon Data")]
    [SerializeField] private GameObject interactableIcon;
    private bool _interacting;

    void Start()
    {
        interactableIcon.SetActive(false);
        _interacting = false;
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

    public void StartedInteracting()
    {
        _interacting = true;
        interactableIcon.SetActive(false);
    }

    public void FinishedInteracting()
    {
        _interacting = false;
        interactableIcon.SetActive(true);
    }

    public void CurrentlyLookingAt()
    {
        if (_interacting == false)
        {
            interactableIcon.SetActive(true);
        }
    }

    public void NotLookingAt()
    {
        if (_interacting == false)
        {
            interactableIcon.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            interactableIcon.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            interactableIcon.SetActive(false);
        }
    }
}