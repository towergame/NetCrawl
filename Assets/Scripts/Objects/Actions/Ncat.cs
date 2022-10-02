using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ncat : ActionBase
{
	public Ncat()
	{
		name = "ncat";
		aliases = new string[] { "ncat", "netcat" };
		response = "CLIENT moved to SERVER.";
	}
	public override Values Execute(Values input, ref FirewallBase firewall)
	{
		int? num = input.SERVER + input.CLIENT / 2;
		return new Values(input.CLIENT / 2, num > 15 ? 15 : num);
	}
}
