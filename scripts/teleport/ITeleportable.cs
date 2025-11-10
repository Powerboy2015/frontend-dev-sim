using Godot;



// This is an teleportable interface that we will use in order to give it to the teleport object.
// It can impelement this in order to call it back later.
public interface ITeleportable
{
	public void TeleportTo(Vector2 destination);
}
