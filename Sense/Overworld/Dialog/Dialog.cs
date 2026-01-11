using Godot;
using System;
using System.Threading;
using System.Threading.Tasks;

public partial class Dialog : CanvasLayer
{
	TextTyper Typer;
	Control DialogContainer;

	public bool Started = false;

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

	private async void OnEnd()
	{
		DialogContainer.Hide();
		await ToSignal(GetTree().CreateTimer(0.0025), "timeout");
		Started = false;
	}

	public void DialogStart(string text)
	{
		if (!Started)
		{
			Started = true;
			Typer.StartTyping(text);
		}
	}

	private static Task ToSignal(GodotObject source, string signal)
    {
        var tcs = new TaskCompletionSource<bool>();
        Callable callable = Callable.From(() => tcs.SetResult(true));
        source.Connect(signal, callable);
        return tcs.Task;
    }
}
