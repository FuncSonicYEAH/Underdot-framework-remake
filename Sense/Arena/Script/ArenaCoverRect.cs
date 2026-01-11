using Godot;
using System;

[Tool]
[GlobalClass]
public partial class ArenaCoverRect : ArenaCover
{
	[Export] Vector2 Size = new Vector2(100, 100);

	public override void DrawArena(Rid border_render_item, Rid border_culling_item, Rid mask_render_item, Rid mask_culling_item)
	{
		Vector2 BorderSize = Size + new Vector2((float)BorderWeight * 2, (float)BorderWeight * 2);
		Rect2 BorderRect = new Rect2(-BorderSize / 2, BorderSize);
		RenderingServer.CanvasItemAddRect(border_culling_item, BorderRect, Colors.White);

		Rect2 ContentRect = new Rect2(-Size / 2, Size);
		RenderingServer.CanvasItemAddRect(mask_culling_item, ContentRect, Colors.White);

		//GD.Print("Draw");
		//GD.Print(GetRecentPointInsideArena(GlobalPosition));
	}
}
