using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerDamageFlash : MonoBehaviour
{
    public Image flashImage;
    public float flashDuration = 0.2f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.5f);

    private Coroutine flashRoutine;

    public void TriggerFlash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        flashImage.color = flashColor;

        float timer = 0f;
        while (timer < flashDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(flashColor.a, 0f, timer / flashDuration);
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }

        flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);
        flashRoutine = null;
    }
}
