using Godot;
using System;

public partial class HowToPlayScene : Control
{
	public override void _Ready()
	{
		GetNode<Button>("BackButton").Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://scenes/menuScene/menuScene.tscn");
		};
	}
}
