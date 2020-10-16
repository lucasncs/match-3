using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
	[SerializeField] private AudioMixer _mixer;
	[SerializeField] private BooleanDataAsset _audioSetting;

	public bool Mute
	{
		get => !_audioSetting.Value;
		set => _audioSetting.Value = !value;
	}

	protected override void Awake()
	{
		base.Awake();
		AdjustVolume(_audioSetting.Value);
	}

	private void OnEnable()
	{
		_audioSetting.OnValueChange += AdjustVolume;
	}
	private void OnDisable()
	{
		_audioSetting.OnValueChange -= AdjustVolume;
	}

	private void AdjustVolume(bool _value)
	{
		_mixer.SetFloat("MasterVolume", _value ? 0f : -80f);
	}

	public void InvertAudioSetting()
	{
		Mute = !Mute;
	}
}
