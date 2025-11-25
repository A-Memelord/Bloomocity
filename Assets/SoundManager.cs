using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource soundEffectsSource;
    public AudioSource musicSource;

    public AudioClip[] soundEffects;

    public AudioClip[] musicAudio;

    void Start()
    {
        PlaySoundMusic();
    }

    void Update()
    {
        if (!musicSource.isPlaying)
        {
            PlaySoundMusic();
        }
    }

    public void PlaySoundMusic()
    {
        if (musicAudio.Length == 0) return;

        AudioClip clip = musicAudio[Random.Range(0, musicAudio.Length)];
        musicSource.clip = clip;
        musicSource.Play();
    }
}
