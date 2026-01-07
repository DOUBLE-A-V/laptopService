using System;
using UnityEngine;

[Serializable]
public class ServiceTask
{
	public ServiceTask(
		string name,
		string message,
		string from,
		float costFrom,
		float costTo,
		float currentCost,
		int difficulty,
		int postID
		)
	{
		this.name = name;
		this.message = message;
		this.from = from;
		this.costFrom = costFrom;
		this.costTo = costTo;
		this.currentCost = currentCost;
		this.difficulty = difficulty;
		this.postID = postID;
	}
	
	public string name;
	public string message;
	public string from;
	public float costFrom;
	public float costTo;
	public float currentCost;

	public int difficulty;

	public int postID;
}