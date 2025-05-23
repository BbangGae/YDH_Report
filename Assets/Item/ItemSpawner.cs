using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public float spawnInterval = 10f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnItem), 0f, spawnInterval);
    }

    private void SpawnItem()
    {
        if (transform.childCount == 0)
            Instantiate(itemPrefab, transform.position, Quaternion.identity, transform);
    }
}
