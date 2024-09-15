using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Px/Audio Clip")]
public class PxAudioClip : ScriptableObject
{
    public PxAudioClipData clipData;
}
[Serializable]
public class PxAudioClipData
{
    public AudioClip clip;
    public float volume = 1.0f;
}


public static class PxAudioClipDataExtensions
{
    public static PxAudioClipData ToPxData(this AudioClip clip)
    {
        return new PxAudioClipData
        {
            clip = clip
        };
    }
}