using Godot;
using System;

public partial class PlayerCharacter : CharacterBody2D
{
	public const float Speed = 300.0f;

	public override void _PhysicsProcess(double delta)
	{
		LookAt(ToGlobal(ToLocal(GetGlobalMousePosition()).Rotated(0.5f * Mathf.Pi)));
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Velocity = direction * Speed;
		MoveAndSlide();
	}
}
