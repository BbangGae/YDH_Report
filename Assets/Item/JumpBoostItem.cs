using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Items/JumpBoostItem")]
public class JumpBoostItem : ItemBase
{
    public float jumpMultiplier = 2f;
    public float duration = 5f;

    public override void Activate(GameObject target)
    {
        PlayerMovement pm = target.GetComponent<PlayerMovement>();
        if (pm != null)
            pm.StartCoroutine(ApplyJumpBoost(pm));
    }

    private IEnumerator ApplyJumpBoost(PlayerMovement pm)
    {
        float originalJump = pm.jumpHeight;
        pm.jumpHeight *= jumpMultiplier;
        yield return new WaitForSeconds(duration);
        pm.jumpHeight = originalJump;
    }
}
