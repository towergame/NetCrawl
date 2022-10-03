using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
	private float musicScale;
	private float sfxScale;

	public FirewallDict menuDict;

	public GameObject sfxSlideGO;
	private Slider sfxSlide;
	public GameObject musicSlideGO;
	private Slider musicSlide;

	public Toggle humToggle;

	// Start is called before the first frame update
	void Start()
	{
		musicScale = PlayerPrefs.GetFloat("music", 0.5f);
		sfxScale = PlayerPrefs.GetFloat("sfx", 0.5f);

		sfxSlide = sfxSlideGO.GetComponent<Slider>();
		sfxSlide.value = sfxScale;
		musicSlide = musicSlideGO.GetComponent<Slider>();
		musicSlide.value = musicScale;

		humToggle.isOn = PlayerPrefs.GetInt("hum", 1) == 1;
	}

	public void MusicChange()
	{
		musicScale = musicSlide.value;
		PlayerPrefs.SetFloat("music", musicScale);
	}

	public void SFXChange()
	{
		sfxScale = sfxSlide.value;
		PlayerPrefs.SetFloat("sfx", sfxScale);
	}

	public void SwitchMenu(string newMenu)
	{
		foreach (string key in menuDict.Keys)
		{
			menuDict[key].SetActive(false);
		}

		menuDict[newMenu].SetActive(true);
	}

	public void LoadGame(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void ToggleHum()
	{
		PlayerPrefs.SetInt("hum", humToggle.isOn ? 1 : 0);
	}

	public void LeaveGame()
	{
		Application.Quit();
	}
}
