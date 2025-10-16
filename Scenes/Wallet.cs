using Godot;
using System;

public partial class Wallet : Node2D
{
	public int coins = 0;

	public Label Coins;

	public override void _Ready()
	{
		Coins = GetNode<Label>("Coins");
		Coins.Text = coins.ToString();
		
		var button1 =  GetNode<Button>("Button1");
		button1.Pressed += () => AddCoins(1);
		
		var button5 =  GetNode<Button>("Button2");
		button5.Pressed += () => AddCoins(5);
		
		var buttonMin1 =  GetNode<Button>("Button3");
		buttonMin1.Pressed += () => RemoveCoins(1);
		
		var buttonMin5 =  GetNode<Button>("Button4");
		buttonMin5.Pressed += () => RemoveCoins(5);
}
	public void AddCoins(int amount)
	{
		coins += amount;
		Coins.Text = coins.ToString();
	}

	public void RemoveCoins(int amount)
	{
		if (coins > 0)
		{
			coins -= amount;

			if (coins >= 0)
			{

				Coins.Text = coins.ToString();
			}
			else
			{
				coins = 0;
				Coins.Text = coins.ToString();
			}
		}
	}
}
