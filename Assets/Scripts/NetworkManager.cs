using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
	public GameObject nodePrefab;
	public List<GameObject> nodes;

	public Vector2 topLeft = new Vector2(0, 0);
	public Vector2 bottomRight = new Vector2(100, 100);
	public float z = 0;

	public void GenerateNetwork(int steps, float deviationCoeff)
	{
		Node nodeInstance = null;
		for (int x = 0; x < steps; x++)
		{
			GameObject node = Instantiate(nodePrefab, new Vector3(Random.Range(topLeft.x, bottomRight.x), Random.Range(topLeft.y, bottomRight.y), z), new Quaternion());
			nodes.Add(node);
			if (nodeInstance != null)
			{
				int coinFlip = Random.Range(0, 2);
				if (coinFlip == 0)
				{
					nodeInstance.left = node;
					float randomCoeff = Random.value;
					GameObject randomNode = Instantiate(nodePrefab, new Vector3(Random.Range(topLeft.x, bottomRight.x), Random.Range(topLeft.y, bottomRight.y), z), new Quaternion());
					nodeInstance.right = randomNode;
					nodes.Add(randomNode);
					Node provisionalNodeInstance = randomNode.GetComponent<Node>();
					while (randomCoeff < deviationCoeff)
					{
						int anotherCoinFlip = Random.Range(0, 2);
						GameObject anotherRandomNode = Instantiate(nodePrefab, new Vector3(Random.Range(topLeft.x, bottomRight.x), Random.Range(topLeft.y, bottomRight.y), z), new Quaternion());
						nodes.Add(anotherRandomNode);
						if (anotherCoinFlip == 0) provisionalNodeInstance.left = anotherRandomNode;
						else provisionalNodeInstance.right = anotherRandomNode;
						provisionalNodeInstance = anotherRandomNode.GetComponent<Node>();
					}
				}
				else
				{
					nodeInstance.right = node;
					float randomCoeff = Random.value;
					GameObject randomNode = Instantiate(nodePrefab, new Vector3(Random.Range(topLeft.x, bottomRight.x), Random.Range(topLeft.y, bottomRight.y), z), new Quaternion());
					nodeInstance.left = randomNode;
					nodes.Add(randomNode);
					Node provisionalNodeInstance = randomNode.GetComponent<Node>();
					while (randomCoeff < deviationCoeff)
					{
						int anotherCoinFlip = Random.Range(0, 2);
						GameObject anotherRandomNode = Instantiate(nodePrefab, new Vector3(Random.Range(topLeft.x, bottomRight.x), Random.Range(topLeft.y, bottomRight.y), z), new Quaternion());
						nodes.Add(anotherRandomNode);
						if (anotherCoinFlip == 0) provisionalNodeInstance.left = anotherRandomNode;
						else provisionalNodeInstance.right = anotherRandomNode;
						provisionalNodeInstance = anotherRandomNode.GetComponent<Node>();
					}
				}
			}
		}
	}
}
