using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendNumberToUIText : MonoBehaviour
{
    [SerializeField, Range(-1, 16)] int _decimalCount = -1;
    [SerializeField] Text _text;
    string decimalFormat { get { return _decimalCount == -1 ? "" : "N" + _decimalCount; } }
    public void SetText(int single)
    {
        UpdateText(single.ToString(decimalFormat));
    }
    public void SetText(float single)
    {
        UpdateText(single.ToString(decimalFormat));
    }
    public void SetText(Vector2 vector)
    {
        UpdateText(vector.ToString(decimalFormat));
    }
    public void SetText(Vector3 vector)
    {
        UpdateText(vector.ToString(decimalFormat));
    }
    void UpdateText(string text)
    {
        if (_text == null) return;
        _text.text = text;
    }
}
