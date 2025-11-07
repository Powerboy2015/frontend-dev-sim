using Godot;
using System;

public partial class HowToPlayScene : Control
{
	private ConfigManager _config;
	
	// Key bind preference storing
	private Button _upButton;
	private Button _downButton;
	private Button _leftButton;
	private Button _rightButton;
	private Button _interactButton;
	private Button _menuButton;
	
	public override void _Ready()
	{
		_config = GetNode<ConfigManager>("/root/ConfigManager");
		
		GetNode<Button>("BackButton").Pressed += () =>
		{
			AudioHandler.Instance.PlaySFX(SFXType.Click);
			GetTree().ChangeSceneToFile("res://scenes/menuScene/menuScene.tscn");
		};
		LoadFromConfig();
	}
	
	private void LoadFromConfig()
	{
		var baseDir = "MarginContainer/VBoxContainer/PanelContainer/ScrollContainer/MarginContainer/VBoxContainer/Controls/HBoxContainer/";
		
		_upButton = GetNode<Button>(baseDir + "Movement/MoveUp/Button");
		_downButton = GetNode<Button>(baseDir + "Movement/MoveDown/Button");
		_leftButton = GetNode<Button>(baseDir + "Movement/MoveLeft/Button");
		_rightButton = GetNode<Button>(baseDir + "Movement/MoveRight/Button");
		_interactButton = GetNode<Button>(baseDir + "Other/Interact/Button");
		_menuButton = GetNode<Button>(baseDir + "Other/Menu/Button");
		
		// Update keybinds from Config
		_upButton.Text = ((Key)_config.GetControl("MoveUp")).ToString();
		_downButton.Text = ((Key)_config.GetControl("MoveDown")).ToString();
		_leftButton.Text = ((Key)_config.GetControl("MoveLeft")).ToString();
		_rightButton.Text = ((Key)_config.GetControl("MoveRight")).ToString();
		_interactButton.Text = ((Key)_config.GetControl("Interact")).ToString();
		_menuButton.Text = ((Key)_config.GetControl("Menu")).ToString();
	}
}
