using Godot;

public partial class HUD : CanvasLayer
{
	private Label MoneyLabel;

	public override void _Ready()
	{
		MoneyLabel = GetNode<Label>("MarginContainer/HBoxContainer/MoneyLabel");
		Wallet.Instance.AddCoins(1000);
		UpdateMoney();
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
			} else {
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
