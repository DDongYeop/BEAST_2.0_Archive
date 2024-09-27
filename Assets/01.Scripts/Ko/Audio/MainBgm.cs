using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainBgm : MonoBehaviour
{
    private AudioSource _AudioSource;

    private void Awake()
    {
        _AudioSource = GetComponent<AudioSource>();
    }

    public void PlayBgm(string _sceneName)
    {
        if((_sceneName == "Start" || _sceneName == "Menu 2") && !_AudioSource.isPlaying)
        {
            _AudioSource.time = 0;
            _AudioSource.Play();
        }
        else if(_sceneName != "Start" && _sceneName != "Menu 2")
        {
            _AudioSource.Stop();
        }

    }
}
