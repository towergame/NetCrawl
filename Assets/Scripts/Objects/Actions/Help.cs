using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Help : ActionBase
{
	public Help()
	{
		name = "help";
		aliases = new string[] { "help", "?" };
		response = new string[] {
			"---[Help]---",
			"ping - Transmit CLIENT to remote node",
			"curl - Transmit SERVER to remote node",
			"netcat - Move half of CLIENT to SERVER",
			"wget - Move half of SERVER to CLIENT",
			"------------"
		};
	}
	public override Values Execute(Values input, ref FirewallBase firewall)
	{
		return input;
	}
}
