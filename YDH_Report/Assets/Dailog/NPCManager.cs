using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance;

    public List<NPCInteractionState> npcStates = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public NPCInteractionState GetState(string npcId)
    {
        var state = npcStates.Find(s => s.npcId == npcId);
        if (state == null)
        {
            state = new NPCInteractionState(npcId);
            npcStates.Add(state);
        }
        return state;
    }
}
