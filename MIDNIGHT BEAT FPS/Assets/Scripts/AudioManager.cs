using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;       // Singleton instance
    public AudioMixer audioMixer;             // Reference to the Audio Mixer
    public string exposedParameter = "MasterVolume"; // Name of the exposed parameter in the Audio Mixer
    private AudioSource audioSource;          // AudioSource for music

    [Header("Scene Settings")]
    public string[] stopMusicInScenes;        // List of scene names where music should stop

    void Awake()
    {
        // Ensure only one AudioManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);    // Persist across scenes
        }
        else
        {
            Destroy(gameObject);             // Destroy duplicates
        }
    }

    void Start()
    {
        // Get or add the AudioSource component
        audioSource = GetComponent<AudioSource>();

        if (!audioSource.isPlaying)
        {
            audioSource.Play();              // Start playing music if not already playing
        }

        // Subscribe to sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Unsubscribe from the event when the AudioManager is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the current scene is in the list of scenes to stop music
        foreach (string sceneName in stopMusicInScenes)
        {
            if (scene.name == sceneName)
            {
                StopMusic();
                return;
            }
        }
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();              // Stop the music
        }
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat(exposedParameter, volume);
    }

    public float GetVolume()
    {
        float value;
        if (audioMixer.GetFloat(exposedParameter, out value))
        {
            return value;
        }
        return 0f; // Default value if not found
    }
}
