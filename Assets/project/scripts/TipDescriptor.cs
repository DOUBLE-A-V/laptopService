using System;
using System.Collections.Generic;

struct ColorsTags
{
	public static string red = "<color=#ff0000>";
	public static string green =  "<color=#00ff00>";
	public static string blue =  "<color=#8888ff>";
	public static string yellow =  "<color=#ffff00>";
	public static string orange =  "<color=#ffa500>";
	public static string white =  "<color=#ffffff>";
}

[Serializable]
public class TipDescriptor
{
	public enum ToolName
	{
		Hand,
		Rag,
		
	}

	public enum TargetName
	{
		Scratches,
		Water,
		Dust,
		Stickers,
		StickySPCResidue,
		Scribbles,
		JuiceSPCStain,
		HugeSPCScratches,
		
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

	public int qualityExists = 0;
	public int qualityRemoved = 0;

	public int energyCost = 0;
	
	public string CookText()
	{
		string result = "<size=3><blue>" + title + "<end></size>\n\n";
		if (maxUses != 0) result += "<color=#ff9922>energy cost: <wave>" + (energyCost == 0 ? "free" : energyCost) + "<end></wave>\n";
		if (weaknesses.Count > 0)
		{
			result += "<orange>weaknesses:<end><wave>\n";
			foreach (ToolName toolName in weaknesses)
			{
				result += "<i>-" + toolName + "</i>\n";
			}

			result += "\n</wave>";
		}

		if (strengths.Count > 0)
		{
			result += "<red>strengths:<end><grow>\n";
			foreach (TargetName targetName in strengths)
			{
				result += ("<i>-" + targetName).Replace("SPC", " ") + "</i>\n";
			}
			result += "\n</grow>";
		}
		
		if (ignoresTools.Count > 0)
		{
			result += "<red>ignores:<end><grow>\n";
			foreach (ToolName toolName in ignoresTools)
			{
				result += "<i>-" + toolName + "</i>\n";
			}

			result += "\n</grow>";
		}

		if (damage > 0)
		{
			result += "<red>damage: <grow>" + damage + "\n</grow>";
		}

		if (maxUses > 0 && usesLeft >= 0)
		{
			result += "<orange>durability: <wave>" + usesLeft + " / " + maxUses + "<end></wave>\n";
		}
		
		if (maxHealth > 0 && health >= 0)
		{
			result += "<orange>health: <wave>" + health + " / " + maxHealth + "<end></wave>\n";
		}


		result += rawText;
		result += "\n";
		if (qualityExists != 0)
		{
			if (qualityExists > 0)
			{
				result += "<orange>if on laptop:<end> <green><jump>+" + qualityExists + "%</jump> quality<end>\n";
			}
			else
			{
				result += "<orange>if on laptop:<end> <red><grow>" + qualityExists + "% quality</grow><end>\n";
			}
		}
		
		if (qualityRemoved != 0)
		{
			if (qualityRemoved > 0)
			{
				result += "<orange>if removed:<end> <green><jump>+" + qualityRemoved + "%</jump> quality<end>\n";
			}
			else
			{
				result += "<orange>if removed:<end> <red><grow>" + qualityRemoved + "% quality</grow><end>\n";
			}
		}
		result = result
			.Replace("<white>", ColorsTags.white)
			.Replace("<red>", ColorsTags.red)
			.Replace("<green>", ColorsTags.green)
			.Replace("<blue>", ColorsTags.blue)
			.Replace("<yellow>", ColorsTags.yellow)
			.Replace("<orange>", ColorsTags.orange)
			.Replace("<end>", "</color>");
		return result;
	}
}