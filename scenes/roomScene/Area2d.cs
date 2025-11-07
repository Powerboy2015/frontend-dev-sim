using Godot;
using System;

public partial class Area2d : Area2D
{
	public override void _Ready()
	{
		// Connect the signals to methods
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}
	
	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			GD.Print("Player entered");
			var button = GetNode<TouchScreenButton>("RoomView/Button");
			button.Show();
		}
	}
			
	private void OnBodyExited(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			GD.Print("Player left");
		}
	}
}
