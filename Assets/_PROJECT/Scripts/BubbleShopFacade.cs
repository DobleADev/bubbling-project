using System.Collections;
using System.Collections.Generic;
using DobleADev.Scriptables.Variables;
using DoubleADev.Scriptables.Events;
using UnityEngine;
using UnityEngine.UI;

public class BubbleShopFacade : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GridLayoutGroup _selectionGrid;
    [SerializeField] BubbleShopOption _optionPrefab;
    [SerializeField] Image _bubbleImage;
    [SerializeField] Text _bubbleName;
    [SerializeField] Text _bubbleDescription;
    [SerializeField] Text _bubbleCost;
    [SerializeField] GameObject _costGraphic;
    [SerializeField] GameObject _availableGraphic;
    [SerializeField] GameObject _equippedGraphic;
    [SerializeField] GameObject _insufficentFundsPopup;
    [SerializeField] GameObject _confirmBuyPopup;
    [Header("Data")]
    [SerializeField] IntScriptableVariable _currentBaterias;
    [SerializeField] BoolScriptableVariable _currentGameWinned;
    [SerializeField] BubbleData _nooneSelection;	
    [SerializeField] BubbleData _lockedSelection;	
    [SerializeField] BubbleData[] _bubbles;
    [SerializeField] GameObjectEvent _onBubbleEquipped;
    private int _lastSelectedIndex = -1;
    private int _equippedIndex = -1;
    private List<BubbleShopOption> _gridSelections = new List<BubbleShopOption>();
    // private List<GameObject> _gridSelections;

    public void Start()
    {
        UpdateDetailsData(_nooneSelection);
    }

    public void FetchOptions()
    {
        for (int i = 0; i < _bubbles.Length; i++)
        {
            var newOption = Instantiate(_optionPrefab, _selectionGrid.transform);
            var bubble = _bubbles[i];
            newOption.selection = i;
            newOption.image = bubble.image;
            newOption.gameObject.SetActive(true);
            
            if (bubble.requireWinGame)
            {
                if (!_currentGameWinned.GetValueTyped() && !bubble.acquired)
                {
                    newOption.Lock();
                }
                else
                {
                    bubble.acquired = true;
                }
            }
            _gridSelections.Add(newOption);    
        }

        BubbleShopOption firstSelection;
        if (_gridSelections.Count == 0)
        {
            return;
        }

        if (_equippedIndex == -1)
        {
            firstSelection = _gridSelections[0];
        }
        else
        {
            firstSelection = _gridSelections[_equippedIndex];
        }

        if (firstSelection.TryGetComponent(out Button button))
            {
                button.Select();
                EquipSelected();
            }
    }

    public void ClearOptions()
    {
        for (int i = 0; i < _gridSelections.Count; i++)
        {
            Destroy(_gridSelections[i].gameObject);
        }
        _gridSelections.Clear();
    }

    public void Select(int index)
    {
        var bubble = _bubbles[index];
        if (bubble == null)
        {
            UpdateDetailsData(_nooneSelection);
            return;
        }
        _lastSelectedIndex = index;

        if (bubble.requireWinGame && !bubble.acquired)
        {
            UpdateDetailsData(_lockedSelection);
        }
        else
        {
            UpdateDetailsData(bubble);
        }
    }


    public void EquipSelected()
    {
        var bubble = _bubbles[_lastSelectedIndex];
        if (bubble == null)
        {
            return;
        }

        if (!bubble.acquired)
        {
            if (bubble.requireWinGame)
            {
                return;
            }
            else if (bubble.cost > _currentBaterias.GetValueTyped())
            {
                _insufficentFundsPopup.SetActive(true);
                return;
            }
            else if (bubble.cost <= _currentBaterias.GetValueTyped())
            {
                _confirmBuyPopup.SetActive(true);
                return;
            }
        }

        if (_gridSelections.Count > 0)
        {
            if (_equippedIndex != -1)
            {
                _gridSelections[_equippedIndex].Dequip();
            }
            _gridSelections[_lastSelectedIndex].Equip();
        }
        _equippedIndex = _lastSelectedIndex;
        UpdateDetailsData(bubble);
        _onBubbleEquipped?.Invoke(bubble.prefab);
    }

    public void BuySelected()
    {
        var bubbleSelected = _bubbles[_lastSelectedIndex];
        _currentBaterias.SetValueTyped(_currentBaterias.GetValueTyped() - bubbleSelected.cost);
        bubbleSelected.acquired = true;
        Debug.Log(bubbleSelected.name + " bought successfuly");
        EquipSelected();
    }

    public void UpdateDetailsData(BubbleData bubbleData)
    {
        _bubbleImage.sprite = bubbleData.image;
        _bubbleName.text = bubbleData.name;
        _bubbleDescription.text = bubbleData.description;

        _costGraphic.SetActive(false);
        _availableGraphic.SetActive(false);
        _equippedGraphic.SetActive(false);

        if (_lastSelectedIndex == _equippedIndex && _equippedIndex != -1)
        {
            _equippedGraphic.SetActive(true);
        }
        else if (bubbleData.acquired)
        {
            _availableGraphic.SetActive(true);
        }
        else if (bubbleData.cost > 0)
        {
            _bubbleCost.text = bubbleData.cost.ToString();
            _costGraphic.SetActive(true);
        }
    }
}
