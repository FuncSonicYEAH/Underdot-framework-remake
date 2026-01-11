using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Arena : Node2D
{
	[Export] public double BorderWeight = 5.0;

	public virtual void DrawArena(Rid border_render_item, Rid border_culling_item,
		Rid mask_render_item, Rid mask_culling_item)
	{}

	public virtual List<Vector2> GetBorderShapes()
	{
		return new List<Vector2> {};
	}
}
