using Godot;
using System;
using System.Collections.Generic;

// Put other sound effect names here to make them public to other files
public enum SFXType
{
	Click,
}

// Put music here
public enum MusicType
{

}

public partial class AudioHandler : Node
{
	public static AudioHandler Instance { get; private set; }
	
	private AudioStreamPlayer _musicPlayer;
	private AudioStreamPlayer _sfxPlayer;

	private Dictionary<SFXType, AudioStream> _sfxLibrary = new();
	private Dictionary<MusicType, AudioStream> _musicLibrary = new();
	
	public override void _EnterTree()
	{
		Instance = this;
	}
	
	public override void _Ready()
	{
		// Create audio players
		_sfxPlayer = new AudioStreamPlayer();
		_sfxPlayer.Bus = "SFX";
		AddChild(_sfxPlayer);
		
		_musicPlayer = new AudioStreamPlayer();
		_musicPlayer.Bus = "Music";
		AddChild(_musicPlayer);
		
		// Load audio files to libraries
		_sfxLibrary[SFXType.Click] = GD.Load<AudioStream>("res://assets/sound/sfx/click.ogg");
	}
	
	// Global Effect Player
	public void PlaySFX(SFXType sfxType)
	{
		if (_sfxLibrary.TryGetValue(sfxType, out AudioStream sound))
		{
			_sfxPlayer.Stream = sound;
			_sfxPlayer.Play();
		}
		else
		{
			GD.PrintErr($"SFX '{sfxType}' not found in library");
		}
	}
	
	// Global Music player
	public void PlayMusic(MusicType musicType, bool loop = true)
	{
		if (_musicLibrary.TryGetValue(musicType, out AudioStream music))
		{
			_musicPlayer.Stream = music;
			_musicPlayer.Play();
		}
		else
		{
			GD.PrintErr($"Music '{musicType}' not found in library");
		}
	}
	
	public void StopMusic()
	{
		_musicPlayer.Stop();
	}
	
	public static float LinearToDb(float linear)
	{
		if (linear < +0)
			return -80f; // Minimum dB value
		float normalized = linear / 100f;
		return Mathf.LinearToDb(normalized);
	}
	
	public static float DbToLinear(float db)
	{
		if (db <= -80f)
			return 0f; // Minimum linear value
		float normalized = Mathf.DbToLinear(db);
		return normalized * 100f;
	}
	
	public void SetBusVolume(string busName, float volume)
	{
		int busIndex = AudioServer.GetBusIndex(busName);
		AudioServer.SetBusVolumeDb(busIndex, LinearToDb(volume));
	}
	
	public float GetBusVolume(string busName)
	{
		int busIndex = AudioServer.GetBusIndex(busName);
		return DbToLinear(AudioServer.GetBusVolumeDb(busIndex));
	}
}
