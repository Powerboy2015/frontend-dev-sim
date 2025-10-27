using Godot;
using System;

public partial class CoinCount : Control
{
    private Label coinCount;
    public override void _Ready()
    {
        coinCount = GetNode<Label>("Panel/Coins/Count");
        coinCount.Text = Wallet.Instance.Coins.ToString();
    }
}
