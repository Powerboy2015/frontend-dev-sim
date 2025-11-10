using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class TeleportPoint2D : Node2D
{
	[Export] public string TeleportId { get; set; } = "";
	[Export] public string Displayname { get; set; } = "Teleport Point";
	[Export] public Array<TeleportPoint2D> ConnectedPoints { get; set; } = new Array<TeleportPoint2D>();

	[Export] public string DestinationTeleportId { get; set; } = ""; // Just use normal Export for now

	private Area2D detectionArea;

	public override void _Ready()
	{
		detectionArea = GetNodeOrNull<Area2D>("Area2D");
		if (detectionArea != null)
		{
			detectionArea.BodyEntered += OnBodyEntered;
		}
		else
		{
			GD.PrintErr($"TeleportPoint2D '{Name}' missing Area2D child!");
		}
	}

	private void OnBodyEntered(Node body)
	{
		GD.Print($"Body entered teleport point '{TeleportId}': {body.Name}");
		if (body is ITeleportable teleportable)
		{
			GD.Print($"Teleporting '{body.Name}' via teleport point '{TeleportId}'");
			Teleport(teleportable);
		}
	}

	public void Teleport(ITeleportable teleportable)
	{
		var sceneManager = GetNode<SceneTransitionManager>("/root/SceneTransitionManager");

		// Same scene teleport (legacy)
		if (ConnectedPoints.Count > 0)
		{
			teleportable.TeleportTo(ConnectedPoints[0].GlobalPosition);
		}
		// Cross-scene or same-scene teleport by ID
		else if (!string.IsNullOrEmpty(DestinationTeleportId))
		{
			var destinationData = sceneManager.GetTeleportPointData(DestinationTeleportId);
			if (destinationData != null)
			{
				sceneManager.TargetTeleportId = DestinationTeleportId;
				string currentScenePath = GetTree().CurrentScene?.SceneFilePath;

				if (destinationData.ScenePath != currentScenePath)
				{
					GetTree().ChangeSceneToFile(destinationData.ScenePath);
				}
				else
				{
					sceneManager.CallDeferred(nameof(SceneTransitionManager.TeleportPlayerToDestination));
				}
			}
			else
			{
				GD.PrintErr($"Destination teleport point '{DestinationTeleportId}' not found in registry!");
			}
		}
	}
}
