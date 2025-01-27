using UnityEngine;

[CreateAssetMenu(fileName = "newBubble", menuName = "Scriptable Object/Bubble Data")]
public class BubbleData : ScriptableObject
{
    public GameObject prefab;
    public Sprite image;
    public string description;
    public int cost;
    public bool requireWinGame;
    public bool acquired;
}
