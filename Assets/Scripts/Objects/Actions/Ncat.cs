using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ncat : ActionBase
{
	public Ncat()
	{
		name = "ncat";
		aliases = new string[] { "ncat", "netcat" };
	}
	public override Values Execute(Values input, ref FirewallBase firewall)
	{
		return new Values(input.CLIENT / 2, input.SERVER + input.CLIENT / 2);
	}
}
