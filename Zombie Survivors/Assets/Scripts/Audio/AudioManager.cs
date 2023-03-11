using System;
using System.Collections;
using UnityEngine;

#region Sound Object
[Serializable]
public class Sound
{
	public AudioClip clip;
	public SoundTitle soundTitle;

	[Range(0f, 1f)]
	public float volume;

	public bool loop;

	[HideInInspector]
	public AudioSource source;
}
#endregion

public enum SoundTitle
{
	Gameplay_Theme,
	MainMenu_Theme,
	BossFight_Theme,
	Airdrop_Pickup,
	EXP_Pickup,
	Money_Pickup,
	Zombie_Hit,
	Helicopter_Arrive,
	Helicopter_Leave,
	Squad_Multiplier,
	Card_Select,
	Card_Upgrade,
	UI_Button_CLick,
	Zombie_Death,
	Soldier_Hurt,
	Gun_Shoot
}

//public enum AudioMixMode
//{
//	LinearAudioSourceVolume,
//	LinearMixerVolume,
//	LogrithmicMixerVolume
//}

public class AudioManager : MonoBehaviour
{
	//[Header("MIXER")]
	//[SerializeField] private AudioMixer audioMixer;
	//[SerializeField] private AudioMixMode mixMode;

	[Space(2)]
	[Header("SOUNDS")]
	public Sound[] sounds;

	public static AudioManager Instance;

	private AudioSource music1;
	private AudioSource music2;
	private AudioSource sfxSource;
	private bool firstSourceIsPlaying;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		music1 = gameObject.AddComponent<AudioSource>();
		music2 = gameObject.AddComponent<AudioSource>();
		sfxSource = gameObject.AddComponent<AudioSource>();
	}

	//public void OnSliderChanged(float value)
	//{
	//	switch (mixMode)
	//	{
	//		case AudioMixMode.LinearAudioSourceVolume:
	//			music1.volume = value;
	//			music2.volume = value;
	//			sfxSource.volume = value;
	//			break;

	//		case AudioMixMode.LinearMixerVolume:
	//			audioMixer.SetFloat("Volume", (-80 + value * 80));
	//			break;

	//		case AudioMixMode.LogrithmicMixerVolume:
	//			audioMixer.SetFloat("Volume", Mathf.Log10(value) * 20);
	//			//PlayerPrefs.SetFloat("GlobalVolume", value);
	//			//PlayerPrefs.Save();
	//			break;
	//	}
	//}

	public void PlayMusic(SoundTitle musicTitle)
	{
		Sound musicToPlay = Array.Find(sounds, sound => sound.soundTitle == musicTitle);

		if (musicToPlay == null)
		{
			print($"Sound titled [{musicTitle}] was not found");
			return;
		}

		AudioSource activeSound = (firstSourceIsPlaying) ? music1 : music2;

		activeSound.clip = musicToPlay.clip;
		activeSound.volume = musicToPlay.volume;
		activeSound.loop = musicToPlay.loop;
		activeSound.Play();
	}

	public void PlaySFX(SoundTitle sfxTitle)
	{
		Sound sfx = Array.Find(sounds, sound => sound.soundTitle == sfxTitle);

		sfxSource.PlayOneShot(sfx.clip, sfx.volume);
	}

	public void PlayMusicWithFade(SoundTitle songTitle, float transitionTime = 1.0f)
	{
		Sound newSong = Array.Find(sounds, sound => sound.soundTitle == songTitle);

		AudioSource activeSound = (firstSourceIsPlaying) ? music1 : music2;

		StartCoroutine(UpdateMusicWithFade(activeSound, newSong, transitionTime));
	}

	public void PlayMusicWithCrossFade(SoundTitle musicTtile, float transitionTime = 1.0f)
	{
		Sound musicToPlay = Array.Find(sounds, sound => sound.soundTitle == musicTtile);

		AudioSource activeSound = (firstSourceIsPlaying) ? music1 : music2;
		AudioSource newSound = (firstSourceIsPlaying) ? music2 : music1;

		firstSourceIsPlaying = !firstSourceIsPlaying;

		newSound.clip = musicToPlay.clip;
		newSound.volume = musicToPlay.volume;
		newSound.loop = musicToPlay.loop;
		newSound.Play();
		StartCoroutine(UpdateMusicWithCrossFade(activeSound, newSound, transitionTime));
	}

	private IEnumerator UpdateMusicWithCrossFade(AudioSource originalSorce, AudioSource newSource, float transitionTime)
	{
		float t = 0.0f;

		// Fade out
		for (t = 0; t < transitionTime; t += Time.deltaTime)
		{
			originalSorce.volume = (newSource.volume - (t / transitionTime));
			newSource.volume = (t / transitionTime);
			yield return null;
		}
		originalSorce.Stop();
	}

	private IEnumerator UpdateMusicWithFade(AudioSource activeSource, Sound newSound, float transitionTime)
	{
		// Make sure the source is active and playing
		if (!activeSource.isPlaying)
			activeSource.Play();

		float t = 0.0f;
		float startingVolume = activeSource.volume;
		float targetVolume = 0.0f;

		// Fade out the active sound
		while (t < transitionTime)
		{
			t += Time.deltaTime;
			activeSource.volume = Mathf.Lerp(startingVolume, targetVolume, t / transitionTime);
			yield return null;
		}

		// Stop the active sound and switch the clip
		activeSource.Stop();
		activeSource.clip = newSound.clip;
		activeSource.Play();

		t = 0.0f;
		startingVolume = 0.0f;
		targetVolume = newSound.volume;

		// Fade in the new sound
		while (t < transitionTime)
		{
			t += Time.deltaTime;
			activeSource.volume = Mathf.Lerp(startingVolume, targetVolume, t / transitionTime);
			yield return null;
		}

		activeSource.volume = newSound.volume;
	}


	public void SetMusicVolume(float volume)
	{
		music1.volume = volume;
		music2.volume = volume;
	}

	public void SetSFXVolume(float volume)
	{
		sfxSource.volume = volume;
	}
}
