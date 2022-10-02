using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inbound : FirewallBase
{
	public Inbound()
	{
		name = "INBOUND";
		health = new Values(Random.Range(1, 16), null);
	}

	public override void Execute(Values input)
	{
		if (input.CLIENT == null) return;
		health.CLIENT -= input.CLIENT;
	}

	public override string GetHealth()
	{
		return System.Convert.ToString(health.CLIENT);
	}
}
