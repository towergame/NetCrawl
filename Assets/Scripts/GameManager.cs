using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using Baracuda.Monitoring;
using UnityEngine;
using Unity.VisualScripting;
using TMPro;

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
	[Monitor]
	private int? FW_CLIENT = 0;

	[Monitor]
	private string currCommand = "";
	public List<string> initialLines = new();
	private readonly List<string> log = new();

	private readonly Dictionary<string, ActionBase> possibleActions = new();

	private readonly Dictionary<string, FirewallBase> possibleLayers = new();
	// private readonly LinkedList<FirewallBase> firewallLayers = new();

	private TMP_Text terminalObj;
	public GameObject terminalGO;

	private NetworkManager networkManager;
	private Node currentNode = null;

	public void SelectNode(Node n)
	{
		if (currentNode != null) currentNode.selected = false;
		currentNode = n;
		currentNode.selected = true;
	}

	public FirewallBase GenerateLayer(List<string> options)
	{
		int choice = UnityEngine.Random.Range(0, options.Count);
		return possibleLayers[options[choice]];
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
			possibleLayers.Add(firewall.name, firewall);
		}

		networkManager = gameObject.GetComponent<NetworkManager>();
		networkManager.GenerateNetwork(3, 0.33f);
		currentNode = networkManager.nodes.First().GetComponent<Node>();
		currentNode.selected = true;

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
		FW_CLIENT = currentNode.firewallLayers.FirstOrDefault() != null ? currentNode.firewallLayers.FirstOrDefault().health.CLIENT : 0;
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
					if (currlayer.health.CLIENT <= 0 || currlayer.health.SERVER <= 0) currentNode.firewallLayers.RemoveAt(0);
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
