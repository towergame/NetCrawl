using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inviolable : FirewallBase
{
	public Inviolable()
	{
		name = "INVIOLABLE";
		health = new Values(666, 666);
	}

	public override void Execute(Values input)
	{
		return;
	}

	public override string GetHealth()
	{
		return System.Convert.ToString(health.CLIENT) + ", " + System.Convert.ToString(health.SERVER);
	}
}
