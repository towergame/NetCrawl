using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ping : ActionBase
{
	public Ping()
	{
		name = "ping";
		aliases = new string[] { "ping" };
		response = new string[] { "CLIENT transmitted to node." };
	}
	public override Values Execute(Values input, ref FirewallBase firewall)
	{
		firewall.Execute(new Values(input.CLIENT, null));
		return new Values(0, input.SERVER);
	}
}
