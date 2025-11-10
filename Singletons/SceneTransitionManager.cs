using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class SceneTransitionManager : Node
{
	[Export]
	public TeleportRegistry Registry { get; set; }

	public string TargetTeleportId { get; set; }

	private Dictionary<string, TeleportPoint2D> activeTeleportPoints = new();

	public override void _Ready()
	{
		// Load the registry if not assigned
		if (Registry == null)
		{
			Registry = GD.Load<TeleportRegistry>("res://teleport_registry.tres");
			if (Registry == null)
			{
				GD.PrintErr("Failed to load teleport registry!");
			}
			else
			{
				GD.Print($"Loaded registry with {Registry.TeleportPointIds.Count} teleport points");
			}
		}

		GetTree().NodeAdded += OnNodeAdded;
		GetTree().NodeRemoved += OnNodeRemoved;
	}

	private void OnNodeAdded(Node node)
	{
		// Track active teleport points in current scene
		if (node is TeleportPoint2D teleportPoint && !string.IsNullOrEmpty(teleportPoint.TeleportId))
		{
			activeTeleportPoints[teleportPoint.TeleportId] = teleportPoint;
			GD.Print($"Registered active teleport point: {teleportPoint.TeleportId}");
		}

		// Handle scene changes
		if (node.GetParent() == GetTree().Root && !string.IsNullOrEmpty(TargetTeleportId))
		{
			CallDeferred(nameof(TeleportPlayerToDestination));
		}
	}

	private void OnNodeRemoved(Node node)
	{
		if (node is TeleportPoint2D teleportPoint && !string.IsNullOrEmpty(teleportPoint.TeleportId))
		{
			activeTeleportPoints.Remove(teleportPoint.TeleportId);
			GD.Print($"Unregistered teleport point: {teleportPoint.TeleportId}");
		}
	}

	public void TeleportPlayerToDestination()
	{
		if (string.IsNullOrEmpty(TargetTeleportId))
			return;

		var player = GetTree().GetFirstNodeInGroup("player");

		if (player == null || player is not ITeleportable teleportable)
		{
			GD.PrintErr("Player not found or doesn't implement ITeleportable!");
			TargetTeleportId = null;
			return;
		}

		// Check if destination is in current scene
		if (activeTeleportPoints.TryGetValue(TargetTeleportId, out var destinationPoint))
		{
			teleportable.TeleportTo(destinationPoint.GlobalPosition);
			GD.Print($"Teleported to {TargetTeleportId} at {destinationPoint.GlobalPosition}");
		}
		else
		{
			GD.PrintErr($"Teleport point {TargetTeleportId} not found in current scene!");
		}

		TargetTeleportId = null;
	}

	// Get all teleport point IDs from the registry (for dropdowns)
	public TeleportPointData GetTeleportPointData(string id)
	{
		return Registry?.GetTeleportPointData(id);
	}

	// Get all teleport point IDs from the registry (for dropdowns)
	public string[] GetAllTeleportIds()
	{
		return Registry?.GetAllTeleportIds() ?? new string[0];
	}
}
