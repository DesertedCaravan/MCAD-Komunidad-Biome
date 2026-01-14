using UnityEngine;

public class IslandRotation : MonoBehaviour
{
    [Header("Island Game Object")]
    [SerializeField] private GameObject islandGameObject;

    [Header("Size Variables")]
    [SerializeField] private float zPosVariable; // 0.01667f // 0 => -4 in 240 seconds
    [SerializeField] private float zoomVariable; // 0.00416f // 1 => 2 in 240 seconds

    private float islandZPosVariable;
    private bool islandZoom;
    [SerializeField] private float counter;

    void Start()
    {
        islandZoom = false;

        islandZPosVariable = 0f;
        SetZoom(islandZPosVariable);

        islandGameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        counter = 0f;
    }

    void Update()
    {
        islandGameObject.transform.Rotate(new Vector3(0, Time.deltaTime, 0), Space.Self);

        if (islandZoom == true)
        {
            islandZPosVariable -= zPosVariable * Time.deltaTime;
            SetZoom(islandZPosVariable);

            islandGameObject.transform.localScale += new Vector3(zoomVariable, zoomVariable, zoomVariable) * Time.deltaTime;

            counter += Time.deltaTime;

            if (counter >= 240)
            {
                islandZoom = false;
            }
        }
    }

    public void ZoomIn()
    {
        islandZoom = true;
    }

    public void SetZoom(float x)
    {
        Vector3 pos = islandGameObject.transform.position;
        pos.z = x;
        islandGameObject.transform.position = pos;
    }

}