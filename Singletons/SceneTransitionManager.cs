using Godot;

public partial class SceneTransitionManager : Node
{
	public NodePath TargetTeleportPointPath { get; set; }

	public override void _Ready()
	{

	}

	private void OnNodeAdded(Node node)
	{
		if (node.GetParent() == GetTree().Root && !string.IsNullOrEmpty(TargetTeleportPointPath))
		{
			CallDeferred(nameof(TeleportPlayerToDestination));
		}
	}

	private void TeleportPlayerToDestination(Node sceneRoot)
	{
		if (string.IsNullOrEmpty(TargetTeleportPointPath))
			return;

		var player = GetTree().GetFirstNodeInGroup("player");

		if (player is ITeleportable teleportable && sceneRoot.HasNode(TargetTeleportPointPath))
		{
			var destinationPoint = sceneRoot.GetNodeOrNull<TeleportPoint2D>(TargetTeleportPointPath);
			if (destinationPoint != null)
			{
				teleportable.TeleportTo(destinationPoint.GlobalPosition);
			}
		}
		else
		{
			GD.PrintErr("Failed to teleport player: either player not found or destination point not found.");
		}

		TargetTeleportPointPath = null;
	}
}
