using System;
using UnityEngine;

[Serializable]
public class ServiceTask
{
	public string name;
	public string message;
	public string from;
	public float costFrom;
	public float costTo;
	public float currentCost;

	public int postID;
}