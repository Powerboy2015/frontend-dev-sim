using Godot;
using System;

public partial class PlayerMovement : CharacterBody2D
{
	[Export]
	public int playerSpeed = 200;
	public void getInput()
	{
		Vector2 inputDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		Velocity = inputDirection * playerSpeed;
	}

	public override void _PhysicsProcess(double delta)
	{
		getInput();
		MoveAndSlide();
	}


}
