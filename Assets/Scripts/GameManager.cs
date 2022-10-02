using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using Baracuda.Monitoring;
using UnityEngine;
using TMPro;

[Serializable] public class FirewallDict : SerializableDictionary<string, GameObject> { }

public class GameManager : MonoBehaviour
{
	public GameObject clientDecGo;
	private TMP_Text clientDecText;
	public GameObject clientBinGo;
	private TMP_Text clientBinText;
	public GameObject serverDecGo;
	private TMP_Text serverDecText;
	public GameObject serverBinGo;
	private TMP_Text serverBinText;
	private int CLIENT = 0;
	private int SERVER = 0;
	public GameObject timeGo;
	private TMP_Text timeText;
	private float timePassed = 0f;

	private string currCommand = "";
	public List<string> initialLines = new();
	private readonly List<string> log = new();

	private readonly Dictionary<string, ActionBase> possibleActions = new();

	private readonly Dictionary<string, Type> possibleLayers = new();
	// private readonly LinkedList<FirewallBase> firewallLayers = new();

	public FirewallDict firewallBlocks;
	public GameObject firewallStackUI;
	private readonly List<GameObject> stackElements = new();

	private TMP_Text terminalObj;
	public GameObject terminalGO;

	private NetworkManager networkManager;
	private Node currentNode = null;

	public void SelectNode(Node node)
	{
		if (currentNode != null) currentNode.selected = false;
		currentNode = node;
		currentNode.selected = true;
		RegenerateLayers();
	}

	public void RegenerateLayers()
	{
		if (currentNode == null) return;

		foreach (GameObject block in stackElements)
		{
			Destroy(block);
		}

		stackElements.Clear();

		int n = 0;
		foreach (FirewallBase firewall in currentNode.firewallLayers)
		{
			if (firewallBlocks.ContainsKey(firewall.name))
			{
				GameObject block = Instantiate(firewallBlocks[firewall.name], new Vector2(528.5f - 200 + n * 57, 225), new Quaternion(), firewallStackUI.transform);
				// RectTransform rt = block.GetComponent<RectTransform>();
				// rt.position = new Vector2(-200 + n * 57, 0);
				stackElements.Add(block);
				n++;
			}
		}
	}

	public FirewallBase GenerateLayer(List<string> options)
	{
		int choice = UnityEngine.Random.Range(0, options.Count);
		Type type = possibleLayers[options[choice]];
		FirewallBase firewall = (FirewallBase)Activator.CreateInstance(type);
		return firewall;
	}

	List<Type> GetPossibleType<T>()
	{
		List<Type> possible = new();
		Type parent = typeof(T);
		Type[] types = Assembly.GetAssembly(parent).GetTypes();
		Type[] inheritingTypes = types.Where(t => t.IsSubclassOf(parent)).ToArray();
		foreach (Type x in inheritingTypes)
		{
			MemberInfo info = x;
			possible.Add(x);
		}
		return possible;
	}

	// Start is called before the first frame update
	public void Start()
	{
		List<Type> actions = GetPossibleType<ActionBase>();
		foreach (Type type in actions)
		{
			ActionBase a = (ActionBase)Activator.CreateInstance(type);
			foreach (string alias in a.aliases)
			{
				possibleActions.Add(alias, a);
			}
		}

		List<Type> firewalls = GetPossibleType<FirewallBase>();
		foreach (Type type in firewalls)
		{
			FirewallBase firewall = (FirewallBase)Activator.CreateInstance(type);
			possibleLayers.Add(firewall.name, type);
		}

		networkManager = gameObject.GetComponent<NetworkManager>();
		networkManager.GenerateNetwork(3, 0.33f);
		// SelectNode(networkManager.nodes.First().GetComponent<Node>());

		foreach (string line in initialLines)
		{
			log.Add(line);
		}

		terminalObj = terminalGO.GetComponent<TMP_Text>();

		clientDecText = clientDecGo.GetComponent<TMP_Text>();
		clientBinText = clientBinGo.GetComponent<TMP_Text>();
		serverDecText = serverDecGo.GetComponent<TMP_Text>();
		serverBinText = serverBinGo.GetComponent<TMP_Text>();
		timeText = timeGo.GetComponent<TMP_Text>();

		CLIENT = UnityEngine.Random.Range(0, 16);
		SERVER = UnityEngine.Random.Range(0, 16);
	}

	// Update is called once per frame
	void Update()
	{
		timePassed += Time.deltaTime;
		if (timePassed > 10)
		{
			timePassed = 0;
			CLIENT = UnityEngine.Random.Range(0, 16);
			SERVER = UnityEngine.Random.Range(0, 16);
			// string nextKey = possibleLayers.Keys.ToArray()[UnityEngine.Random.Range(0, possibleLayers.Count)];
			// currentNode.firewallLayers.AddLast(possibleLayers[nextKey]);
		}

		if (Input.anyKeyDown)
		{
			if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Return))
			{
				log.Add("user@hostname>> " + currCommand);
				if (possibleActions.ContainsKey(currCommand) && !currentNode.cracked)
				{
					FirewallBase currlayer = currentNode.firewallLayers[0];
					Values newVals = possibleActions[currCommand].Execute(new Values(CLIENT, SERVER), ref currlayer);
					log.Add(possibleActions[currCommand].response);
					CLIENT = newVals.CLIENT != null ? (int)newVals.CLIENT : CLIENT;
					SERVER = newVals.SERVER != null ? (int)newVals.SERVER : SERVER;
					if (currlayer.health.CLIENT <= 0 || currlayer.health.SERVER <= 0)
					{
						currentNode.firewallLayers.RemoveAt(0);
						RegenerateLayers();
					}
					if (currentNode.firewallLayers.Count == 0)
					{
						if (currentNode.left != null) currentNode.left?.SetActive(true);
						if (currentNode.right != null) currentNode.right?.SetActive(true);
						currentNode.cracked = true;
						if (currentNode.final) Debug.Log("Win!");
					}
				}
				currCommand = "";
			}
			else if (Input.GetKeyDown(KeyCode.Backspace))
			{
				currCommand = currCommand[0..(currCommand.Length - 1)];
			}
			else
			{
				currCommand += Input.inputString;
			}
		}

		if (log.Count > 19) log.RemoveRange(0, log.Count - 19);
		string terminal = "";
		foreach (string line in log)
		{
			terminal += line + "\n";
		}
		terminal += "user@hostname>> " + currCommand;
		terminalObj.text = terminal;

		clientDecText.text = Convert.ToString(CLIENT);
		string binary = Convert.ToString(CLIENT, 2);
		if (binary.Length < 4) binary = new String('0', 4 - binary.Length) + binary;
		clientBinText.text = binary;

		serverDecText.text = Convert.ToString(SERVER);
		binary = Convert.ToString(SERVER, 2);
		if (binary.Length < 4) binary = new String('0', 4 - binary.Length) + binary;
		serverBinText.text = binary;

		timeText.text = Convert.ToString((int)Mathf.Round(10 - timePassed));

		if (currentNode != null)
		{
			// if (stackElements.Count == 0)
			// {
			// 	RegenerateLayers();
			// }
			// else
			// {
			for (int x = 0; x < currentNode.firewallLayers.Count; x++)
			{
				if (currentNode.firewallLayers[x] == null || stackElements[x] == null) break;
				stackElements[x].GetComponent<FirewallBlock>().health = currentNode.firewallLayers[x].GetHealth();
			}
			// }
		}
	}

	void Awake()
	{
		this.RegisterMonitor();
	}

	void OnDestroy()
	{
		this.UnregisterMonitor();
	}
}
