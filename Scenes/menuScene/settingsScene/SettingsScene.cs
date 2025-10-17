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
		
		_masterSlider.MinValue = 0;
		_masterSlider.MaxValue = 100;
		_musicSlider.MinValue = 0;
		_musicSlider.MaxValue = 100;
		_sfxSlider.MinValue = 0;
		_sfxSlider.MaxValue = 100;
		
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
			AudioHandler.Instance.SetBusVolume("Master", (float)value);
		};
		
		_musicSlider.ValueChanged += (double value) =>
		{
			AudioHandler.Instance.SetBusVolume("Music", (float)value);
		};
		
		_sfxSlider.ValueChanged += (double value) =>
		{
			AudioHandler.Instance.SetBusVolume("SFX", (float)value);
		};
		
		// Save button
		GetNode<Button>("MarginContainer/VBoxContainer/PanelContainer/MarginContainer/VBoxContainer/SaveButton").Pressed += () =>
		{
			AudioHandler.Instance.PlaySFX(SFXType.Click);
			SaveSettings();
			// Show the label
			var savedLabel = GetNode<Label>("MarginContainer/VBoxContainer/PanelContainer/MarginContainer/SavedLabel");
			savedLabel.Show();
		};
		
		// Back button
		GetNode<Button>("MarginContainer/BackButton").Pressed += () =>
		{
			AudioHandler.Instance.PlaySFX(SFXType.Click);
			GetTree().ChangeSceneToFile("res://scenes/menuScene/menuScene.tscn");
		};
		
		// Update UI with loaded values
		UpdateUIFromConfig();
	}
	
	// Convert linear volume (0-100) to decibels (-80 to 0)
	private float LinearToDb(float linear)
	{
		if (linear <= 0)
			return -80.0f; // Mute
		
		// Convert 0-100 to 0-1 range, then to dB
		float normalized = linear / 100.0f;
		return Mathf.LinearToDb(normalized);
	}
	
	// Convert decibels to linear volume (0-100)
	private float DbToLinear(float db)
	{
		if (db <= -80.0f)
			return 0.0f; // Muted
			
		// Convert dB to 0-1 range, then to 0-100
		float normalized = Mathf.DbToLinear(db);
		return normalized * 100.0f;
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
		_masterSlider.Value = AudioHandler.Instance.GetBusVolume("Master");
		_musicSlider.Value = AudioHandler.Instance.GetBusVolume("Music");
		_sfxSlider.Value = AudioHandler.Instance.GetBusVolume("SFX");
		
		// Update keybinds
		_upButton.CurrentKey = (Key)_config.GetControl("MoveUp");
		_downButton.CurrentKey = (Key)_config.GetControl("MoveDown");
		_leftButton.CurrentKey = (Key)_config.GetControl("MoveLeft");
		_rightButton.CurrentKey = (Key)_config.GetControl("MoveRight");
		_interactButton.CurrentKey = (Key)_config.GetControl("Interact");
		_menuButton.CurrentKey = (Key)_config.GetControl("Menu");
	}
}
