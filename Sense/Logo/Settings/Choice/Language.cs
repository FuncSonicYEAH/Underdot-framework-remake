using Godot;
using System;

public partial class Language : Label
{
	private SettingMain Parent => GetParent<SettingMain>();
	
	private static readonly string[] SupportedLanguages = { "en", "zh_CN" };
	private static int currentLanguageIndex = 0;

	public override void _Ready()
	{
		base._Ready();
		// Initialize currentLanguageIndex based on Setting.CurrentLanguage
		GetNode<Label>("Value").Text = "< " + Tr("Language") + " >";
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (@event.IsActionPressed("left"))
			{
				currentLanguageIndex = (currentLanguageIndex - 1 + SupportedLanguages.Length) % SupportedLanguages.Length;
				ApplyLanguage();
				GetViewport().SetInputAsHandled();
			}
			else if (@event.IsActionPressed("right"))
			{
				currentLanguageIndex = (currentLanguageIndex + 1) % SupportedLanguages.Length;
				ApplyLanguage();
				GetViewport().SetInputAsHandled();
			}
		}
	}

	private void ApplyLanguage()
	{
		Setting.CurrentLanguage = SupportedLanguages[currentLanguageIndex];
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		GetNode<Label>("Value").Text = "< " + Tr("Language") + " >";
	}
}
