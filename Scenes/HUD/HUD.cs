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
				AudioHandler.Instance.PlaySFX(SFXType.Click);
				menu.Hide();
			} else {
				// Show menu
				GD.Print("Menu Open");
				AudioHandler.Instance.PlaySFX(SFXType.Click);
				menu.Show();
			}
		}
		
	}
	
	public void UpdateMoney()
	{
		MoneyLabel.Text = Wallet.Instance.Coins.ToString();
	}
}
