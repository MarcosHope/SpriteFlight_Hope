using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;
    public bool loop = false;

    [HideInInspector] public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] sounds;

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

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name) => FindSound(name)?.source.Play();

    public void Stop(string name) => FindSound(name)?.source.Stop();

    public void Pause(string name) => FindSound(name)?.source.Pause();

    private Sound FindSound(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
                return s;
        }
        Debug.LogWarning("AudioManager: sound \"" + name + "\" not found.");
        return null;
    }
}
