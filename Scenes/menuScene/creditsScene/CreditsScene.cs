using Godot;
using System;

public partial class CreditsScene : Control
{
	public override void _Ready()
	{
		GetNode<Button>("MarginContainer/BackButton").Pressed += () =>
		{
			AudioHandler.Instance.PlaySFX(SFXType.Click);
			GetTree().ChangeSceneToFile("res://scenes/menuScene/menuScene.tscn");
		};
	}
}
