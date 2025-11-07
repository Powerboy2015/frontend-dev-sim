using Godot;
using System;

public partial class PauseMenu : CanvasLayer
{
	public override void _Ready()
	{
		GetNode<Button>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/Continue").Pressed += () =>
		{
			AudioHandler.Instance.PlaySFX(SFXType.Click);
			var menu = GetNode<CanvasLayer>("./");
			menu.Hide();
			GetTree().Paused = false;
		};
		
		GetNode<Button>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/Menu").Pressed += () =>
		{
			AudioHandler.Instance.PlaySFX(SFXType.Click);
			GetTree().Paused = false;
			GetTree().ChangeSceneToFile("res://scenes/menuScene/menuScene.tscn");
		};
	}
	
}
