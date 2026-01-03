using Godot;
using System;
using System.Collections.Generic;

public partial class Logo : Control
{
	Dictionary<string,MenuState> States = new Dictionary<string,MenuState>{ };
	MenuState CurrentState = null;

	public event Action<string> StateChanged;

	public override void _Ready()
	{
		base._Ready();

		States["Logo"] = GetNode<MenuState>("MainLogo");
		States["MainMenu"] = GetNode<MenuState>("MainMenu");
		States["Settings"] = GetNode<MenuState>("Settings");

		foreach (var state in States.Values)
		{
			state.Visible = false;
			state.ChangeRequested += ChangeState;
			state.ProcessMode = Node.ProcessModeEnum.Disabled;
		}

		GD.Print("States: ", States);
		ChangeState("Logo");

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

			StateChanged?.Invoke(name);

			GD.Print("Changed state to: ", name);
		}
		else
		{
			GD.PrintErr($"State '{name}' not found!");
		}
	}

}
