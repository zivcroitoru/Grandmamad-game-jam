using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip healSFX;



    [Header("Clips")]
    public AudioClip roundBGM;
    public AudioClip gameOverSFX;
    public AudioClip pickupSFX;
    public AudioClip mamadBGM;

    void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        PlayRoundMusic();
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null || musicSource == null) return;
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayRoundMusic()
    {
        PlayMusic(roundBGM, true);
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    public void PlayGameOverSFX()
    {
        PlaySFX(gameOverSFX);
    }

    public void PlayPickupSFX()
    {
        PlaySFX(pickupSFX);
    }
    public void PlayHealSFX()
    {
        PlaySFX(healSFX);
    }
    public void PlayMamadMusic()
    {
        PlayMusic(mamadBGM, false);
    }
}
