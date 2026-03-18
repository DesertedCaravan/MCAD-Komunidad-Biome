using UnityEngine;

[System.Serializable]
public class SubtitleText
{
    [SerializeField] [TextArea] private string subtitle;
    [SerializeField] private float time;

    public string Subtitle => subtitle;
    public float Time => time;
}