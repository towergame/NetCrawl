using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shared : FirewallBase
{
	public Shared()
	{
		name = "SHARED";
		health = new Values(UnityEngine.Random.Range(1, 16) + UnityEngine.Random.Range(1, 8), null);
	}

	public override void Execute(Values input)
	{
		if (input.SERVER != null) health.CLIENT -= input.SERVER;
		if (input.CLIENT != null) health.CLIENT -= input.CLIENT;
	}
}
