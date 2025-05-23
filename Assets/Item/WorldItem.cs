using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WorldItem : MonoBehaviour
{
    public ItemBase itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && itemData != null)
        {
            itemData.Activate(other.gameObject);
            Destroy(gameObject); // 사용 후 제거
        }
    }
}
