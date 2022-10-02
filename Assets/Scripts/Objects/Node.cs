using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
	public readonly LinkedList<FirewallBase> firewallLayers = new();
	public List<string> possibleLayers;

	public GameObject? left;
	public GameObject? right;
}
