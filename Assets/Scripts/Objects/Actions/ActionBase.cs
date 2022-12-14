using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionBase
{
	public string name;
	public string[] aliases; // For terminal commands
	public string[] response; // For terminal callback
	public abstract Values Execute(Values input, ref FirewallBase firewall);
}
