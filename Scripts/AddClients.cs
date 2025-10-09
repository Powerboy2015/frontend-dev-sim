using Godot;
using Godot.Collections;
using System;

public partial class AddClients : ScrollContainer
{
    [Export] private PackedScene clientBase;
    [Export] private Array<ClientBase> Clients;

    private VBoxContainer Container;

    public override void _Ready()
    {
        Container = GetNode<VBoxContainer>("Content");

        foreach (ClientBase Client in Clients)
        {
            var instance = clientBase.Instantiate();
            instance.GetNode<Label>("Panel/ClientName").Text = Client.Name;
            instance.GetNode<Label>("Panel/ClientDescription").Text = Client.Description;
            instance.GetNode<Button>("Panel/Button").Text = "help " + Client.Name;
            Container.AddChild(instance);
        }
    }

}
