using System;
using System.Collections.Generic;

struct ColorsTags
{
	public static string red = "<color=#ff0000>";
	public static string green =  "<color=#00ff00>";
	public static string blue =  "<color=#8888ff>";
	public static string yellow =  "<color=#ffff00>";
	public static string orange =  "<color=#ffa500>";
}

[Serializable]
public class TipDescriptor
{
	public enum ToolName
	{
		Hand,
		
	}

	public enum TargetName
	{
		Sticker,
		
	}
	
	public string title;
	public string rawText;
	public List<ToolName> weaknesses = new();
	public List<TargetName> strengths = new();
	public List<ToolName> ignoresTools = new();

	public int damage = 0;

	public int maxUses = 0;
	public int usesLeft = 0;

	public int maxHealth = 0;
	public int health = 0;
	
	public string CookText()
	{
		string result = "<size=3><blue>" + title + "<end></size>\n\n";
		if (weaknesses.Count > 0)
		{
			result += "<orange>weaknesses:<end>\n";
			foreach (ToolName toolName in weaknesses)
			{
				result += "<i>-" + toolName + "</i>\n";
			}

			result += "\n";
		}

		if (strengths.Count > 0)
		{
			result += "<red>strengths:<end>\n";
			foreach (TargetName targetName in strengths)
			{
				result += "<i>-" + targetName + "</i>\n";
			}
			result += "\n";
		}
		
		if (ignoresTools.Count > 0)
		{
			result += "<red>ignores:<end>\n";
			foreach (ToolName toolName in ignoresTools)
			{
				result += "<i>-" + toolName + "</i>\n";
			}

			result += "\n";
		}

		if (damage > 0)
		{
			result += "<red>damage: " + damage + "\n";
		}

		if (maxUses > 0 && usesLeft >= 0)
		{
			result += "<orange>durability: " + usesLeft + " / " + maxUses + "<end>\n";
		}
		
		if (maxHealth > 0 && health >= 0)
		{
			result += "<orange>health: " + health + " / " + maxHealth + "<end>\n";
		}


		result += rawText;
		result = result
			.Replace("<red>", ColorsTags.red)
			.Replace("<green>", ColorsTags.green)
			.Replace("<blue>", ColorsTags.blue)
			.Replace("<yellow>", ColorsTags.yellow)
			.Replace("<orange>", ColorsTags.orange)
			.Replace("<end>", "</color>");
		return result;
	}
}