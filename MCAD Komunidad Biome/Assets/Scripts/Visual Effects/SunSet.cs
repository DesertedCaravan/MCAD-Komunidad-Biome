using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class SunSet : MonoBehaviour
{
    [Header("Sunset Objects")]
    [SerializeField] private Material skybox;
    // [SerializeField] private GameObject sun;
    [SerializeField] private GameObject oceanBackRows;
    [SerializeField] private GameObject sunLight;

    [Header("Sunset Variables")]
    // [SerializeField] private float _sunsetSkyboxFloat; // 0.00415f // 1 => 2 in 241 seconds
    // [SerializeField] private float _sunsetSphereHeight; // 0.08755f // 20 => -1.1 in 241 seconds
    [SerializeField] private float _sunLightXRotation; // 0.10166f // 163 - 187.5 => in 241 seconds

    // private float _sunsetSkyboxAtmosThickness;
    // private float _sunsetYPosVariable;
    private float _sunLightCurrentXRotation;
    private bool _sunsetBool;
    private float _counter;

    void Start()
    {
        _sunsetBool = false;

        oceanBackRows.SetActive(true);

        _sunLightCurrentXRotation = 163f;
        // _sunsetSkyboxAtmosThickness = 1.0f;
        // _sunsetYPosVariable = 20.0f;

        SetSunset();

        // sun.SetActive(false);
        sunLight.SetActive(false);

        _counter = 0f;
    }

    void Update()
    {
        if (_sunsetBool == true)
        {
            // _sunsetSkyboxAtmosThickness += _sunsetSkyboxFloat * Time.deltaTime;
            // _sunsetSkyboxAtmosThickness = Mathf.Clamp(_sunsetSkyboxAtmosThickness, 0f, 2.0f);
            // _sunsetYPosVariable -= _sunsetSphereHeight * Time.deltaTime;

            _sunLightCurrentXRotation += _sunLightXRotation * Time.deltaTime;
            _sunLightCurrentXRotation = Mathf.Clamp(_sunLightCurrentXRotation, 0f, 187.5f);
            SetSunset();

            _counter += Time.deltaTime;

            if (_counter >= 241)
            {
                _sunsetBool = false;
            }
        }
    }

    private void SetSunset()
    {
        // Reference: https://discussions.unity.com/t/how-can-i-change-the-atmosphere-thickness-skybox-material-with-another-script/878013/2
        // skybox.SetFloat("_AtmosphereThickness", _sunsetSkyboxAtmosThickness);
        // sun.transform.position = new Vector3(sun.transform.position.x, _sunsetYPosVariable, sun.transform.position.z);

        sunLight.transform.rotation = Quaternion.Euler(_sunLightCurrentXRotation, sunLight.transform.rotation.y, sunLight.transform.rotation.z);
    }

    public void PrepareSunset()
    {
        // sun.SetActive(false);
        sunLight.SetActive(false);
    }

    public void StartSunset()
    {
        oceanBackRows.SetActive(false);
        // sun.SetActive(true);
        sunLight.SetActive(true);

        _sunsetBool = true;
    }
}