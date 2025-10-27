using Godot;
using System;

public partial class ClientSettings : Node
{
    public static ClientBase ChosenClient;

    public static void ChangeClient(ClientBase client)
    {
        ChosenClient = client;
    }

}
