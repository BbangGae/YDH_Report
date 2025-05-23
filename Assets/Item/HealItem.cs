using UnityEngine;

[CreateAssetMenu(menuName = "Items/HealItem")]
public class HealItem : ItemBase
{
    public int healAmount = 20;

    public override void Activate(GameObject target)
    {
        PlayerHealth health = target.GetComponent<PlayerHealth>();
        if (health != null)
            health.Heal(healAmount);
    }
}
