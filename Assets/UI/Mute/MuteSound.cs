using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteSound : MonoBehaviour
{
    private AudioSource[] audioSources;

    void Awake()
    {
        audioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

        if (PlayerPrefs.GetInt("mute") == 0)
        {
            Mute(true); // mute
        }
        else
        {
            Mute(false); // demute
        }
    }

    public void Mute(bool val)
    {
        foreach(AudioSource audioSource in audioSources)
        {
            audioSource.volume = val ? 0 : 1;
        }
    }
}
