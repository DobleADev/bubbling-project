using UnityEngine;

[CreateAssetMenu(fileName = "newTimeScale", menuName = "Scriptable Object/Time Scale")]
public class ScriptableTimeScale : ScriptableObject
{
    public float Multiplier = 1;
    public void SetMultiplier(float value) 
    {
        Multiplier = value;
    }
}
