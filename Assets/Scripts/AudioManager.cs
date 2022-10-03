using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public AudioSource backgroundHum;
	public AudioSource beeping;
	public AudioClip beep;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		float sfxVol = PlayerPrefs.GetFloat("sfx", 0.5f);
		backgroundHum.volume = sfxVol * 0.05f;
		beeping.volume = sfxVol;

		int hum = PlayerPrefs.GetInt("hum", 1);
		if (hum == 1 && !backgroundHum.isPlaying) backgroundHum.Play();
		else if (hum == 0 && backgroundHum.isPlaying) backgroundHum.Stop();
	}

	public void Beep()
	{
		beeping.PlayOneShot(beep);
	}
}
