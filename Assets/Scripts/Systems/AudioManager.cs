using UnityEngine;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager Instance { get; private set; }
    private static AudioSource currentNarration;
    private GameObject ambiancePlayer;

    void Awake()
    {
        CreateSingleton();
        InitializeManager();
    }

    private void InitializeManager()
    {
        // Reset any state variables
        currentNarration = null;
    }

    private void Start()
    {
        PlayAmbiance();
    }

    private void PlayAmbiance()
    {
        Sound s = Instance.FindSound("ambiance");
        ambiancePlayer = new GameObject("ambiancePlayer");
        ambiancePlayer.transform.position = new Vector3(0, 0, 0);
        AudioSource audioSource = ambiancePlayer.AddComponent<AudioSource>();

        audioSource.clip = s.clip;
        audioSource.spatialBlend = 0f;
        audioSource.volume = 0.02f;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void CleanupAudio()
    {
        // Stop all coroutines
        StopAllCoroutines();
        
        // Destroy ambiance player if it exists
        if (ambiancePlayer != null)
            Destroy(ambiancePlayer);
            
        // Destroy current narration if it exists
        if (currentNarration != null)
            Destroy(currentNarration.gameObject);
            
        currentNarration = null;
    }

    public static void Play(string name, Vector3 position, bool global=false, float pitch=0f, float volume=0f, float minDistance=0f, bool isNarration = false)
    {
        Sound s = Instance.FindSound(name);
        if (s == null) {
            Debug.LogError("Sound not found: " + name);
            return;
        }
        Debug.Log("Playing sound: " + name);

        // Only stop previous narration if this is a new narration
        if (isNarration && currentNarration != null && currentNarration.isPlaying)
        {
            Destroy(currentNarration.gameObject);
        }

        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        
        if (pitch > 0f)
        {
            audioSource.pitch = pitch;
        }
        if (volume > 0f)
        {
            audioSource.volume = volume;
        }
        else
        {
            audioSource.volume = s.volume;
        }
        if (minDistance > 0)
        {
            audioSource.minDistance = minDistance;
        }

        audioSource.clip = s.clip;
        audioSource.spatialBlend = global ? 0f : 1f;
        audioSource.volume = s.volume;
        audioSource.Play();

        // Only store narration references
        if (isNarration)
        {
            currentNarration = audioSource;
        }

        // Destroy the game object after the clip finishes playing
        Instance.StartCoroutine(DestroyAudioSourceWhenFinished(audioSource));
    }

    private static IEnumerator DestroyAudioSourceWhenFinished(AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        if (audioSource != null)
        {
            Destroy(audioSource.gameObject);
        }
    }

    private Sound FindSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }
        return s;
    }

    void CreateSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
