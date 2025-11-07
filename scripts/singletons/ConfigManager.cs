using Godot;
using System;

public partial class ConfigManager : Node
{
	// 
	public static ConfigManager Instance { get; private set; }
	
	private ConfigFile CF = new ConfigFile();
	
	public string CFName = "settings.cfg";
	
	public override void _EnterTree()
	{
		Instance = this;
		OnLoad();
	}
		
	public int GetControl(string control)
	{
		return (int)(long)CF.GetValue("Controls", control, (int)Key.None);
	}
	
	public void SetControl(string control, int keyCode)
	{
		CF.SetValue("Controls", control, keyCode);
	}
	
	public void OnLoad()
	{
		if (CF.Load("user://" + CFName) != Error.Ok)
		{
			GD.Print("No existing config file found. Creating a new one.");
			Save();
		}
		Load();
	}
	
	public void Save()
	{
		// Screen settings
		var window = GetWindow();
		CF.SetValue("Screen", "ResolutionWidth", window.ContentScaleSize.X);
		CF.SetValue("Screen", "ResolutionHeight", window.ContentScaleSize.Y);
		CF.SetValue("Screen", "Fullscreen", window.Mode == Window.ModeEnum.Fullscreen);
		
		// Sound settings
		CF.SetValue("Sound", "Master", AudioServer.GetBusVolumeDb(0));
		CF.SetValue("Sound", "Music", AudioServer.GetBusVolumeDb(1));
		CF.SetValue("Sound", "SFX", AudioServer.GetBusVolumeDb(2));
		
		// Control settings
		if (!CF.HasSectionKey("Controls", "MoveUp"))
			CF.SetValue("Controls", "MoveUp", (int)Key.W);
		if (!CF.HasSectionKey("Controls", "MoveDown"))
			CF.SetValue("Controls", "MoveDown", (int)Key.S);
		if (!CF.HasSectionKey("Controls", "MoveLeft"))
			CF.SetValue("Controls", "MoveLeft", (int)Key.A);
		if (!CF.HasSectionKey("Controls", "MoveRight"))
			CF.SetValue("Controls", "MoveRight", (int)Key.D);
		if (!CF.HasSectionKey("Controls", "Interact"))
			CF.SetValue("Controls", "Interact", (int)Key.E);
		if (!CF.HasSectionKey("Controls", "Menu"))
			CF.SetValue("Controls", "Menu", (int)Key.Escape);
			
		// Control settings
		ApplyControlToInputMap("move_up", "MoveUp");
		ApplyControlToInputMap("move_down", "MoveDown");
		ApplyControlToInputMap("move_left", "MoveLeft");
		ApplyControlToInputMap("move_right", "MoveRight");
		ApplyControlToInputMap("interact", "Interact");
		ApplyControlToInputMap("menu", "Menu");
			
		CF.Save("user://" + CFName);
		GD.Print("Settings saved to " + ProjectSettings.GlobalizePath("user://" + CFName));
	}
	
	public void Load()
	{
		var window = GetWindow();
		var current = window.Size;
		
		// Screen settings
		int width = (int)(long)CF.GetValue("Screen", "ResolutionWidth", (long)current.X);
		int height = (int)(long)CF.GetValue("Screen", "ResolutionHeight", (long)current.Y);
		bool fullscreen = (bool)CF.GetValue("Screen", "Fullscreen", false);

		window.Size = new Vector2I(width, height);
		window.Mode = fullscreen ? Window.ModeEnum.Fullscreen : Window.ModeEnum.Windowed;

		// Sound settings
		float master = (float)CF.GetValue("Sound", "Master", 0.0f);
		float music = (float)CF.GetValue("Sound", "Music", 0.0f);
		float sfx = (float)CF.GetValue("Sound", "SFX", 0.0f);

		AudioServer.SetBusVolumeDb(0, master);
		AudioServer.SetBusVolumeDb(1, music);
		AudioServer.SetBusVolumeDb(2, sfx);
		
		// Control settings
		ApplyControlToInputMap("move_up", "MoveUp");
		ApplyControlToInputMap("move_down", "MoveDown");
		ApplyControlToInputMap("move_left", "MoveLeft");
		ApplyControlToInputMap("move_right", "MoveRight");
		ApplyControlToInputMap("interact", "Interact");
		ApplyControlToInputMap("menu", "Menu");
	}
	
	private void ApplyControlToInputMap(string action, string configKey)
	{
		// Get key from config
		int keyCode = (int)(long)CF.GetValue("Controls", configKey, (int)Key.None);
		
		if (keyCode == (int)Key.None)
			return;
			
		// Clear existing mappings
		if (InputMap.HasAction(action))
		{
			InputMap.ActionEraseEvents(action);
		}
		else
		{
			InputMap.AddAction(action);
		}
		
		// Add new key mapping
		var keyEvent = new InputEventKey();
		keyEvent.Keycode = (Key)keyCode;
		InputMap.ActionAddEvent(action, keyEvent);
	}
	
	private Vector2I ParseResolution(string resolution)
	{
		var parts = resolution.Split('x');
		if (parts.Length == 2 && int.TryParse(parts[0], out int width) && int.TryParse(parts[1], out int height))
		{
			return new Vector2I(width, height);
		}
		return new Vector2I(1152, 648);
	}

	private string FormatResolution(Vector2I resolution)
	{
		return $"{resolution.X}x{resolution.Y}";
	}
	
	public string GetResolutionString()
	{
		var size = GetWindow().Size;
		int width = (int)(long)CF.GetValue("Screen", "ResolutionWidth", (long)size.X);
		int height = (int)(long)CF.GetValue("Screen", "ResolutionHeight", (long)size.Y);
		return FormatResolution(new Vector2I(width, height));
	}
	
	public void SetResolution(string resolution)
	{
		var size = ParseResolution(resolution);
		CF.SetValue("Screen", "ResolutionWidth", size.X);
		CF.SetValue("Screen", "ResolutionHeight", size.Y);

		var window = GetWindow();
		window.Size = size;
	}
}
