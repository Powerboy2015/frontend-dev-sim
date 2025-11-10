using Godot;
using System;

public partial class ClientScreen : Control
{
	private Button _closeButton;

	public override void _Ready()
	{
		_closeButton = GetNode<Button>("Panel/MenuBar/Button");
		_closeButton.Pressed += OnCloseWindowPressed;
		Visible = true;
	}

	private void OnCloseWindowPressed()
	{
		AudioHandler.Instance.PlaySFX(SFXType.Click);
		var layer = GetParent() as CanvasLayer;
		if (layer != null)
			layer.Hide();
		else
			Visible = false;

		GetTree().Paused = false;
	}
}
