using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageFlashUI : MonoBehaviour
{
    public Image damageImage;
    public float flashDuration = 0.3f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.4f); // 연한 빨강

    private Coroutine flashCoroutine;

    public void Flash()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        damageImage.color = flashColor;

        float t = 0f;
        Color startColor = flashColor;
        Color endColor = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);

        while (t < flashDuration)
        {
            damageImage.color = Color.Lerp(startColor, endColor, t / flashDuration);
            t += Time.deltaTime;
            yield return null;
        }

        damageImage.color = endColor;
    }
}
