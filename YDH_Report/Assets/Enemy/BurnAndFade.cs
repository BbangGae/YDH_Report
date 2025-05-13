using UnityEngine;

public class BurnAndFade : MonoBehaviour
{
    public float duration = 1.5f;
    private float timer = 0f;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        float t = timer / duration;

        // 🔥 점점 작아짐
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);

        // 💨 점점 투명해짐
        if (sr != null)
        {
            Color c = sr.color;
            c.a = Mathf.Lerp(1f, 0f, t);
            sr.color = c;
        }

        // 🧹 일정 시간 후 제거
        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }
}
