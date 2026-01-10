using Godot;
using System;
using System.Collections.Generic;

public partial class SettingMain : VBoxContainer
{
	public int Choice = 0;
	private List<Color> Colors = new List<Color>
	{
		new Color(1,1,1), // White
		new Color(1,1,0)  // Yellow
	};

	public override void _Process(double delta)
	{
		base._Process(delta);
		SetLabelsColor();
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event.IsActionPressed("up"))
		{
			Choice--;
			if (Choice < 0)
			{
				Choice = 0;
			}
		}
		else if (@event.IsActionPressed("down"))
		{
			Choice++;
			if (Choice >= GetChildCount())
			{
				Choice = GetChildCount() - 1;
			}
		}

		if (@event.IsActionPressed("shift"))
		{
			GetParent<MenuSettings>().RequestChange("MainMenu");
		}
	}

	private void SetLabelsColor()
	{
		List<Label> Labels = new List<Label>();
		foreach (Node Child in GetChildren())
		{
			if (Child is Label label)
			{
				int Index = label.GetIndex();
				if (Index == Choice)
				{
					label.Modulate = Colors[1]; // Highlighted color
					label.ProcessMode = Node.ProcessModeEnum.Inherit; // Ensure processing is enabled
				}
				else
				{
					label.Modulate = Colors[0]; // Default color
					label.ProcessMode = Node.ProcessModeEnum.Disabled; // Disable processing
				}
			}
		}
	}
}
