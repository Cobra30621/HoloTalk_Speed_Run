using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private AudioSource audioSource;
    void Start () {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
    }
    public void VolumeChanged(float newVolume) {
        //使音量等同於newVolume
        audioSource.volume = newVolume;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
