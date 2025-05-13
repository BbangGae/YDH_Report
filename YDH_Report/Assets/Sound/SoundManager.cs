using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("SFX")]
    public AudioClip playerHitSFX; // ✅ 공통 플레이어 피격음
    public AudioClip enemyHitSFX;  // ✅ 공통 적 피격음

    [Header("BGM")]
    public AudioClip fieldBGM;
    public AudioClip miniGameBGM;

    public AudioSource sfxSource;
    public AudioSource bgmSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            bgmSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (fieldBGM != null)
        {
            PlayBGM(fieldBGM);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip != null && bgmSource.clip != clip)
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

  
public void SetMasterVolume(float value)
{
    AudioListener.volume = Mathf.Clamp01(value);
    Debug.Log($"🎚️ 마스터 볼륨 설정: {AudioListener.volume}");
}


public void SetBGMVolume(float value)
{
    if (bgmSource != null)
    {
        bgmSource.volume = Mathf.Clamp01(value);
        Debug.Log($"🎼 BGM 볼륨 설정: {bgmSource.volume}");
    }
}


public void SetSFXVolume(float value)
{
    if (sfxSource != null)
    {
        sfxSource.volume = Mathf.Clamp01(value);
        Debug.Log($"🎯 SFX 볼륨 설정: {sfxSource.volume}");
    }
}

}
