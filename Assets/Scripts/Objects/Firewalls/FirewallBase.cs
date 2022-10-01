using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FirewallBase
{
	public string name;
	public Values health;
	public abstract void Execute(Values input);
}
