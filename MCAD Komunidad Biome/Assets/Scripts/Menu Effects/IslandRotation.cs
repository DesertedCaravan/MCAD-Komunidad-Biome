using UnityEngine;

public class IslandRotation : MonoBehaviour
{
    [SerializeField] private GameObject islandGameObject;

    // Update is called once per frame
    void Update()
    {
        islandGameObject.transform.Rotate(new Vector3(0, Time.deltaTime, 0), Space.Self);
    }
}