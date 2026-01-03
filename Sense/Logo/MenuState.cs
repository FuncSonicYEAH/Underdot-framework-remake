using Godot;
using System;

[GlobalClass]
public partial class MenuState : Control
{
	public event Action<string> ChangeRequested;

    protected void RequestChange(string stateName)
    {
        ChangeRequested?.Invoke(stateName);
    }

	public virtual void Enter(){}
	public virtual void Exit(){}
}
