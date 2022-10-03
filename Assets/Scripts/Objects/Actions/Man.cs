using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Man : ActionBase
{
	public Man()
	{
		name = "man";
		aliases = new string[] { "man", "manual" };
		response = new string[] {
			"---[Manual]---",
			"There are 4 firewall types:",
			"- INBOUND, only accepts CLIENT data",
			"- OUTBOUND, only accepts SERVER data",
			"- SHARED, accepts both CLIENT and SERVER data",
			"- CONTIGUOUS, accepts both data for first transmission,",
			"then only data of the type the transmission was",
			"------------"
		};
	}
	public override Values Execute(Values input, ref FirewallBase firewall)
	{
		return input;
	}
}
