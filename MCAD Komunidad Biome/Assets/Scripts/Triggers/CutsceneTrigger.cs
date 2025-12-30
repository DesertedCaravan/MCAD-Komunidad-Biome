using UnityEngine;
using UnityEngine.Events;

public class CutsceneTrigger : MonoBehaviour
{
    [Header("Response Events")]
    [SerializeField] private UnityEvent onTriggerResponse;

    private bool _wasTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null && _wasTriggered == false)
        {
            _wasTriggered = true;

            onTriggerResponse.Invoke();
        }
    }
}