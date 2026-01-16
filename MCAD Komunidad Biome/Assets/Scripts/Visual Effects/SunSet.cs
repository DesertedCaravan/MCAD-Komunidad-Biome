using UnityEngine;

public class SunSet : MonoBehaviour
{
    [Header("Sunset Objects")]
    [SerializeField] private Material skybox;
    [SerializeField] private GameObject sun;
    [SerializeField] private GameObject oceanBackRows;

    [Header("Sunset Variables")]
    [SerializeField] private float _sunsetSkyboxFloat; // 0.00667f // 1 => 2 in 150 seconds
    [SerializeField] private float _sunsetSphereHeight; // 0.14066f // 20 => -1.1 in 150 seconds
    private float _sunsetSkyboxAtmosThickness;
    private float _sunsetYPosVariable;
    private bool _sunsetBool;
    private float _counter;

    void Start()
    {
        _sunsetBool = false;

        oceanBackRows.SetActive(true);

        _sunsetSkyboxAtmosThickness = 1.0f;
        _sunsetYPosVariable = 20.0f;

        SetSunset();

        sun.SetActive(false);

        _counter = 0f;
    }

    void Update()
    {
        if (_sunsetBool == true)
        {
            _sunsetSkyboxAtmosThickness += _sunsetSkyboxFloat * Time.deltaTime;
            _sunsetSkyboxAtmosThickness = Mathf.Clamp(_sunsetSkyboxAtmosThickness, 0f, 2.0f);
            _sunsetYPosVariable -= _sunsetSphereHeight * Time.deltaTime;

            SetSunset();

            _counter += Time.deltaTime;

            if (_counter >= 150)
            {
                _sunsetBool = false;
            }
        }
    }

    private void SetSunset()
    {
        // Reference: https://discussions.unity.com/t/how-can-i-change-the-atmosphere-thickness-skybox-material-with-another-script/878013/2
        skybox.SetFloat("_AtmosphereThickness", _sunsetSkyboxAtmosThickness);
        sun.transform.position = new Vector3(sun.transform.position.x, _sunsetYPosVariable, sun.transform.position.z);
    }

    public void PrepareSunset()
    {
        sun.SetActive(false);
    }

    public void StartSunset()
    {
        oceanBackRows.SetActive(false);
        sun.SetActive(true);

        _sunsetBool = true;
    }
}