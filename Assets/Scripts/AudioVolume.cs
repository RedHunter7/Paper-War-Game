using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVolume : MonoBehaviour
{
	public AudioSource music;
	public AudioSource[] sfx;
    // Start is called before the first frame update
    void Start()
    {
        music.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
		foreach(AudioSource sound in sfx)
		{
			sound.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
