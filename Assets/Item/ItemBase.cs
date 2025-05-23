using UnityEngine;

public abstract class ItemBase : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;

    public abstract void Activate(GameObject target);
}
