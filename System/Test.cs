using Godot;
using System;

public partial class Test : Control
{
	public override void _Ready()
	{
		base._Ready();
		GetNode<Character>("/root/Character").Load(0);
		GD.Print(Character.CharacterName);
	}
}
