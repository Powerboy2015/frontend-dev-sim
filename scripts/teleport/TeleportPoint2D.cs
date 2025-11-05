using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class TeleportPoint2D : Node2D
{
	[Export]
	public Array<TeleportPoint2D> ConnectedPoints { get; set; } = new Array<TeleportPoint2D>();

	[Export]
	public PackedScene DestinationScene { get; set; }

	[Export]
	public string DestinationPointPath { get; set; } // Autocomplete dropdown in inspector!

	private Area2D detectionArea;

	public override void _Ready()
	{
		detectionArea = GetNode<Area2D>("Area2D");
		detectionArea.BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node body)
	{
		if (body is ITeleportable teleportable)
		{
			Teleport(teleportable);
		}
	}

	public void Teleport(ITeleportable teleportable)
	{
		// Same scene teleport
		if (ConnectedPoints.Count > 0)
		{
			teleportable.TeleportTo(ConnectedPoints[0].GlobalPosition);
		}
		// Cross-scene teleport
		else if (DestinationScene != null)
		{
			// Store the destination path for the next scene
			var sceneManager = GetNode<SceneTransitionManager>("/root/SceneTransitionManager");
			sceneManager.TargetTeleportPointPath = DestinationPointPath;
			GetTree().ChangeSceneToPacked(DestinationScene);
		}
	}
}
