using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;

[Tool]
[GlobalClass]
public partial class ArenaGroup : Node2D
{
	private Material CullingMateral;

	public Rid PhysicsBody;

	private StaticBody2D _collisionBoundary;

	public Rid BorderViewport;
	public Rid MaskViewport;

	public Rid BorderCanvas;
	public Rid MaskCanvas;

	public Rid BorderRenderItem;
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

		CullingMateral = ResourceLoader.Load<Material>("res://Sense/Arena/tran_material.tres");

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

		BorderRenderItem = RenderingServer.CanvasItemCreate();
		RenderingServer.CanvasItemSetParent(BorderRenderItem, BorderCanvas);
		BorderCullingItem = RenderingServer.CanvasItemCreate();
		RenderingServer.CanvasItemSetParent(BorderCullingItem, BorderCanvas);
		MaskRenderItem = RenderingServer.CanvasItemCreate();
		RenderingServer.CanvasItemSetParent(MaskRenderItem, MaskCanvas);
		MaskCullingItem = RenderingServer.CanvasItemCreate();
		RenderingServer.CanvasItemSetParent(MaskCullingItem, MaskCanvas);

		RenderingServer.CanvasItemSetMaterial(BorderCullingItem, CullingMateral.GetRid());
		RenderingServer.CanvasItemSetMaterial(MaskCullingItem, CullingMateral.GetRid());

		_collisionBoundary = new StaticBody2D();
		_collisionBoundary.Name = "ArenaCollisionBoundary";
		_collisionBoundary.CollisionLayer = 0;
		AddChild(_collisionBoundary);
	}

	public override void _Notification(int what)
	{
		base._Notification(what);
		if (what == NotificationPredelete)
		{
			RenderingServer.FreeRid(BorderRenderItem);
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
		List<Rid> CanvasItems = [ BorderRenderItem, BorderCullingItem, MaskRenderItem, MaskCullingItem ];
		
		foreach (Rid item in CanvasItems)
		{
			RenderingServer.CanvasItemClear(item);
		}

		foreach (Node Child in GetChildren())
		{
			Arena ArenaChild = Child as Arena;
			if (ArenaChild == null || !ArenaChild.Visible) continue;

			foreach (Rid item in CanvasItems)
			{
				RenderingServer.CanvasItemAddSetTransform(item, ArenaChild.GetTransform());
			}

			ArenaChild.DrawArena(CanvasItems[0],CanvasItems[1],CanvasItems[2],CanvasItems[3]);
		}

		Rid CanvasItem = GetCanvasItem();
		RenderingServer.CanvasItemClear(CanvasItem);
		RenderingServer.CanvasItemAddTextureRect(CanvasItem, GetViewportRect(), BorderViewportTexture );
		RenderingServer.CanvasItemAddTextureRect(CanvasItem, GetViewportRect(), MaskViewportTexture );
	}
}
