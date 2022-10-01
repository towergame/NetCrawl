using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using Baracuda.Monitoring;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[Monitor]
	private int CLIENT = 0;
	[Monitor]
	private int SERVER = 0;
	[Monitor]
	private float timePassed = 0f;
	[Monitor]
	private int? FW_CLIENT = 0;

	[Monitor]
	private string currCommand = "";

	private readonly Dictionary<string, ActionBase> possibleActions = new();
	private readonly Stack<ActionBase> processingStack = new();

	private readonly Dictionary<string, FirewallBase> possibleLayers = new();
	private readonly LinkedList<FirewallBase> firewallLayers = new();

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

		firewallLayers.AddLast(possibleLayers["SHARED"]);
	}

	// Update is called once per frame
	void Update()
	{
		FW_CLIENT = firewallLayers.First != null ? firewallLayers.First.Value.health.CLIENT : 0;
		timePassed += Time.deltaTime;
		if (timePassed > 10)
		{
			timePassed = 0;
			CLIENT = UnityEngine.Random.Range(0, 16);
			SERVER = UnityEngine.Random.Range(0, 16);
			string nextKey = possibleLayers.Keys.ToArray()[UnityEngine.Random.Range(0, possibleLayers.Count)];
			firewallLayers.AddLast(possibleLayers[nextKey]);
		}

		if (Input.anyKeyDown)
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				if (possibleActions.ContainsKey(currCommand))
				{
					Debug.Log("Command recognised");
					FirewallBase currlayer = firewallLayers.First.Value;
					firewallLayers.RemoveFirst();
					Values newVals = possibleActions[currCommand].Execute(new Values(CLIENT, SERVER), ref currlayer);
					CLIENT = newVals.CLIENT != null ? (int)newVals.CLIENT : CLIENT;
					SERVER = newVals.SERVER != null ? (int)newVals.SERVER : SERVER;
					if (currlayer.health.CLIENT > 0 || currlayer.health.SERVER > 0) firewallLayers.AddFirst(currlayer);
				}
				currCommand = "";
			}
			else
			{
				currCommand += Input.inputString;
			}
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
