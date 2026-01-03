using Godot;
using System;

[Tool]
[GlobalClass]

public partial class ArenaExpandRectangle : ArenaExpand
{
    [Export]
    public Vector2 Size = new Vector2(100, 100);

    public override void DrawArena(Rid border_render_item, Rid border_culling_item, Rid mask_render_item, Rid mask_culling_item)
    {
        Rid borderRenderItem = border_render_item;
        Rid maskRenderItem = mask_render_item;

        Vector2 borderSize = new Vector2((float)BorderWeight, (float)BorderWeight);
        Rect2 borderRect = new Rect2(-(Size * 0.5f + borderSize), Size + borderSize * 2);
        RenderingServer.CanvasItemAddRect(borderRenderItem, borderRect, BorderColor);

        Rect2 contentRect = new Rect2(-Size * 0.5f, Size);
        RenderingServer.CanvasItemAddRect(maskRenderItem, contentRect, BackgroundColor);
    }

    public bool IsInsideInArena(Vector2 pos)
    {
        Rect2 contentRect = new Rect2(-Size * 0.5f, Size - Vector2.One * new Vector2((float)BorderWeight, (float)BorderWeight));
        return contentRect.HasPoint(pos);
    }

    public Vector2 GetRecentPointInArena(Vector2 pos)
    {
        Vector2 half = Size * 0.5f;
        Vector2 closestLocal = Vector2.Zero;
        closestLocal.X = Mathf.Clamp(pos.X, -half.X, half.X);
        closestLocal.Y = Mathf.Clamp(pos.Y, -half.Y, half.Y);
        return closestLocal;
    }
}