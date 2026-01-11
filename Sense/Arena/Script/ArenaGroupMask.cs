using Godot;
using System;

[Tool]
[GlobalClass]
public partial class ArenaGroupMask : Node2D
{
    [Export] ArenaGroup _arenaGroup;

    public override void _Ready()
    {
        base._Ready();
        ClipChildren = CanvasItem.ClipChildrenMode.Only;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (_arenaGroup != null && _arenaGroup.MaskViewportTexture.IsValid)
        {
            //GD.Print(_arenaGroup.MaskViewportTexture.IsValid);
            GlobalTransform = _arenaGroup.GlobalTransform;
            Rid canvas = GetCanvasItem();
            RenderingServer.CanvasItemClear(canvas);
            RenderingServer.CanvasItemAddTextureRect(canvas, GetViewportRect(), _arenaGroup.MaskViewportTexture);
        }
    }
}