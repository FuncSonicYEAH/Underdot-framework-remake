using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class ArenaExpand : Arena
{
	[Export] public Color BorderColor = Colors.White;
	[Export] public Color BackgroundColor = Colors.Black;

	public virtual bool IsInsideArena(Vector2 position)
	{
		return false;
	}
	
	public virtual Vector2 GetRecentPointInsideArena(Vector2 position)
	{
		return position;
	}
}
