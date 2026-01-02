using Godot;
using System;

public partial class Setting : Node
{
	public static string DefaultLanguage = "en";
	public static readonly Vector2 BaseWindowSize = new Vector2(640, 480);

	private static string _currentLanguage = DefaultLanguage;
	private static float _windowScale = 1.0f;

	
	public static Setting Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;

		LoadUserSettings();

		CurrentLanguage = _currentLanguage;
		WindowScale = _windowScale; 
	}

	public static string CurrentLanguage
	{
		get => _currentLanguage;
		set
		{
			_currentLanguage = string.IsNullOrEmpty(value) ? DefaultLanguage : value;
			TranslationServer.SetLocale(_currentLanguage);
			SaveUserSettings();
		}
	}

	public static float WindowScale
	{
		get => _windowScale;
		set
		{
			_windowScale = Mathf.Clamp(value, 0.5f, 2.0f);
			if (Instance != null)
			{
				Instance.QueueRedrawWindow();
				SaveUserSettings();
			}
			else
			{
				GD.PushWarning("Setting.Instance is null. Window scale not applied yet.");
			}
		}
	}

	private void QueueRedrawWindow()
	{
		CallDeferred(nameof(ApplyWindowScaleAndCenter));
	}

	private void ApplyWindowScaleAndCenter()
	{
		var window = GetWindow();

		Vector2 newSize = BaseWindowSize * _windowScale;
		window.Size = new Vector2I((int)newSize.X, (int)newSize.Y);

		Vector2I screenSize = DisplayServer.ScreenGetSize();
		Vector2I pos = new Vector2I(
			(screenSize.X - window.Size.X) / 2,
			(screenSize.Y - window.Size.Y) / 2
		);
		window.Position = pos;
	}

	private const string SAVE_KEY_LANG = "user_lang";
	private const string SAVE_KEY_SCALE = "window_scale";

	public static void SaveUserSettings()
	{
		var config = new ConfigFile();
		config.SetValue("General", SAVE_KEY_LANG, _currentLanguage);
		config.SetValue("General", SAVE_KEY_SCALE, _windowScale);
		config.Save("user://settings.cfg");
	}

	public static void LoadUserSettings()
	{
		var config = new ConfigFile();
		Error err = config.Load("user://settings.cfg");
		if (err != Error.Ok)
		{
			GD.PrintRaw("No saved settings found. Using defaults.");
			_currentLanguage = DefaultLanguage;
			_windowScale = 1.0f;
			return;
		}

		_currentLanguage = config.GetValue("General", SAVE_KEY_LANG, DefaultLanguage).AsString();
		_windowScale = (float)config.GetValue("General", SAVE_KEY_SCALE, 1.0f).AsDouble();
	}
}
