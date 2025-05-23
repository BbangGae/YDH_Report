using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public float invincibleTime = 1.5f;
    public Slider healthSlider;

    private int currentHealth;
    private bool isInvincible = false;

    public DamageFlashUI damageFlashUI;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damage") && !isInvincible)
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (damageFlashUI != null)
            damageFlashUI.Flash();

        if (currentHealth <= 0)
            Dead();
        else
            StartCoroutine(InvincibilityCoroutine());
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = (float)currentHealth / maxHealth;
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"체력 회복됨: {amount} → 현재 체력: {currentHealth}/{maxHealth}");
        UpdateHealthUI();
    }

    void Dead()
    {
        Debug.Log("체력 없음");
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(1f);
        if (FadeManager.Instance != null)
            FadeManager.Instance.FadeToScene("GameOver");
        else
            SceneManager.LoadScene("GameOver");
    }
}
