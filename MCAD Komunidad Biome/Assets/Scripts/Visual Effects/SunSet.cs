using UnityEngine;

public class SunSet : MonoBehaviour
{
    [Header("Sunset Objects")]
    [SerializeField] private Material skybox;
    [SerializeField] private GameObject oceanBackRows;
    [SerializeField] private GameObject sunLight;

    [Header("Sunset Variables")]
    [SerializeField] private float _sunLightXRotation; // 0.10166f // 163 - 187.5 => in 241 seconds

    private float _sunLightCurrentXRotation;
    private bool _sunsetBool;
    private float _counter;

    void Start()
    {
        _sunsetBool = false;

        oceanBackRows.SetActive(true);

        _sunLightCurrentXRotation = 163f;

        SetSunset();

        sunLight.SetActive(false);

        _counter = 0f;
    }

    void Update()
    {
        if (_sunsetBool == true)
        {
            _sunLightCurrentXRotation += _sunLightXRotation * Time.deltaTime;
            _sunLightCurrentXRotation = Mathf.Clamp(_sunLightCurrentXRotation, 0f, 187.5f);
            SetSunset();

            _counter += Time.deltaTime;

            if (_counter >= 241f)
            {
                _sunsetBool = false;
            }
        }
    }

    private void SetSunset()
    {
        sunLight.transform.rotation = Quaternion.Euler(_sunLightCurrentXRotation, sunLight.transform.rotation.y, sunLight.transform.rotation.z);
    }

    public void PrepareSunset()
    {
        sunLight.SetActive(false);
    }

    public void StartSunset()
    {
        oceanBackRows.SetActive(false);
        sunLight.SetActive(true);

        _sunsetBool = true;
    }
}