using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
	public GameObject nodePrefab;
	public GameObject nodeParent;
	public List<GameObject> nodes;

	public GameObject bounds;
	public Vector2 topLeft = new(0, 0);
	public Vector2 bottomRight = new(100, 100);
	public float z = 0;

	public void Start()
	{
		if (bounds != null)
		{
			RectTransform bound = bounds.GetComponent<RectTransform>();
			topLeft = new Vector3(bound.position.x - bound.rect.width / 2, bound.position.y - bound.rect.height / 2, z);
			bottomRight = new Vector3(bound.position.x + bound.rect.width / 2, bound.position.y + bound.rect.height / 2, z);
			// topLeft = new Vector2(bound.rect.xMin, bound.rect.yMax);
			// bottomRight = new Vector2(bound.rect.xMax, bound.rect.yMin);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(new Vector3(topLeft.x, topLeft.y), new Vector3(topLeft.x, bottomRight.y));
		Gizmos.DrawLine(new Vector3(topLeft.x, topLeft.y), new Vector3(bottomRight.x, topLeft.y));
		Gizmos.DrawLine(new Vector3(bottomRight.x, bottomRight.y), new Vector3(topLeft.x, bottomRight.y));
		Gizmos.DrawLine(new Vector3(bottomRight.x, bottomRight.y), new Vector3(bottomRight.x, topLeft.y));
	}

	public void GenerateNetwork(int steps, float deviationCoeff)
	{
		Node nodeInstance = null;
		for (int x = 0; x < steps; x++)
		{
			GameObject node = Instantiate(nodePrefab, new Vector3(Random.Range(topLeft.x, bottomRight.x), Random.Range(topLeft.y, bottomRight.y), z), new Quaternion(), nodeParent.transform);
			nodes.Add(node);
			if (x > 0) node.SetActive(false);
			Node currentNode = node.GetComponent<Node>();
			if (x == steps - 1) currentNode.final = true;
			if (nodeInstance != null)
			{
				currentNode.GenerateLine(nodeInstance);
				int coinFlip = Random.Range(0, 2);
				if (coinFlip == 0)
				{
					nodeInstance.left = node;
					float randomCoeff = Random.value;
					GameObject randomNode = Instantiate(nodePrefab, new Vector3(Random.Range(topLeft.x, bottomRight.x), Random.Range(topLeft.y, bottomRight.y), z), new Quaternion(), nodeParent.transform);
					nodeInstance.right = randomNode;
					nodes.Add(randomNode);
					randomNode.SetActive(false);
					Node provisionalNodeInstance = randomNode.GetComponent<Node>();
					provisionalNodeInstance.GenerateLine(nodeInstance);
					while (randomCoeff < deviationCoeff)
					{
						int anotherCoinFlip = Random.Range(0, 2);
						GameObject anotherRandomNode = Instantiate(nodePrefab, new Vector3(Random.Range(topLeft.x, bottomRight.x), Random.Range(topLeft.y, bottomRight.y), z), new Quaternion(), nodeParent.transform);
						nodes.Add(anotherRandomNode);
						anotherRandomNode.SetActive(false);
						if (anotherCoinFlip == 0) provisionalNodeInstance.left = anotherRandomNode;
						else provisionalNodeInstance.right = anotherRandomNode;
						anotherRandomNode.GetComponent<Node>().GenerateLine(provisionalNodeInstance);
						provisionalNodeInstance = anotherRandomNode.GetComponent<Node>();
						randomCoeff = Random.value;
					}
				}
				else
				{
					nodeInstance.right = node;
					float randomCoeff = Random.value;
					GameObject randomNode = Instantiate(nodePrefab, new Vector3(Random.Range(topLeft.x, bottomRight.x), Random.Range(topLeft.y, bottomRight.y), z), new Quaternion(), nodeParent.transform);
					nodeInstance.left = randomNode;
					nodes.Add(randomNode);
					randomNode.SetActive(false);
					Node provisionalNodeInstance = randomNode.GetComponent<Node>();
					provisionalNodeInstance.GenerateLine(nodeInstance);
					while (randomCoeff < deviationCoeff)
					{
						int anotherCoinFlip = Random.Range(0, 2);
						GameObject anotherRandomNode = Instantiate(nodePrefab, new Vector3(Random.Range(topLeft.x, bottomRight.x), Random.Range(topLeft.y, bottomRight.y), z), new Quaternion(), nodeParent.transform);
						nodes.Add(anotherRandomNode);
						anotherRandomNode.SetActive(false);
						if (anotherCoinFlip == 0) provisionalNodeInstance.left = anotherRandomNode;
						else provisionalNodeInstance.right = anotherRandomNode;
						anotherRandomNode.GetComponent<Node>().GenerateLine(provisionalNodeInstance);
						provisionalNodeInstance = anotherRandomNode.GetComponent<Node>();
						randomCoeff = Random.value;
					}
				}
			}
			nodeInstance = currentNode;
		}
	}
}
