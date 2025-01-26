using System.Collections.Generic;
using DoubleADev.Scriptables.Events;
using UnityEngine;

public class IntRandomChooser : MonoBehaviour
{
    [SerializeField] bool _repeatable;
    [SerializeField] IntOption[] _reference;
    [SerializeField] IntEvent _onIntChoosed;
    List<IntOption> _ints = new List<IntOption>();
    [System.Serializable]
    public struct IntOption
    {
        public int Int;
    }

    public void Choose()
    {
        if (_ints.Count == 0) Reset();

        int selection = Random.Range(0, _ints.Count);
        var selectedInt = _ints[selection].Int;
        if (_repeatable == false)
        {
            _ints.RemoveAt(selection);
        }
        _onIntChoosed?.Invoke(selectedInt);
    }

    public void Reset()
    {
        _ints = new List<IntOption>(_reference);
    }
}
