using Godot;
using System;

public partial class ClientScreen : Control
{
	private Button _closeButton;
	private Label coinCount;

	public override void _Ready()
	{
		coinCount = GetNode<Label>("Panel/Coins/Count");
		coinCount.Text = Wallet.Instance.Coins.ToString();

		_closeButton = GetNode<Button>("Panel/MenuBar/Button");
		_closeButton.Pressed += OnClosePressed;
		Visible = true;
	}

	private void OnClosePressed()
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
