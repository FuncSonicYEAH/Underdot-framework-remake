using Godot;
using System;

public partial class Dialog : CanvasLayer
{
	TextTyper Typer;
	Control DialogContainer;

	public override void _Ready()
	{
		DialogContainer = GetNode<Control>("Control");
		Typer = GetNode<TextTyper>("Control/TextTyper");
		DialogContainer.Hide();

		Typer.TypingStart += OnTypingStarted;
		Typer.CheckedEnd += OnEnd;

		DialogStart("* Hello[pause][br]* It's good!!![pause][end]");
	}

	private void OnTypingStarted()
	{
		GD.Print("Typing start");
		DialogContainer.Show();
	}

	private void OnEnd()
	{
		DialogContainer.Hide();
	}

	public void DialogStart(string text)
	{
		Typer.StartTyping(text);
	}
}
