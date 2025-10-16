using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

public partial class Wallet : Node2D
{
	public static Wallet Instance { get; private set; }

	public int Coins { get; private set; }

	public override void _Ready()
	{
		Instance = this;
	}

	public void AddCoins(int amount)
	{
		Coins += amount;
	}

	public void RemoveCoins(int amount)
	{
		if (Coins - amount > 0)
		{
			Coins -= amount;
		}

		else
		{
			return;
		}
	}
}
