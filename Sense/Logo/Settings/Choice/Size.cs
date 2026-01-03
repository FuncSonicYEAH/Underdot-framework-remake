using Godot;
using System;

public partial class Size : Label
{
	private SettingMain Parent => GetParent<SettingMain>();

	public override void _Input(InputEvent @event)
	{
		if (Parent.Choice == 1)
		{
			base._Input(@event);
			if (@event.IsActionPressed("left"))
			{
				Setting.WindowScale -= 0.5f;
			}
			else if (@event.IsActionPressed("right"))
			{
				Setting.WindowScale += 0.5f;
			}
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		GetNode<Label>("Value").Text = "< x" + Setting.WindowScale.ToString() + " >";
	}
}
