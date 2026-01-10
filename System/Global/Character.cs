using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

public partial class Character : Node
{
	// ========== Save System Configuration ==========
	private string SaveRoot = "user://saves/";
	private string SlotNameFormat = "slot{0}/";
	private string CharacterDataFile = "character.json";
	private int DefaultSlots = 2;

	public override void _Ready()
	{
		base._Ready();
		string path = ProjectSettings.GlobalizePath(SaveRoot);
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
	}

	// ========== Character Stats ==========
	public static string CharacterName { get; set; } = "Player";
	public static int CharacterMaxHp { get; set; } = 20;
	public static int CharacterCurrentHp { get; set; } = 20;
	public static int CharacterLevel { get; set; } = 1;
	public static int CharacterGold { get; set; } = 0;

	private class CharacterData
	{
		public string CharacterName { get; set; } = "Player";
		public int CharacterMaxHp { get; set; } = 20;
		public int CharacterCurrentHp { get; set; } = 20;
		public int CharacterLevel { get; set; } = 1;
		public int CharacterGold { get; set; } = 0;
	}

	public static void ResetCharacter()
	{
		CharacterName = "Player";
		CharacterMaxHp = 20;
		CharacterCurrentHp = 20;
		CharacterLevel = 1;
		CharacterGold = 0;
	}

	// ========== Character Methods ==========
	public static void TakeDamage(int damage)
	{
		CharacterCurrentHp = Math.Max(0, CharacterCurrentHp - damage);
	}

	public static void Heal(int amount)
	{
		CharacterCurrentHp = Math.Min(CharacterMaxHp, CharacterCurrentHp + amount);
	}

	public static void GainGold(int amount)
	{
		CharacterGold += amount;
	}

	public static void LevelUp()
	{
		CharacterLevel++;
		CharacterMaxHp += 5;
	}

	// ========== Save & Load ==========
	public void Save(int slot)
	{
		if (slot < 0)
		{
			GD.PushWarning("Invalid slot number: " + slot);
			return;
		}

		var data = new CharacterData
		{
			CharacterName = CharacterName,
			CharacterMaxHp = CharacterMaxHp,
			CharacterCurrentHp = CharacterCurrentHp,
			CharacterLevel = CharacterLevel,
			CharacterGold = CharacterGold
		};

		string slotDir = SaveRoot + SlotNameFormat.Replace("{0}", slot.ToString());
		string fullPath = ProjectSettings.GlobalizePath(slotDir + CharacterDataFile);

		
		string dirPath = Path.GetDirectoryName(fullPath);
		if (!Directory.Exists(dirPath))
		{
			Directory.CreateDirectory(dirPath);
		}

		try
		{
			string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(fullPath, jsonString);
			GD.Print($"Saved character to slot {slot}");
		}
		catch (Exception e)
		{
			GD.PushError($"Failed to save slot {slot}: {e.Message}");
		}
	}

	public void Load(int slot)
	{
		if (slot < 0)
		{
			GD.PushWarning("Invalid slot number: " + slot);
			return;
		}

		string slotDir = SaveRoot + SlotNameFormat.Replace("{0}", slot.ToString());
		string fullPath = ProjectSettings.GlobalizePath(slotDir + CharacterDataFile);

		if (!File.Exists(fullPath))
		{
			GD.PushWarning($"No save file found for slot {slot}");
			return;
		}

		try
		{
			string jsonString = File.ReadAllText(fullPath);
			var data = JsonSerializer.Deserialize<CharacterData>(jsonString);

			if (data != null)
			{
				CharacterName = data.CharacterName;
				CharacterMaxHp = data.CharacterMaxHp;
				CharacterCurrentHp = data.CharacterCurrentHp;
				CharacterLevel = data.CharacterLevel;
				CharacterGold = data.CharacterGold;
				GD.Print($"Loaded character from slot {slot}");
			}
		}
		catch (Exception e)
		{
			GD.PushError($"Failed to load slot {slot}: {e.Message}");
		}
	}

	public bool SlotExists(int slot)
	{
		string slotDir = SaveRoot + SlotNameFormat.Replace("{0}", slot.ToString());
		string fullPath = ProjectSettings.GlobalizePath(slotDir + CharacterDataFile);
		return File.Exists(fullPath);
	}

	public void DeleteSlot(int slot)
	{
		if (slot < 0) return;
		string slotDir = SaveRoot + SlotNameFormat.Replace("{0}", slot.ToString());
		string fullPath = ProjectSettings.GlobalizePath(slotDir + CharacterDataFile);

		if (File.Exists(fullPath))
		{
			File.Delete(fullPath);
			GD.Print($"Deleted save slot {slot}");
		}
	}
}
