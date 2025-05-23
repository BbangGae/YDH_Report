using UnityEngine;

public class SoundTriggerZone : MonoBehaviour
{
    public AudioClip bgmClip;
    public float volume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM(bgmClip, volume);
        }
    }
}
