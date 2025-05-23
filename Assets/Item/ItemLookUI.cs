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
    public GameObject interactPromptText; // "E 키로 획득" 안내 텍스트 오브젝트

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

                if (interactPromptText != null)
                    interactPromptText.SetActive(true); //  안내 표시

                return;
            }
        }

        // 감지된 아이템이 없으면 UI 초기화
        nameText.text = "";
        descText.text = "";

        if (interactPromptText != null)
            interactPromptText.SetActive(false); //  안내 숨김
    }
}
