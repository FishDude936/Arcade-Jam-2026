using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("Variables")]
    [Header("Object References")]
    [SerializeField] AudioSource source;
    [SerializeField] List<AudioClip> sounds;
    [SerializeField] List<AudioClip> songs;
    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            source = GetComponent<AudioSource>();
        } else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        PlaySong("KnightFlight-BGM");
    }
    public void PlaySong(string songName)
    {
        AudioClip selectedSong = null;
        foreach (AudioClip song in songs)
        {
            if (song.name == songName)
            {
                selectedSong = song;
            }
        }
        if (selectedSong != null)
        {
            source.Stop();
            source.clip = selectedSong;
            source.Play();
        } else
        {
            source.Stop();
            Debug.Log($"Could not find song: {songName}");
        }
    }
    public void PlaySound(string soundName)
    {
        AudioClip selectedSound = null;
        foreach (AudioClip sound in sounds)
        {
            if (sound.name == soundName)
            {
                selectedSound = sound;
            }
        }
        if (selectedSound != null)
        {
            source.PlayOneShot(selectedSound);
            // GameObject soundObject = Instantiate(new GameObject(selectedSound.name, typeof(AudioSource)), transform);
            // AudioSource soundSource = soundObject.GetComponent<AudioSource>();
            // soundSource.clip = selectedSound;
            // soundSource.Play();
            // Destroy(soundObject, selectedSound.length);
        } else
        {
            Debug.Log($"Could not find sound: {soundName}");
        }
    }
}
