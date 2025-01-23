using System.Collections.Generic;
using UnityEngine;

public class PrefabRandomChooser : MonoBehaviour
{
    [SerializeField] bool _repeatable;
    [SerializeField] PrefabOption[] _reference;
    [SerializeField] GameObjectEvent _onPrefabChoosed;
    List<PrefabOption> _prefabs = new List<PrefabOption>();
    [System.Serializable]
    public struct PrefabOption
    {
        public GameObject prefab;
    }

    public void Choose()
    {
        if (_prefabs.Count == 0) Reset();

        int selection = Random.Range(0, _prefabs.Count);
        var selectedPrefab = _prefabs[selection].prefab;
        if (_repeatable == false)
        {
            _prefabs.RemoveAt(selection);
        }
        _onPrefabChoosed?.Invoke(selectedPrefab);
    }

    public void Reset()
    {
        _prefabs = new List<PrefabOption>(_reference);
    }
}
