using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wget : ActionBase
{
	public Wget()
	{
		name = "wget";
		aliases = new string[] { "wget" };
	}
	public override Values Execute(Values input, ref FirewallBase firewall)
	{
		return new Values(input.CLIENT + input.SERVER / 2, input.SERVER / 2);
	}
}
