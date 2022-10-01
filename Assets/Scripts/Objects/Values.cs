using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CLIENT, SERVER values for functions
public class Values
{
	public int? SERVER;
	public int? CLIENT;

	public Values(int? CLIENT, int? SERVER)
	{
		this.CLIENT = CLIENT;
		this.SERVER = SERVER;
	}
}
