using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    public DialogueSentence[] sentences; // 배열로 관리
}

[System.Serializable]
public class DialogueSentence
{
    public string speakerName;  // 개별 문장의 화자 이름
    [TextArea(2, 5)]
    public string text;         // 실제 대사
}
