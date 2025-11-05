using Godot;

public partial class Player : CharacterBody2D, ITeleportable
{
    public void TeleportTo(Vector2 destination)
    {
        GlobalPosition = destination;
        Velocity = Vector2.Zero; // Stop movement after teleport
    }

}