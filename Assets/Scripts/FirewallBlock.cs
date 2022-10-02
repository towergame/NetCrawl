using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class FirewallBlock : MonoBehaviour
{
	public string health;
	public GameObject healthTextObj;
	private TMP_Text healthText;

	void Start()
	{
		healthText = healthTextObj.GetComponent<TMP_Text>();
	}

	// Update is called once per frame
	void Update()
	{
		healthText.text = health;
	}
}
