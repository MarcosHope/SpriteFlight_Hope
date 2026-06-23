using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;
    public bool loop = false;

    [Range(0.1f, 3f)] public float speed = 1f;

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
            s.source.pitch = s.pitch * s.speed;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name) => FindSound(name)?.source.Play();

    public void Stop(string name) => FindSound(name)?.source.Stop();

    public void Pause(string name) => FindSound(name)?.source.Pause();

    // Plays the clip and, once it finishes, plays it again — repeats are clean with no overlap.
    public void PlayUntilDone(string name, int repeatCount = 1)
    {
        Sound s = FindSound(name);
        if (s != null)
            StartCoroutine(PlayRepeatCoroutine(s, repeatCount));
    }

    private System.Collections.IEnumerator PlayRepeatCoroutine(Sound s, int repeatCount)
    {
        for (int i = 0; i < repeatCount; i++)
        {
            s.source.Stop();
            s.source.Play();
            yield return new WaitForSeconds(s.source.clip.length / s.source.pitch);
        }
    }

    // Changes playback speed at runtime (also adjusts pitch to match).
    public void SetSpeed(string name, float speed)
    {
        Sound s = FindSound(name);
        if (s == null) return;
        s.speed = speed;
        s.source.pitch = s.pitch * s.speed;
    }

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
