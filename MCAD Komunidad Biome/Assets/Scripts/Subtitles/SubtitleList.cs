using UnityEngine;

[CreateAssetMenu(fileName = "New Subtitle List", menuName = "Subtitles/Subtitle List")]
public class SubtitleList: ScriptableObject
{
    [SerializeField] private SubtitleText[] subtitles;

    public SubtitleText[] Subtitles => subtitles; // getter function
    public bool HasResponses => Subtitles != null && Subtitles.Length > 0;
}