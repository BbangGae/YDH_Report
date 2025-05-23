using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Source")]
    public AudioSource bgmSource;

   


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBGM(AudioClip clip, float volume = 1f, bool loop = true)
    {
        if (bgmSource == null) return;

        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return; // ?? ?? ??? ??

        bgmSource.clip = clip;
        bgmSource.volume = volume;
        bgmSource.loop = loop;
        bgmSource.Play();
    }



    public void StopBGM()
    {
        if (bgmSource != null)
            bgmSource.Stop();
    }
}
