using Godot;
using System;

public partial class ChooseClient : Control
{
    private string StaticDir = "res://Resources/Clients/";
    private ClientBase Client;

    [Export] private PackedScene baller;

    public override void _Ready()
    {
        var ClientName = GetNode<Label>("Panel/ClientName").Text;

        Client = GD.Load<ClientBase>(StaticDir + ClientName + ".tres");
    }

    private void ChosenClient()
    {
        ClientSettings.ChangeClient(Client);

        var test = GetParent().GetParent().GetParent().GetParent().GetParent();
        var instance = baller.Instantiate();
        test.AddChild(instance);
        
        GetParent().GetParent().GetParent().QueueFree();
    }
}
