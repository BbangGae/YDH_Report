[System.Serializable]
public class NPCInteractionState
{
    public string npcId;
    public bool hasTalkedBefore;

    public NPCInteractionState(string id)
    {
        npcId = id;
        hasTalkedBefore = false;
    }
}
