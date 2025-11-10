using Godot;

public partial class Player : CharacterBody2D, ITeleportable
{
	[Export]
	public float speed { get; set; } = 300.0f;

	public CharacterBody2D collisionPolygon2D;

	private Area2D currentTeleportArea = null;

	[Export]
	public NodePath enterButtonIcon;

	[Export]
	public Sprite2D JerryIcon;
	private Sprite2D enterButton;

	public void TeleportTo(Vector2 destination)
	{
		GlobalPosition = destination;
		Velocity = Vector2.Zero;
		GD.Print($"Player teleported to {destination}");
	}

	public override void _Ready()
	{
		AddToGroup("player");
		enterButton = GetNode<Sprite2D>(enterButtonIcon);
		enterButton.Visible = false;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		if (direction != Vector2.Zero)
		{
			velocity = direction * speed;
		}
		else
		{
			velocity = Vector2.Zero;
		}

		if (direction.X < 0)
		{
			JerryIcon.FlipH = true;
		}
		else if (direction.X > 0)
		{
			JerryIcon.FlipH = false;
		}

		Velocity = velocity;
		MoveAndSlide();

		// Check for teleport input when in teleport area
		if (currentTeleportArea != null && Input.IsActionJustPressed("ui_accept"))
		{
			// Trigger teleport on the TeleportPoint2D
			if (currentTeleportArea.HasMethod("Teleport"))
			{
				currentTeleportArea.Call("Teleport");
			}
		}
	}

	public void SetTeleportArea(Area2D area)
	{
		currentTeleportArea = area;
	}

	public void ClearTeleportArea(Area2D area)
	{
		if (currentTeleportArea == area)
		{
			currentTeleportArea = null;
		}
	}


	public void DisplayIcon(bool _display)
	{
		enterButton.Visible = _display;
	}

}
