using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	static AudioManager instance;

	/*[Header("__USE_SETTINGS__")]
	public bool isUsingSetting;*/

	public List<Audio> sounds;
	private List<Audio> pausedSounds;

	public static AudioManager GetInstance()
	{
		return instance;
	}

	void Awake()
	{
		//DontDestroyOnLoad(gameObject);

		if (instance != null && instance != this)
			Destroy(gameObject);

		instance = this;

		foreach (Audio s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();

			if (s.clips.Length > 1)
			{
				s.hasMultipleClips = true;
			}
			else if (s.clips.Length > 0)
			{
				s.source.clip = s.clips[0];
			}

			s.source.loop = s.loop;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.playOnAwake = s.playOnAwake;
		}
	}

	public void Play(string name)
	{
		//Audio s = Array.Find(sounds, sound => sound.name == name);
		Audio s = null;
		foreach (Audio audio in sounds)
		{
			if (audio.name == name)
			{
				s = audio;
				break;
			}
		}

		if (s == null)
		{
			Debug.LogError("Cannot find audio in list!");
			return;
		}

		if (s.hasMultipleClips)
		{
			int newIndex = UnityEngine.Random.Range(0, s.clips.Length);
			s.source.clip = s.clips[newIndex];
		}


		if (IsPlaying(name))
		{
			if (s.source.clip && s.curInstNb < s.maxInstNb)
			{
				s.curInstNb += 1;

				AudioSource instancedSource = gameObject.AddComponent<AudioSource>();

				instancedSource.clip = s.source.clip;

				instancedSource.volume = s.volume;
				instancedSource.pitch = s.pitch;

				instancedSource.Play();

				StartCoroutine(StartInstSourceDestroyCD(s.source.clip.length, s, instancedSource));
			}
		}
		else
		{
			s.source.Play();
		}
	}

	public void Stop(string name)
	{
		//Audio s = Array.Find(sounds, sound => sound.name == name);
		Audio s = null;
		foreach (Audio audio in sounds)
		{
			if (audio.name == name)
			{
				s = audio;
				break;
			}
		}
		if (s == null)
			return;

		s.source.Stop();
	}

	public void StopAll()
	{
		foreach (Audio s in sounds)
		{
			if (s == null)
				return;

			s.source.Stop();
		}
	}

	public void Pause(string name)
	{
		//Audio s = Array.Find(sounds, sound => sound.name == name);
		Audio s = null;
		foreach (Audio audio in sounds)
		{
			if (audio.name == name)
			{
				s = audio;
				break;
			}
		}

		if (s == null)
			return;

		s.source.Pause();
	}

	public void PauseAll()
	{
		if (pausedSounds.Count > 0)
		{
			pausedSounds.Clear();
		}

		foreach (Audio s in sounds)
		{
			if (s == null)
				return;

			pausedSounds.Add(s);

			s.source.Pause();
		}
	}

	public void PlayAllPaused()
	{
		if (pausedSounds.Count > 0)
		{
			foreach (Audio s in pausedSounds)
			{
				if (s == null)
					return;

				s.source.Play();
			}

			pausedSounds.Clear();
		}
	}

	public bool IsPlaying(string name)
	{
		//Audio s = Array.Find(sounds, sound => sound.name == name);
		Audio s = null;
		foreach (Audio audio in sounds)
		{
			if (audio.name == name)
			{
				s = audio;
				break;
			}
		}

		if (s == null)
			return false;

		return s.source.isPlaying;
	}

	public void ChangeClip(string name, AudioClip clip)
	{
		Audio s = null;
		foreach (Audio audio in sounds)
		{
			if (audio.name == name)
			{
				s = audio;
				break;
			}
		}

		if (s == null || clip == null || s.hasMultipleClips == true)
			return;

		s.source.clip = clip;
	}

	private IEnumerator StartInstSourceDestroyCD(float CD, Audio s, AudioSource instSource)
	{
		yield return new WaitForSeconds(CD);

		s.curInstNb += -1;
		Component.Destroy(instSource);

		StopCoroutine(StartInstSourceDestroyCD(CD, s, instSource));
	}
}
