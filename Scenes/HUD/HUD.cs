using Godot;

public partial class HUD : CanvasLayer
{
	private Label MoneyLabel;
	[Export] public CpuParticles2D cpuParticles2D;

	public override void _Ready()
	{
		MoneyLabel = GetNode<Label>("MarginContainer/MarginContainer/HBoxContainer/MoneyLabel");
		Wallet.Instance.AddCoins(1000);
		UpdateMoney();
		Wallet.Instance.CoinsChanged += OnCoinsChanged;
	}

	private void OnCoinsChanged()
	{
		// Safety check in case this gets called after the node is freed
		if (cpuParticles2D != null && IsInstanceValid(cpuParticles2D))
		{
			cpuParticles2D.Emitting = true;
		}
	}

	// Unsubscribe when HUD is freed
	public override void _ExitTree()
	{
		if (Wallet.Instance != null)
		{
			Wallet.Instance.CoinsChanged -= OnCoinsChanged;
		}
	}

	public override void _Process(double delta)
	{
		UpdateMoney();

		if (Input.IsActionJustPressed("menu"))
		{
			var menu = GetNode<CanvasLayer>("PauseMenu");

			if (menu.Visible)
			{
				// Hide menu
				GD.Print("Menu Close");
				menu.Hide();
				GetTree().Paused = false;
			}
			else
			{
				// Show menu
				GD.Print("Menu Open");
				menu.Show();
				GetTree().Paused = true;
			}
		}

	}

	public void UpdateMoney()
	{
		MoneyLabel.Text = Wallet.Instance.Coins.ToString();

	}
}
