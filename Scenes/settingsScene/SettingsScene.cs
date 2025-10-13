using Godot;
using System;

public partial class SettingsScene : Control
{
	private ConfigManager _config;
	
	private readonly string[] RESOLUTIONS = new[]
	{
		"1152x648",
		"1280x720",
		"1366x768",
		"1600x900",
		"1920x1080",
		"2560x1440"
	};

	// Screen preference storing
	private OptionButton _resInput;
	private CheckButton _fullscreenButton;

	// Sound preference storing
	private HSlider _masterSlider;
	private HSlider _musicSlider;
	private HSlider _sfxSlider;

	// Key bind preference storing
	private KeyBindButton _upButton;
	private KeyBindButton _downButton;
	private KeyBindButton _leftButton;
	private KeyBindButton _rightButton;
	private KeyBindButton _interactButton;
	private KeyBindButton _menuButton;
	
	public override void _Ready()
	{
		// Ja pak die singleton ook hier erbij
		_config = GetNode<ConfigManager>("/root/ConfigManager");
		
		var baseDir = "MarginContainer/VBoxContainer/PanelContainer/MarginContainer/VBoxContainer/ScrollContainer/MarginContainer/Settings/";
		
		// Get UI elements
		_resInput = GetNode<OptionButton>(baseDir + "Screen/Resolution/OptionButton");
		foreach (var res in RESOLUTIONS)
		{
			_resInput.AddItem(res);
		}
		
		_fullscreenButton = GetNode<CheckButton>(baseDir + "Screen/Fullscreen/CheckButton");
		_masterSlider = GetNode<HSlider>(baseDir + "Sound/Master/HSlider");
		_musicSlider = GetNode<HSlider>(baseDir + "Sound/Music/HSlider");
		_sfxSlider = GetNode<HSlider>(baseDir + "Sound/SFX/HSlider");
		
		// Keybind stuff ofzo
		_upButton = GetNode<KeyBindButton>(baseDir + "Controls/HBoxContainer/Movement/MoveUp/Button");
		_downButton = GetNode<KeyBindButton>(baseDir + "Controls/HBoxContainer/Movement/MoveDown/Button");
		_leftButton = GetNode<KeyBindButton>(baseDir + "Controls/HBoxContainer/Movement/MoveLeft/Button");
		_rightButton = GetNode<KeyBindButton>(baseDir + "Controls/HBoxContainer/Movement/MoveRight/Button");
		_interactButton = GetNode<KeyBindButton>(baseDir + "Controls/HBoxContainer/Other/Interact/Button");
		_menuButton = GetNode<KeyBindButton>(baseDir + "Controls/HBoxContainer/Other/Menu/Button");
		
		// Fullscreen toggle - enable/disable resolution dropdown
		_fullscreenButton.Toggled += (bool toggled) =>
		{
			_resInput.Disabled = toggled;
		};

		_masterSlider.ValueChanged += (double value) =>
		{
			AudioServer.SetBusVolumeDb(0, (float)value);
		};
		
		_musicSlider.ValueChanged += (double value) =>
		{
			AudioServer.SetBusVolumeDb(1, (float)value);
		};
		
		_sfxSlider.ValueChanged += (double value) =>
		{
			AudioServer.SetBusVolumeDb(2, (float)value);
		};
		
		// Save button
		GetNode<Button>("MarginContainer/VBoxContainer/PanelContainer/MarginContainer/VBoxContainer/SaveButton").Pressed += () =>
		{
			SaveSettings();
			// Show the label
			var savedLabel = GetNode<Label>("MarginContainer/VBoxContainer/PanelContainer/MarginContainer/SavedLabel");
			savedLabel.Show();
		};
		
		// Back button
		GetNode<Button>("BackButton").Pressed += () =>
		{
			GetTree().ChangeSceneToFile("res://scenes/menuScene/menuScene.tscn");
		};
		
		// Update UI with loaded values
		UpdateUIFromConfig();
	}
	
	private void SaveSettings()
	{
		// Save fullscreen first
		GetWindow().Mode = _fullscreenButton.ButtonPressed ? Window.ModeEnum.Fullscreen : Window.ModeEnum.Windowed;

		// Only save resolution if not in fullscreen
		if (GetWindow().Mode != Window.ModeEnum.Fullscreen)
		{
			_config.SetResolution(RESOLUTIONS[_resInput.Selected]);
		}

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
		// Update resolution
		var currentRes = _config.GetResolutionString();
		int resIndex = Array.IndexOf(RESOLUTIONS, currentRes);

		if (resIndex >= 0)
		{
			_resInput.Selected = resIndex;
		}
		
		// Update fullscreen
		bool isFullscreen = GetWindow().Mode == Window.ModeEnum.Fullscreen;
		_fullscreenButton.ButtonPressed = isFullscreen;
		_resInput.Disabled = isFullscreen;
		
		// Update audio
		_masterSlider.Value = AudioServer.GetBusVolumeDb(0);
		_musicSlider.Value = AudioServer.GetBusVolumeDb(1);
		_sfxSlider.Value = AudioServer.GetBusVolumeDb(2);

		// Update keybinds
		_upButton.CurrentKey = (Key)_config.GetControl("MoveUp");
		_downButton.CurrentKey = (Key)_config.GetControl("MoveDown");
		_leftButton.CurrentKey = (Key)_config.GetControl("MoveLeft");
		_rightButton.CurrentKey = (Key)_config.GetControl("MoveRight");
		_interactButton.CurrentKey = (Key)_config.GetControl("Interact");
		_menuButton.CurrentKey = (Key)_config.GetControl("Menu");
	}
}
