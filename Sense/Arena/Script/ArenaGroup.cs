using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;

[Tool]
[GlobalClass]
public partial class ArenaGroup : Node2D
{
	public Rid BorderViewport;
	public Rid MaskViewport;

	public Rid BorderCanvas;
	public Rid MaskCanvas;

	public Rid BroderRenderItem;
	public Rid BorderCullingItem;
	public Rid MaskRenderItem;
	public Rid MaskCullingItem;

	public Rid BorderViewportTexture;
	public Rid MaskViewportTexture;

	public override void _Ready()
	{
		base._Ready();

		BorderCanvas = RenderingServer.CanvasCreate();
		MaskCanvas = RenderingServer.CanvasCreate();

		BorderViewport = RenderingServer.ViewportCreate();
		RenderingServer.ViewportSetActive(BorderViewport, true);
		RenderingServer.ViewportSetTransparentBackground(BorderViewport, true);
		RenderingServer.ViewportSetClearMode(BorderViewport, RenderingServer.ViewportClearMode.Always);
		RenderingServer.ViewportSetUpdateMode(BorderViewport, RenderingServer.ViewportUpdateMode.Always);
		RenderingServer.ViewportAttachCanvas(BorderViewport, BorderCanvas);

		MaskViewport = RenderingServer.ViewportCreate();
		RenderingServer.ViewportSetActive(MaskViewport, true);
		RenderingServer.ViewportSetTransparentBackground(MaskViewport, true);
		RenderingServer.ViewportSetClearMode(MaskViewport, RenderingServer.ViewportClearMode.Always);
		RenderingServer.ViewportSetUpdateMode(MaskViewport, RenderingServer.ViewportUpdateMode.Always);
		RenderingServer.ViewportAttachCanvas(MaskViewport, MaskCanvas);

		BorderViewportTexture = RenderingServer.ViewportGetTexture(BorderViewport);
		MaskViewportTexture = RenderingServer.ViewportGetTexture(MaskViewport);

		BroderRenderItem = RenderingServer.CanvasItemCreate();
		RenderingServer.CanvasItemSetParent(BroderRenderItem, BorderCanvas);
		BorderCullingItem = RenderingServer.CanvasItemCreate();
		RenderingServer.CanvasItemSetParent(BorderCullingItem, BorderCanvas);
		MaskRenderItem = RenderingServer.CanvasItemCreate();
		RenderingServer.CanvasItemSetParent(MaskRenderItem, MaskCanvas);
		MaskCullingItem = RenderingServer.CanvasItemCreate();
		RenderingServer.CanvasItemSetParent(MaskCullingItem, MaskCanvas);
	}

	public override void _Notification(int what)
	{
		base._Notification(what);
		if (what == NotificationPredelete)
		{
			RenderingServer.FreeRid(BroderRenderItem);
			RenderingServer.FreeRid(BorderCullingItem);
			RenderingServer.FreeRid(MaskRenderItem);
			RenderingServer.FreeRid(MaskCullingItem);
			RenderingServer.FreeRid(BorderCanvas);
			RenderingServer.FreeRid(MaskCanvas);
			RenderingServer.FreeRid(BorderViewport);
			RenderingServer.FreeRid(MaskViewport);
		}
		else if (what == NotificationEnterTree)
		{
			if (BorderViewport.IsValid)
				BorderViewportTexture = RenderingServer.ViewportGetTexture(BorderViewport);
			if (MaskViewport.IsValid)
				MaskViewportTexture = RenderingServer.ViewportGetTexture(MaskViewport);
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		RenderingServer.ViewportSetSize(BorderViewport, (int)GetViewportRect().Size.X, (int)GetViewportRect().Size.Y);
		RenderingServer.ViewportSetSize(MaskViewport, (int)GetViewportRect().Size.X, (int)GetViewportRect().Size.Y);
		//GD.Print((int)GetViewportRect().Size.X, "," ,(int)GetViewportRect().Size.Y);
		DrawArenas();
	}

	public void DrawArenas()
	{
		Godot.Collections.Array CanvasItems = new Godot.Collections.Array() { BroderRenderItem, BorderCullingItem, MaskRenderItem, MaskCullingItem };
		
		foreach (Rid item in CanvasItems)
		{
			RenderingServer.CanvasItemClear(item);
		}

		foreach (Node Child in GetChildren())
		{
			if (Child is not ArenaExpand arena)
				continue;
			
			foreach (Rid item in CanvasItems)
			{
				RenderingServer.CanvasItemSetTransform(item, arena.GetGlobalTransform().AffineInverse());
			}

			Child.Callv("DrawArena", CanvasItems);
			
		}

		Rid CanvasItem = GetCanvasItem();
		RenderingServer.CanvasItemClear(CanvasItem);
		RenderingServer.CanvasItemAddTextureRect(CanvasItem, GetViewportRect(), BorderViewportTexture );
		RenderingServer.CanvasItemAddTextureRect(CanvasItem, GetViewportRect(), MaskViewportTexture );
	}

}
