using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
	public readonly LinkedList<FirewallBase> firewallLayers = new();
	public List<string> possibleLayers;

	public GameObject? left;
	public GameObject? right;

	public void GenerateLine(Node parent)
	{
		LineRenderer lr = gameObject.AddComponent<LineRenderer>();

		lr.SetPosition(0, gameObject.transform.position);
		lr.SetPosition(1, parent.transform.position);
		lr.startWidth = 0.1f;
		lr.endWidth = 0.1f;
	}
}
