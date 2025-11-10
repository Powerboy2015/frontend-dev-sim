using Godot;
using System;
using System.Linq;

public partial class RoomUpgradeLoader : Node
{
    public override void _Ready()
    {

        UpgradeManager.instance.GetEnabledUpgrades().ForEach(upgradeName =>
        {
            OnUpgradeEnabled(upgradeName);
        });

        UpgradeManager.instance.UpgradeEnabled += OnUpgradeEnabled;
    }

    private void OnUpgradeEnabled(string upgradeName)
    {
        GD.Print($"Upgrade enabled: {upgradeName}");

        Sprite2D upgradeSprite = GetNodeOrNull<Sprite2D>(upgradeName);
        if (upgradeSprite != null)
        {
            upgradeSprite.Visible = true;
        }
        else
        {
            GD.PrintErr($"Upgrade sprite '{upgradeName}' not found in RoomUpgradeLoader!");
        }
    }
}