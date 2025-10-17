using Godot;
using System;

public partial class BuyComputer : Control
{
	private Button BuyButton;
	
	public override void _Ready()
	{
		BuyButton = GetNode<Button>("Panel/BuyButton");
		BuyButton.Pressed += HandleButtonPressed;
	}
	
	private void HandleButtonPressed()
	{
		Wallet.Instance.RemoveCoins(300);
	}
}
