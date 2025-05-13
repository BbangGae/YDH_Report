using UnityEngine;

public enum PowerupType { Healpack, Shotgun, STup }

public class PowerupItem : MonoBehaviour
{
    public PowerupType type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        TopDownShooter shooter = other.GetComponent<TopDownShooter>();
        if (shooter != null)
        {
            switch (type)
            {
                case PowerupType.Healpack:
                    shooter.HealPercent(0.3f);
                    break;
                case PowerupType.Shotgun:
                    shooter.ActivateShotgun(10f);
                    break;
                case PowerupType.STup:
                    shooter.ActivateStatBoost(10f);
                    break;
            }
        }

        Destroy(gameObject); // 아이템은 1회용
    }
}
