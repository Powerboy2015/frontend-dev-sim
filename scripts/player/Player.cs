using Godot;

public partial class Player : CharacterBody2D, ITeleportable
{
	[Export]
	public float speed { get; set; } = 300.0f;

	public void TeleportTo(Vector2 destination)
	{
		GlobalPosition = destination;
		Velocity = Vector2.Zero;
		GD.Print($"Player teleported to {destination}");
	}

	public override void _Ready()
	{
		AddToGroup("player");
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

		Velocity = velocity;
		MoveAndSlide();
	}

}
