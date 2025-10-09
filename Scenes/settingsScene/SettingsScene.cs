using Godot;
using System;
using System.Threading;

public partial class SettingsScene : Control
{
	[Export] private NodePath ConfigNodePath;
	private ConfigINI _config;

	private readonly string[] RESOLUTIONS = new[]
	{
		"1152x648"
	};
	
	// Key bind preference storing
	private KeyBindButton _upButton;
	private KeyBindButton _downButton;
	private KeyBindButton _leftButton;
	private KeyBindButton _rightButton;
	private KeyBindButton _interactButton;
	private KeyBindButton _menuButton;
	
	public override void _Ready()
	{
		var baseDir = "VBoxContainer/PanelContainer/MarginContainer/VBoxContainer/ScrollContainer/MarginContainer/Settings/";
		_config = GetNode<ConfigINI>(ConfigNodePath);
		
		if (_config == null)
		{
			GD.PushError("ConfigINI not found. Assign ConfigNodePath in the Inspector.");
			return;
		}
		_config.OnLoad();
		
		// Get UI elements
		var resInput = GetNode<OptionButton>(baseDir + "Screen/Resolution/OptionButton");
		foreach (var res in RESOLUTIONS)
		{
			resInput.AddItem(res);
		}
		
		// Connect resolution change event
		resInput.ItemSelected += (long index) =>
		{
			_config.SetResolution(RESOLUTIONS[(int)index]);
		};
		
		var fullscreen = GetNode<CheckButton>(baseDir + "Screen/Fullscreen/CheckButton");
		var masterSlider = GetNode<HSlider>(baseDir + "Sound/Master/HSlider");
		var musicSlider = GetNode<HSlider>(baseDir + "Sound/Music/HSlider");
		var sfxSlider = GetNode<HSlider>(baseDir + "Sound/SFX/HSlider");
		
		// Keybind stuff ofzo
		_upButton = GetNode<KeyBindButton>(baseDir + "Controls/HBoxContainer/Movement/MoveUp/Button");
		_downButton = GetNode<KeyBindButton>(baseDir + "Controls/HBoxContainer/Movement/MoveDown/Button");
		_leftButton = GetNode<KeyBindButton>(baseDir + "Controls/HBoxContainer/Movement/MoveLeft/Button");
		_rightButton = GetNode<KeyBindButton>(baseDir + "Controls/HBoxContainer/Movement/MoveRight/Button");
		_interactButton = GetNode<KeyBindButton>(baseDir + "Controls/HBoxContainer/Other/Interact/Button");
		_menuButton = GetNode<KeyBindButton>(baseDir + "Controls/HBoxContainer/Other/Menu/Button");
		
		// Connect UI events to save settings
		fullscreen.Toggled += (bool toggled) =>
		{
			GetWindow().Mode = toggled ? Window.ModeEnum.Fullscreen : Window.ModeEnum.Windowed;
		};
		
		masterSlider.ValueChanged += (double value) =>
		{
			AudioServer.SetBusVolumeDb(0, (float)value);
		};
		
		musicSlider.ValueChanged += (double value) =>
		{
			AudioServer.SetBusVolumeDb(1, (float)value);
		};
		
		sfxSlider.ValueChanged += (double value) =>
		{
			AudioServer.SetBusVolumeDb(2, (float)value);
		};
		
		// Save button
		GetNode<Button>("VBoxContainer/PanelContainer/MarginContainer/VBoxContainer/SaveButton").Pressed += () =>
		{
			SaveSettings();
			// Show the label
			var savedLabel = GetNode<Label>("VBoxContainer/PanelContainer/MarginContainer/SavedLabel");
			savedLabel.Show();
		};
		
		// Back button
		GetNode<Button>("BackButton").Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://Scenes/menuScene/menuScene.tscn");
		};
		
		// Update UI with loaded values
		UpdateUIFromConfig();
	}
	
	private void SaveSettings()
	{
		// Save bindings
		_config.SetControl("MoveUp", (int)_upButton.CurrentKey);
		_config.SetControl("MoveDown", (int)_downButton.CurrentKey);
		_config.SetControl("MoveLeft", (int)_leftButton.CurrentKey);
		_config.SetControl("MoveRight", (int)_rightButton.CurrentKey);
		_config.SetControl("Interact", (int)_interactButton.CurrentKey);
		_config.SetControl("Menu", (int)_menuButton.CurrentKey);
		
		_config.Save();
	}
	
	private void UpdateUIFromConfig()
	{
		var baseDir = "VBoxContainer/PanelContainer/MarginContainer/VBoxContainer/ScrollContainer/MarginContainer/Settings/";
		
		// Update resolution
		var currentRes = _config.GetResolutionString();
		var resInput = GetNode<OptionButton>(baseDir + "Screen/Resolution/OptionButton");
		int resIndex = Array.IndexOf(RESOLUTIONS, currentRes);
		if (resIndex >= 0)
		{
			resInput.Selected = resIndex;
		}
		
		// Update fullscreen
		var fullscreen = GetNode<CheckButton>(baseDir + "Screen/Fullscreen/CheckButton");
		fullscreen.ButtonPressed = GetWindow().Mode == Window.ModeEnum.Fullscreen;
		
		// Update audio
		var masterSlider = GetNode<HSlider>(baseDir + "Sound/Master/HSlider");
		var musicSlider = GetNode<HSlider>(baseDir + "Sound/Music/HSlider");
		var sfxSlider = GetNode<HSlider>(baseDir + "Sound/SFX/HSlider");
		
		masterSlider.Value = AudioServer.GetBusVolumeDb(0);
		musicSlider.Value = AudioServer.GetBusVolumeDb(1);
		sfxSlider.Value = AudioServer.GetBusVolumeDb(2);
		
		// Update keybinds
		_upButton.CurrentKey = (Key)_config.GetControl("MoveUp");
		_downButton.CurrentKey = (Key)_config.GetControl("MoveDown");
		_leftButton.CurrentKey = (Key)_config.GetControl("MoveLeft");
		_rightButton.CurrentKey = (Key)_config.GetControl("MoveRight");
		_interactButton.CurrentKey = (Key)_config.GetControl("Interact");
		_menuButton.CurrentKey = (Key)_config.GetControl("Menu");
	}
}
