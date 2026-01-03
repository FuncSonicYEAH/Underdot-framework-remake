using Godot;
using System;
using System.Collections.Generic;

public partial class Logo : Control
{
	Dictionary<string,MenuState> States = new Dictionary<string,MenuState>{ };
	MenuState CurrentState = null;

	public override void _Ready()
	{
		base._Ready();

		States["Logo"] = GetNode<MenuState>("MainLogo");
		States["MainMenu"] = GetNode<MenuState>("MainMenu");

		foreach (var state in States.Values)
		{
			state.Visible = false;
			state.ProcessMode = Node.ProcessModeEnum.Disabled;
		}

		GD.Print("States: ", States);
		ChangeState("MainMenu");

	}

	public void ChangeState(string name)
	{
		if (CurrentState != null)
		{
			CurrentState.Exit();
			CurrentState.Visible = false;
			CurrentState.ProcessMode = Node.ProcessModeEnum.Disabled;
		}

		if (States.TryGetValue(name, out MenuState newState))
		{
			CurrentState = newState;
			CurrentState.ProcessMode = Node.ProcessModeEnum.Inherit;
			CurrentState.Visible = true;
			CurrentState.Enter();
		}
		else
		{
			GD.PrintErr($"State '{name}' not found!");
		}
	}

}
