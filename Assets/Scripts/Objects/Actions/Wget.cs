using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wget : ActionBase
{
	public Wget()
	{
		name = "wget";
		aliases = new string[] { "wget" };
		response = new string[] { "SERVER moved to CLIENT." };
	}
	public override Values Execute(Values input, ref FirewallBase firewall)
	{
		int? num = input.CLIENT + input.SERVER / 2;
		return new Values(num > 15 ? 15 : num, input.SERVER / 2);
	}
}
