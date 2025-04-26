using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sfxSounds; // Array of Sound objects
    public AudioSource sfxSources; // Array of AudioSource objects

    // Singleton instance
    public static AudioManager instance;

    private void Awake()
    {
        // Check if instance already exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }

    public void Play(string name)
    {
        Sound sound = System.Array.Find(sfxSounds, s => s.name == name);
        if (sound != null)
        {            
            sfxSources.PlayOneShot(sound.clip);
        }
        else
        {
            Debug.LogWarning("Sound not found: " + name);
        }
    }
    
}
