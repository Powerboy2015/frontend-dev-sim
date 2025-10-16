using Godot;
using System;

public partial class MenuScene : Control
{
	public override void _Ready()
	{
		// Hoppa pak die singleton erbij
		var config = GetNode<ConfigManager>("/root/ConfigManager");
		
		GetNode<Button>("MarginContainer/VBoxContainer/VBoxContainer/StartButton").Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://scenes/gameScene/gameScene.tscn");
		};
		
		GetNode<Button>("MarginContainer/VBoxContainer/VBoxContainer/SettingsButton").Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://scenes/settingsScene/settingsScene.tscn");
		};
		
		GetNode<Button>("MarginContainer/VBoxContainer/VBoxContainer/HowToPlayButton").Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://scenes/howToPlayScene/howToPlayScene.tscn");
		};
		
		GetNode<Button>("MarginContainer/VBoxContainer/VBoxContainer/CreditsButton").Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://scenes/howToPlayScene/howToPlayScene.tscn");
		};
		
		GetNode<Button>("MarginContainer/VBoxContainer/VBoxContainer/QuitButton").Pressed += () =>
		{
			GetTree().Quit();
		};
	}
}
