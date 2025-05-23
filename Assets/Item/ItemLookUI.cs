using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemLookUI : MonoBehaviour
{
    public Camera cam;
    public float maxDistance = 5f;
    public LayerMask itemLayer;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, itemLayer))
        {
            WorldItem item = hit.collider.GetComponent<WorldItem>();
            if (item != null && item.itemData != null)
            {
                nameText.text = item.itemData.itemName;
                descText.text = item.itemData.description;
                return;
            }
        }

        nameText.text = "";
        descText.text = "";
    }
}
