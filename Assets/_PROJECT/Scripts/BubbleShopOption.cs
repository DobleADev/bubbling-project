using DoubleADev.Scriptables.Events;
using UnityEngine;
using UnityEngine.UI;

public class BubbleShopOption : MonoBehaviour
{
    [SerializeField] Image _optionImage;
    [SerializeField] GameObject _equippedImage;
    [SerializeField] GameObject _lockedImage;
    [SerializeField] IntEvent _onSelected;
    public Sprite image { get { return _optionImage.sprite; } set { _optionImage.sprite = value; } }
    public int selection { get; set; }

    public void Lock()
    {
        _lockedImage.SetActive(true);
    }

    public void Unlock()
    {
        _lockedImage.SetActive(false);
    }

    public void Equip()
    {
        _equippedImage.SetActive(true);
    }

    public void Dequip()
    {
        _equippedImage.SetActive(false);
    }

    public void Select()
    {
        _onSelected?.Invoke(selection);
    }
}
