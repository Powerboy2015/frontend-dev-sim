using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;
using System.Linq;


public partial class UpgradeManager : Node
{

	public static UpgradeManager instance;

	[Export]
	public Godot.Collections.Array<String> AvailableUpgrades { get; set; }
	private HashSet<string> enabledUpgrades = new HashSet<string>();

	//Creates callable events that we can use in order to update other scripts when an upgrade is enabled/disabled
	[Signal]
	public delegate void UpgradeEnabledEventHandler(string upgradeName);

	[Signal]
	public delegate void UpgradeDisabledEventHandler(string upgradeName);
	public override void _Ready()
	{
		if (instance == null)
		{
			instance = this;

			// Initialize available upgrades if not set in editor
			if (AvailableUpgrades == null || AvailableUpgrades.Count == 0)
			{
				// TODO List of upgrades, can be used to add more items.
				AvailableUpgrades = new Godot.Collections.Array<String>
				{
					"Flower_1",
					"Flower_2",
					"Flower_3",
					"Flower_4",
					"Flower_5",
					"Computer_6",
					"Hat",
					"Closet",
                    "Lamp"
				};

				GD.Print("Initialized AvailableUpgrades with default values.");
			}
		}
		else
		{
			QueueFree();
		}
	}

	public bool EnableUpgrade(string upgradeName)
	{
		if (!AvailableUpgrades.Contains(upgradeName))
		{
			GD.PrintErr($"Upgrade '{upgradeName}' is not available!");
			return false;
		}

		if (enabledUpgrades.Add(upgradeName))
		{
			EmitSignal(SignalName.UpgradeEnabled, upgradeName);
			GD.Print($"Enabled upgrade: {upgradeName}");
			return true;
		}
		return false; // Already enabled
	}

	// Disable an upgrade
	public bool DisableUpgrade(string upgradeName)
	{
		if (enabledUpgrades.Remove(upgradeName))
		{
			EmitSignal(SignalName.UpgradeDisabled, upgradeName);
			GD.Print($"Disabled upgrade: {upgradeName}");
			return true;
		}
		return false; // Wasn't enabled
	}

	// Check if upgrade is enabled
	public bool IsUpgradeEnabled(string upgradeName)
	{
		return enabledUpgrades.Contains(upgradeName);
	}

	// Get all enabled upgrades
	public List<string> GetEnabledUpgrades()
	{
		return enabledUpgrades.ToList();
	}

	// Enable multiple upgrades at once (useful when loading a scene)
	public void EnableUpgrades(params string[] upgradeNames)
	{
		foreach (var upgrade in upgradeNames)
		{
			EnableUpgrade(upgrade);
		}
	}

	// Clear all upgrades
	public void ClearUpgrades()
	{
		enabledUpgrades.Clear();
	}


}
