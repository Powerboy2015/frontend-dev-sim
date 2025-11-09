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
			AudioHandler.Instance.PlaySFX(SFXType.Click);
			GetTree().ChangeSceneToFile("res://scenes/storeScene/Shop.tscn");
		};

		GetNode<Button>("MarginContainer/VBoxContainer/VBoxContainer/SettingsButton").Pressed += () =>
		{
			AudioHandler.Instance.PlaySFX(SFXType.Click);
			GetTree().ChangeSceneToFile("res://scenes/menuScene/settingsScene/settingsScene.tscn");
		};

		GetNode<Button>("MarginContainer/VBoxContainer/VBoxContainer/HowToPlayButton").Pressed += () =>
		{
			AudioHandler.Instance.PlaySFX(SFXType.Click);
			GetTree().ChangeSceneToFile("res://scenes/menuScene/howToPlayScene/howToPlayScene.tscn");
		};

		GetNode<Button>("MarginContainer/VBoxContainer/VBoxContainer/CreditsButton").Pressed += () =>
		{
			AudioHandler.Instance.PlaySFX(SFXType.Click);
			GetTree().ChangeSceneToFile("res://scenes/menuScene/creditsScene/creditsScene.tscn");
		};

		GetNode<Button>("MarginContainer/VBoxContainer/VBoxContainer/QuitButton").Pressed += () =>
		{
			AudioHandler.Instance.PlaySFX(SFXType.Click);
			GetTree().Quit();
		};
	}
}
