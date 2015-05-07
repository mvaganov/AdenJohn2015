using UnityEngine;
using System.Collections;

/// <summary>For creating AudioSources, as temporary objects, at this transform.</summary>
public class PlaySound : MonoBehaviour {
	public AudioClip sound;

	[System.Serializable]
	public class Settings {
		[Tooltip("Will play when this button is pressed. Example: \"Fire1\", \"Fire2\", \"Jump\". If empty, sound will play on Start().")]
		public string buttonPress;

		[Tooltip("If marked true, multiple sounds will be able to play at once")]
		public bool allowDuplicateSounds = false;

		[Tooltip("If marked true, sounds loop infinitely after starting")]
		public bool loop = false;

		[Tooltip("Will keep the sound attached to this transform even if it moves. Useful for 3D sounds")]
		public bool attachToTransform = true;

		[Range(0, 1)]
		public float volume = 1;

		public bool HasTriggerButton() { return buttonPress != null && buttonPress.Length > 0; }

		public bool IsTriggerButtonPresed() { return HasTriggerButton() && Input.GetButtonDown(buttonPress); }
	}

	public Settings settings = new Settings();

	AudioSource currentSound = null;

	void Start() {
		// if there isn't a button that we're waiting for
		if(!settings.HasTriggerButton()) {
			Play();
		}
	}

	void Update() {
		// if the triggering button was pressed
		if(settings.IsTriggerButtonPresed()
		// (don't play again if duplicates are not allowed, and this sound has been started)
		&& !(!settings.allowDuplicateSounds && currentSound != null)) { 
			Play();
		}
		// if a sound is playing, allow volume to be changed from this script
		if(currentSound != null && currentSound.volume != settings.volume) {
			currentSound.volume = settings.volume;
		}
	}

	public AudioSource Play() {
		currentSound = Play(sound, transform, settings.loop, settings.volume);
		if(currentSound != null && settings.attachToTransform) {
			currentSound.transform.parent = transform;
		}
		return currentSound;
	}

	public static AudioSource Play(AudioClip ac, Transform emitter, bool loop, float volume) {
		if(ac == null) {
			print("No sound provided for "+emitter);
			return null;
		}
		GameObject go = new GameObject("sound: " + ac.name);
		AudioSource asrc = go.AddComponent<AudioSource>();
		asrc.loop = loop;
		asrc.clip = ac;
		asrc.volume = volume;
		asrc.Play();
		if(!loop) {
			go.AddComponent<WhenSoundFinished>().SetListener("destroy", () => { Destroy(go); });
		}
		if(emitter != null) {
			go.transform.position = emitter.transform.position;
			go.transform.parent = emitter.transform.parent;
		}
		return asrc;
	}
}

public class WhenSoundFinished : MonoBehaviour {
	AudioSource asrc;
	public delegate void WhenSoundFinishedDelegate();
	public WhenSoundFinishedDelegate whatToInvoke;
	[Tooltip("describes the code that will execute when the attached AudioSource is finished playing")]
	public string thenWhat;
	void Awake() {
		asrc = GetComponent<AudioSource>();
	}
	IEnumerator WaitTillSoundIsDone() {
		enabled = false;
		yield return new WaitForSeconds(asrc.clip.length);
		enabled = true;
	}
	void Update() {
		if(!asrc.isPlaying) {
			whatToInvoke();
		} else {
			StartCoroutine(WaitTillSoundIsDone());
		}
	}
	/// <param name="description">describes the code that will execute when the attached AudioSource is finished playing</param>
	/// <param name="whatToInvoke">what code to execute when the attached AudioSource is finished playing</param>
	/// <returns>itself, used for chaining commands</returns>
	public WhenSoundFinished SetListener(string description, WhenSoundFinishedDelegate whatToInvoke) {
		thenWhat = description;
		this.whatToInvoke = whatToInvoke;
		return this;
	}
}
