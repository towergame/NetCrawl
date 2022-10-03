using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contiguous : FirewallBase
{
	string choice = null;

	public Contiguous()
	{
		name = "CONTIGUOUS";
		health = new Values(UnityEngine.Random.Range(1, 16) + UnityEngine.Random.Range(1, 16), null);
	}

	public override void Execute(Values input)
	{
		if (choice == null && input.CLIENT != null) choice = "CLIENT";
		else if (choice == null && input.SERVER != null) choice = "SERVER";
		else if (choice == null) return;

		if (input.SERVER != null && choice == "SERVER") health.CLIENT -= input.SERVER;
		if (input.CLIENT != null && choice == "CLIENT") health.CLIENT -= input.CLIENT;
	}

	public override string GetHealth()
	{
		if (choice == "CLIENT") return "<color=#00C4FF>" + Convert.ToString(health.CLIENT) + "</color>";
		if (choice == "SERVER") return "<color=#FFA700>" + Convert.ToString(health.CLIENT) + "</color>";
		return Convert.ToString(health.CLIENT);
	}
}
