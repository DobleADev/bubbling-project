using UnityEngine;

public class GoToLevelOnTriggerEnter : MonoBehaviour
{
    [SerializeField] string _filterTag = "Player";
    [SerializeField] SceneContainer _level;
    private void OnTriggerEnter(Collider other) 
    {
        if (_level == null || !other.CompareTag(_filterTag)) return;
        LevelManager.instance.room = _level;
    }
}
