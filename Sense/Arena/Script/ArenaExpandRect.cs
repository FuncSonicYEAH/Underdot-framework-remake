using Godot;
using System;
using System.Collections.Generic;

[Tool]
[GlobalClass]
public partial class ArenaExpandRect : ArenaExpand
{
	[Export] Vector2 Size = new Vector2(100, 100);

	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	public override void DrawArena(Rid border_render_item, Rid border_culling_item, Rid mask_render_item, Rid mask_culling_item)
	{
		Vector2 BorderSize = Size + new Vector2((float)BorderWeight * 2, (float)BorderWeight * 2);
		Rect2 BorderRect = new Rect2(-BorderSize / 2, BorderSize);
		RenderingServer.CanvasItemAddRect(border_render_item, BorderRect, BorderColor);

		Rect2 ContentRect = new Rect2(-Size / 2, Size);
		RenderingServer.CanvasItemAddRect(mask_render_item, ContentRect, BackgroundColor);

		//GD.Print("Draw");
		//GD.Print(GetRecentPointInsideArena(GlobalPosition));
	}

	public override bool IsInsideArena(Vector2 position)
	{
		Rect2 ContentRect = new Rect2(-Size / 2, Size);
		return ContentRect.HasPoint(position);
	}

	public override Vector2 GetRecentPointInsideArena(Vector2 position)
	{
		Rect2 ContentRect = new Rect2(-Size / 2, Size);
		float clampedX = Mathf.Clamp(position.X, ContentRect.Position.X, ContentRect.Position.X + ContentRect.Size.X);
		float clampedY = Mathf.Clamp(position.Y, ContentRect.Position.Y, ContentRect.Position.Y + ContentRect.Size.Y);
		return new Vector2(clampedX, clampedY);
	}
}
