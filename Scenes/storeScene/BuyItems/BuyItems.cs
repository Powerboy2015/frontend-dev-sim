using Godot;
using System;

public partial class BuyItems : Control
{
	private Button BuyButton;
	private Label Amount;

	public override void _Ready()
	{
		BuyButton = GetNode<Button>("Panel/BuyButton");
		BuyButton.Pressed += () => HandleButtonPressed();
	}

	private void HandleButtonPressed()
	{
		Amount = GetNode<Label>("Panel/Price");
		int x = Int32.Parse(Amount.Text);
		GD.Print(x);
		Wallet.Instance.RemoveCoins(x);

		// XXX Names of objects are now directly relevant to upgrades. Use them in order to enable upgrades.
		UpgradeManager.instance.EnableUpgrade(Name);
	}
}
