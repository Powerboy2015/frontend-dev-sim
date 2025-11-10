using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class TeleportPoint2D : Node2D
{
	[Export] public string TeleportId { get; set; } = "";
	[Export] public string Displayname { get; set; } = "Teleport Point";
	[Export] public Array<TeleportPoint2D> ConnectedPoints { get; set; } = new Array<TeleportPoint2D>();
	[Export] public string DestinationTeleportId { get; set; } = "";
	[Export] public bool RequireButtonPress { get; set; } = true; // New: toggle auto vs manual

	private Area2D detectionArea;
	private ITeleportable currentPlayer = null;

	public override void _Ready()
	{
		detectionArea = GetNodeOrNull<Area2D>("Area2D");
		if (detectionArea != null)
		{
			detectionArea.BodyEntered += OnBodyEntered;
			detectionArea.BodyExited += OnBodyExisted;
		}
		else
		{
			GD.PrintErr($"TeleportPoint2D '{Name}' missing Area2D child!");
		}
	}

	public override void _Process(double delta)
	{
		// Check for button press when player is in area
		if (RequireButtonPress && currentPlayer != null)
		{
			if (Input.IsActionJustPressed("ui_accept")) // Space/Enter
			{
				GD.Print($"Player pressed teleport button at '{TeleportId}'");
				Teleport(currentPlayer);
			}
		}
	}

	private void OnBodyEntered(Node body)
	{
		GD.Print($"Body entered teleport point '{TeleportId}': {body.Name}");
		if (body is ITeleportable teleportable)
		{
			currentPlayer = teleportable;

			if (RequireButtonPress)
			{
				GD.Print($"Press [E] or [Space] to teleport to '{Displayname}'");
				// TODO: Show UI prompt here
			}
			else
			{
				// Auto-teleport (original behavior)
				GD.Print($"Teleporting '{body.Name}' via teleport point '{TeleportId}'");
				Teleport(teleportable);
			}
		}
	}

	private void OnBodyExisted(Node body)
	{
		GD.Print($"Body exited teleport point '{TeleportId}': {body.Name}");
		if (body is ITeleportable)
		{
			currentPlayer = null;
			// TODO: Hide UI prompt here
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
