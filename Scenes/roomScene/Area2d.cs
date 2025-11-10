using Godot;
using System;

public partial class Area2d : Area2D
{
	private bool _playerPresent = false;
	private Button _computerButton;
	private Control _clientSceneLayer;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;

		_computerButton = GetNode<Button>("../../../Computer/Button");
		_computerButton.Pressed += OnComputerButtonPressed;

		_clientSceneLayer = GetNode<Control>("../../../ClientScene/ClientScreen");
		_clientSceneLayer.Hide();
	}

	public override void _Process(double delta)
	{
		if (_playerPresent && Input.IsActionJustPressed("interact"))
		{
			ToggleClientScene();
		}
	}

	private void OnComputerButtonPressed()
	{
		ToggleClientScene();
	}

	private void ToggleClientScene()
	{
		var opening = !_clientSceneLayer.Visible;
		if (opening)
		{
			AudioHandler.Instance.PlaySFX(SFXType.Click);
			_clientSceneLayer.Show();
			// GetTree().Paused = true;
		}
		else
		{
			AudioHandler.Instance.PlaySFX(SFXType.Click);
			_clientSceneLayer.Hide();
			// GetTree().Paused = false;
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			GD.Print("Player entered");
			_playerPresent = true;
			_computerButton.Show();
		}
	}

	private void OnBodyExited(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			GD.Print("Player left");
			_playerPresent = false;
			_computerButton.Hide();
		}
	}
}
