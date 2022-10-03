using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curl : ActionBase
{
	public Curl()
	{
		name = "curl";
		aliases = new string[] { "curl" };
		response = new string[] { "SERVER transmitted to node." };
	}
	public override Values Execute(Values input, ref FirewallBase firewall)
	{
		firewall.Execute(new Values(null, input.SERVER));
		return new Values(input.CLIENT, 0);
	}
}
