using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
	public readonly List<FirewallBase> firewallLayers = new();
	public List<string> possibleLayers;
	public int layerCount = 3;
	public bool cracked = false;
	public bool final = false;
	public bool selected = false;
	public int restrictedLayers = 0;

	public GameManager gameManager;

	public GameObject? left;
	public GameObject? right;

	public Color crackedColour;
	public Color uncrackedColour;
	public Color selectedColour;

	public float gradientTime = 1.5f;
	public float selectedCoeff = 0.5f;
	private float gradientScale = 0f;

	public Material lineMat;

	private Color currentColour;

	public void GenerateLine(Node parent)
	{
		LineRenderer lr = gameObject.AddComponent<LineRenderer>();

		lr.SetPosition(0, gameObject.transform.position + new Vector3(0, 0, 5));
		lr.SetPosition(1, parent.transform.position + new Vector3(0, 0, 5));
		lr.startWidth = 0.05f;
		lr.endWidth = 0.05f;
		lr.startColor = Color.white;
		lr.endColor = Color.white;
		lr.material = lineMat;
	}

	public void Start()
	{
		GameObject manager = GameObject.FindGameObjectsWithTag("Manager")[0];
		gameManager = manager.GetComponent<GameManager>();

		for (int x = 0; x < layerCount; x++)
		{
			firewallLayers.Add(gameManager.GenerateLayer(possibleLayers));
		}

		currentColour = uncrackedColour;
	}

	private bool isOver = false;

	public void Update()
	{
		if (Input.GetMouseButtonDown(0) && isOver)
		{
			gameManager.SelectNode(this);
		}
		if (isOver && !selected)
		{
			gradientScale = Mathf.Min(selectedCoeff, gradientScale + Time.deltaTime / gradientTime);
		}
		else
		{
			gradientScale = Mathf.Max(0f, gradientScale - Time.deltaTime / gradientTime);
		}

		currentColour = (cracked ? crackedColour : uncrackedColour) * (1 - gradientScale) + selectedColour * gradientScale;
		currentColour = selected ? selectedColour : currentColour;
		gameObject.GetComponent<SpriteRenderer>().color = currentColour;

		if (final) transform.eulerAngles = new Vector3(0, 0, 45);
	}

	public void OnMouseEnter()
	{
		isOver = true;
	}

	public void OnMouseExit()
	{
		isOver = false;
	}
}
