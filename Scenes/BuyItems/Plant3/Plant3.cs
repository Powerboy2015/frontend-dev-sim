using Godot;
using System;

public partial class Plant3 : Control
{
	private Button BuyButton;
	
	public override void _Ready()
	{
		BuyButton = GetNode<Button>("Panel/BuyButton");
		BuyButton.Pressed += HandleButtonPressed;
	}
	
	private void HandleButtonPressed()
	{
		Wallet.Instance.RemoveCoins(100);
	}
}
