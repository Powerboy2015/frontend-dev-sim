using Godot;

public partial class HUD : CanvasLayer
{
	private Label MoneyLabel;

	public override void _Ready()
	{
		MoneyLabel = GetNode<Label>("MoneyLabel");
		Wallet.Instance.AddCoins(1000);
		UpdateMoney();
	}
	
	public override void _Process(double delta)
	{
		UpdateMoney();
	}
	
	

	public void UpdateMoney()
	{
		MoneyLabel.Text = Wallet.Instance.Coins.ToString();
	}
}
