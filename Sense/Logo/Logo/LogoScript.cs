using Godot;
using System;

public partial class LogoScript : MenuState
{
	private AnimationPlayer _animation;

	public override void Enter()
	{
		AudioManager.enter.PlaySfx(ResourceLoader.Load<AudioStream>("res://Audio/Sounds/other/logo.ogg"));
		_animation = GetNode<AnimationPlayer>("Animation");
		_animation.Play("tips");
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event.IsActionPressed("ui_accept"))
		{
			RequestChange("MainMenu");
		}
	}

}
