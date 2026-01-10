using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public static class StringExtensions
{
    public static string GetSlice(this string str, char separator, int segment)
    {
        if (string.IsNullOrEmpty(str)) return "";
        string[] parts = str.Split(separator);
        return segment >= 0 && segment < parts.Length ? parts[segment] : "";
    }
}

[Tool]
[GlobalClass]
public partial class TextTyper : RichTextLabel
{

    [Export(PropertyHint.MultilineText)] public string TyperText= "";
    [Export] public float TypingSpeed = 0.075f; // Seconds per character

    private int ProgressIndex = 0;
    private float TimeAccumulator = 0f;
    private float WaitAccumulator = 0f;
    private bool IsTyping = true;
    private bool IsSkipping = false;
    private bool Paused = false;
    private bool CanSkip = true;

    public event Action TypingStart;
    public event Action TypingFinished;
    public event Action Destory;

    public override void _Ready()
    {
        base._Ready();
        BbcodeEnabled = true;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        TypingProcess(delta);
    }

    private void TypingProcess(double delta)
    {
        if (!IsTyping) return;

        if (Paused)
        {
            if (Input.IsActionJustPressed("enter"))
            {
                Paused = false;
            }
        }

        else if (WaitAccumulator > 0f)
        {
            WaitAccumulator -= (float)delta;
        }

        else
        {
            TypingStart?.Invoke();

            if (Input.IsActionJustPressed("shift") && !Paused && CanSkip)
            {
                int pauseIndex = TyperText.IndexOf("[pause]", ProgressIndex);
                if (pauseIndex >= 0)
                {
                    while (ProgressIndex < pauseIndex)
                    {
                        PrintText();
                    }
                }
                else
                {
                    while (ProgressIndex < TyperText.Length)
                    {
                        PrintText();
                    }
                }
            }

            /*
            else if (Input.IsActionJustReleased("shift") && !Paused)
            {
                Skipped = false;
            }
            */

            TimeAccumulator += (float)delta;

            while (TimeAccumulator >= TypingSpeed && ProgressIndex < TyperText.Length)
            {
                TimeAccumulator -= TypingSpeed;
                PrintText();
                GD.Print("Typing: " + ProgressIndex.ToString() + "/" + TyperText.Length.ToString());
            }

            if (ProgressIndex >= TyperText.Length)
            {
                TypingFinished?.Invoke();
                IsTyping = false;
                GD.Print("Typing complete.");
            }
        }
    }

    private void PrintText()
    {
        string text_char = TyperText[ProgressIndex].ToString();

        if (text_char == "[")
        {
            int tag_end = TyperText.IndexOf("]", ProgressIndex);
            if (tag_end != -1)
            {
                string tag_content = TyperText.Substring(ProgressIndex + 1, tag_end - ProgressIndex - 1);
                string keyword = tag_content.GetSlice('=', 0);
                string values = tag_content.Length > keyword.Length + 1 ? tag_content.Substring(keyword.Length + 1) : "";

                if (HandleBBCode(keyword, values))
                {
                    ProgressIndex = tag_end + 1;
                }
                else
                {
                    Text += "[" + tag_content + "]";
                    ProgressIndex = tag_end + 1;
                }
            }
            else
            {
                Text += text_char;
                ProgressIndex++;
            }
        }
        else
        {
            Text += text_char;
            ProgressIndex++;
        }
    }

    public bool HandleBBCode(string keyword, string values)
    {

        switch (keyword)
        {
            case "wait":
                if (float.TryParse(values, out float waitTime))
                {
                    WaitAccumulator = waitTime;
                    return true;
                }
                break;
            
            case "speed":
                if (float.TryParse(values, out float newSpeed))
                {
                    TypingSpeed = newSpeed;
                    return true;
                }
                break;

            case "canskip":
                if (bool.TryParse(values, out bool can))
                {
                    CanSkip = can;
                    return true;
                }
                break;
            
            case "/speed":
                TypingSpeed = 0.075f;
                return true;
            
            case "clear":
                Text = "";
                return true;
            
            case "pause":
                Paused = true;
                return true;
            
            case "end":
                ProgressIndex = TyperText.Length;
                return true;
            
            case "destroy":
                Destory?.Invoke();
                GD.Print("Is Destroyed");
                return true;
            
            default:
                break;
        }
        return false;
    }
}