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

        var CanvasLayer = GetParent().GetParent().GetParent().GetParent().GetParent();
        var instance = baller.Instantiate(); // idk why but if i change baller to something else it stops working lmao
        CanvasLayer.AddChild(instance);

        GetParent().GetParent().GetParent().QueueFree();
    }
}
