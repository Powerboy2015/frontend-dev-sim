using Godot;
using System;

public partial class MenuScene : Control
{
	[Export] private NodePath ConfigNodePath;
	private ConfigINI _config;
	public override void _Ready()
	{
		_config = GetNodeOrNull<ConfigINI>(ConfigNodePath);
		if (_config == null)
		{
			GD.PushError("ConfigINI not found. Assign ConfigNodePath in the Inspector.");
			return;
		}
		_config.OnLoad();
		
		GetNode<Button>("VBoxContainer/StartButton").Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://Scenes/gameScene/gameScene.tscn");
		};
		
		GetNode<Button>("VBoxContainer/SettingsButton").Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://Scenes/settingsScene/settingsScene.tscn");
		};
		
		GetNode<Button>("VBoxContainer/HowToPlayButton").Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://Scenes/howToPlayScene/howToPlayScene.tscn");
		};
		
		GetNode<Button>("VBoxContainer/QuitButton").Pressed += () =>
		{
			GetTree().Quit();
		};
	}
}
