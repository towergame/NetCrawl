using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outbound : FirewallBase
{
	public Outbound()
	{
		name = "OUTBOUND";
		health = new Values(null, Random.Range(1, 16));
	}

	public override void Execute(Values input)
	{
		if (input.SERVER == null) return;
		health.SERVER -= input.SERVER;
	}

	public override string GetHealth()
	{
		return System.Convert.ToString(health.SERVER);
	}
}
