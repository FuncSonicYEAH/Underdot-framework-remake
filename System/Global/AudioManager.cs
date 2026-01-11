using Godot;
using System;
using System.Collections.Generic;

public partial class AudioManager : Node
{
    public static AudioManager enter;

    private int _bgmBusIndex = -1;
    private int _sfxBusIndex = -1;

    private float _masterVolume = 1.0f;
    public float MasterVolume
    {
        get => _masterVolume;
        set
        {
            _masterVolume = Mathf.Clamp(value, 0.0f, 1.0f);
            _UpdateBusVolumes();
            _SaveSettings();
        }
    }

    private float _bgmVolume = 1.0f;
    public float BgmVolume
    {
        get => _bgmVolume;
        set
        {
            _bgmVolume = Mathf.Clamp(value, 0.0f, 1.0f);
            _UpdateBusVolumes();
            _SaveSettings();
        }
    }

    private float _sfxVolume = 1.0f;
    public float SfxVolume
    {
        get => _sfxVolume;
        set
        {
            _sfxVolume = Mathf.Clamp(value, 0.0f, 1.0f);
            _UpdateBusVolumes();
            _SaveSettings();
        }
    }

    private bool _muted = false;
    public bool Muted
    {
        get => _muted;
        set
        {
            _muted = value;
            _UpdateBusVolumes();
            _SaveSettings();
        }
    }

    private Dictionary<string, AudioStream> _preloadSfx = new();

    private AudioStreamPlayer _currentBgm = null;

    private const string CONFIG_PATH = "user://audio_settings.cfg";

    public override void _Ready()
    {
        enter = this;
        _LoadSettings();
        _SetupAudioBuses();
        _UpdateBusVolumes();
    }

    public override void _ExitTree()
    {
        _SaveSettings();
    }

    private void _SetupAudioBuses()
    {
        _bgmBusIndex = _GetBusIndex("BGM");
        _sfxBusIndex = _GetBusIndex("SFX");

        if (_bgmBusIndex == -1)
        {
            AudioServer.AddBus();
            _bgmBusIndex = AudioServer.BusCount - 1;
            AudioServer.SetBusName(_bgmBusIndex, "BGM");
            AudioServer.SetBusSend(_bgmBusIndex, "Master");
        }

        if (_sfxBusIndex == -1)
        {
            AudioServer.AddBus();
            _sfxBusIndex = AudioServer.BusCount - 1;
            AudioServer.SetBusName(_sfxBusIndex, "SFX");
            AudioServer.SetBusSend(_sfxBusIndex, "Master");
        }
    }

    private int _GetBusIndex(string name)
    {
        for (int i = 0; i < AudioServer.BusCount; i++)
        {
            if (AudioServer.GetBusName(i) == name)
                return i;
        }
        return -1;
    }

    private void _UpdateBusVolumes()
    {
        float volMult = _muted ? 0.0f : _masterVolume;

        if (_bgmBusIndex != -1)
        {
            float bgmVol = _bgmVolume * volMult;
            AudioServer.SetBusVolumeDb(_bgmBusIndex, Mathf.LinearToDb(bgmVol));
        }

        if (_sfxBusIndex != -1)
        {
            float sfxVol = _sfxVolume * volMult;
            AudioServer.SetBusVolumeDb(_sfxBusIndex, Mathf.LinearToDb(sfxVol));
        }
    }

    public void PlayBgm(AudioStream stream, float fadeInTime = 0.0f, bool autoLoop = true)
    {
        if (_currentBgm != null)
        {
            _currentBgm.Stop();
            _currentBgm.QueueFree();
            _currentBgm = null;
        }

        _currentBgm = new AudioStreamPlayer();
        AddChild(_currentBgm);
        _currentBgm.Stream = stream;
        _currentBgm.Bus = "BGM";
        _currentBgm.VolumeDb = 0.0f;

        if (autoLoop && stream is AudioStreamWav wavStream)
        {
            wavStream.LoopMode = AudioStreamWav.LoopModeEnum.Forward;
        }
       

        _currentBgm.Play();
    }

    public void StopBgm(float fadeOutTime = 0.0f)
    {
        if (_currentBgm == null) return;

        _currentBgm.Stop();
        _currentBgm.QueueFree();
        _currentBgm = null;
    }

    public void PlaySfx(AudioStream stream, string busName = "SFX")
    {
        var player = new AudioStreamPlayer();
        player.Stream = stream;
        player.Bus = busName;
        player.Connect("finished", Callable.From(() => player.QueueFree()));
        AddChild(player);
        player.Play();
    }

    public void PreloadSound(string name, string path)
    {
        var stream = ResourceLoader.Load<AudioStream>(path);
        if (stream != null)
        {
            _preloadSfx[name] = stream;
        }
        else
        {
            GD.PushWarning($"Failed to preload sound: {path}");
        }
    }

    public void PlaySfxPreloaded(string name)
    {
        if (_preloadSfx.TryGetValue(name, out var stream))
        {
            PlaySfx(stream);
        }
        else
        {
            GD.PushError($"Preloaded sound not found: {name}");
        }
    }

    private void _LoadSettings()
    {
        var config = new ConfigFile();
        var err = config.Load(CONFIG_PATH);

        if (err == Error.Ok)
        {
            _masterVolume = config.GetValue("Audio", "master_volume", 1.0f).AsSingle();
            _bgmVolume = config.GetValue("Audio", "bgm_volume", 1.0f).AsSingle();
            _sfxVolume = config.GetValue("Audio", "sfx_volume", 1.0f).AsSingle();
            //_muted = config.GetValue("Audio", "muted", false).AsBool();
        }
    }

    private void _SaveSettings()
    {
        var config = new ConfigFile();
        config.SetValue("Audio", "master_volume", _masterVolume);
        config.SetValue("Audio", "bgm_volume", _bgmVolume);
        config.SetValue("Audio", "sfx_volume", _sfxVolume);
        //config.SetValue("Audio", "muted", _muted);

        var err = config.Save(CONFIG_PATH);
        if (err != Error.Ok)
        {
            GD.PushWarning("Failed to save audio settings.");
        }
    }
}