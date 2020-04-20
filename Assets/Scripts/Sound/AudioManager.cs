using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
    public List<Sound> sounds;

    public static AudioManager Instance { get; private set; }
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    //private void Update() {
    // sounds.ForEach(s => (s.source.volume != s.volume ? s.source.volume = s.volume : s.source.volume = s.volume));
    //}
    private void Start() {
        Play("Theme");
    }
    /// <summary>
    /// Play the sound clip
    /// </summary>
    /// <param name="name">Sound Clip name</param>
    /// <param name="onsehot">Play as one shot</param>
    /// <param name="randomPitch">Play with a random pitch</param>
    public void Play(string name, bool onsehot = false, bool randomPitch = false) {
        Sound s = sounds.Find(Sound => Sound.name == name);
        if (s == null)
            return;
        if (randomPitch) {
            s.pitch = UnityEngine.Random.Range(0.5f, 3);
            s.source.pitch = s.pitch;
        }
        if (onsehot)
            s.source.PlayOneShot(s.clip);
        else
            s.source.Play();
        
    }
    public bool IsPlaying(string name) {
        Sound s = sounds.Find(Sound => Sound.name == name);
        if (s == null)
            return false;
         
        return s.source.isPlaying;

    }
    /// <summary>
    /// Get the current volumen of the sound clip
    /// </summary>
    /// <param name="name">Sound Clip name</param>
    /// <returns></returns>
    internal float GetVolume(string name) {
        Sound s;
        if (name.Equals("Effects")) {
            s = sounds.Find(Sound => Sound.name.Equals("BrickHitted"));
        } else {
            s = sounds.Find(Sound => Sound.name == name);
        }
        if (s == null)
            return 0f;

        return s.source.volume;
    }
        internal bool Pause(string name) {
        Sound s;
        if (name.Equals("Effects")) {
            s = sounds.Find(Sound => Sound.name.Equals("BrickHitted"));
        } else {
            s = sounds.Find(Sound => Sound.name == name);
        }
        if (s == null)
            return false;
        s.source.Pause();
        return true;
    }
    /// <summary>
    /// Set a volume to the clip
    /// </summary>
    /// <param name="name">Sound Clip name</param>
    /// <param name="volume">Volume to set</param>
    public void ChangeVolumen(string name, float volume) {
        Sound s;
        if (name.Equals("Effects")) {
            s = sounds.Find(Sound => Sound.name.Equals("BrickHitted"));
        } else {
            s = sounds.Find(Sound => Sound.name == name);
        }
        if (s == null)
            return;

        s.volume = volume;
        s.source.volume = s.volume;
    }
}
