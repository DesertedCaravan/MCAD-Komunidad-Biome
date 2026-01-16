using UnityEngine;

public class IslandRotation : MonoBehaviour
{
    [Header("Island Game Object")]
    [SerializeField] private GameObject islandGameObject;

    [Header("Size Variables")]
    [SerializeField] private float _yPosVariable; // 0.00416f // -5 => -4 in 240 seconds
    [SerializeField] private float _zPosVariable; // 0.01667f // 0 => -4 in 240 seconds
    [SerializeField] private float _zoomVariable; // 0.00416f // 1 => 2 in 240 seconds
    private float _islandYPosVariable;
    private float _islandZPosVariable;
    private bool _islandZoom;
    private float _counter;

    void Start()
    {
        _islandZoom = false;

        _islandYPosVariable = -5f;
        _islandZPosVariable = 0f;

        SetZoom();
        islandGameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        _counter = 0f;
    }

    void Update()
    {
        islandGameObject.transform.Rotate(new Vector3(0, Time.deltaTime, 0), Space.Self);

        if (_islandZoom == true)
        {
            _islandYPosVariable += _yPosVariable * Time.deltaTime;
            _islandZPosVariable -= _zPosVariable * Time.deltaTime;
            SetZoom();

            islandGameObject.transform.localScale += new Vector3(_zoomVariable, _zoomVariable, _zoomVariable) * Time.deltaTime;

            _counter += Time.deltaTime;

            if (_counter >= 240)
            {
                _islandZoom = false;
            }
        }
    }

    public void ZoomIn()
    {
        _islandZoom = true;
    }

    public void SetZoom()
    {
        Vector3 pos = islandGameObject.transform.position;
        pos.y = _islandYPosVariable;
        pos.z = _islandZPosVariable;
        islandGameObject.transform.position = pos;
    }
}