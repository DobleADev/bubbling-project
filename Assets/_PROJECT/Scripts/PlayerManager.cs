using DobleADev.Core;
using DobleADev.Scriptables.Variables;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] ForeignChildrenController _playerContainer;
    [SerializeField] GameObject _currentPlayerPrefab;
    [SerializeField] Vector3ScriptableVariable _initialPosition;
    public Transform player { get; private set; }
    public void StartPlayer()
    {
        player = _playerContainer.InstatiateChildren(_currentPlayerPrefab);
        player.position = _initialPosition.GetValueTyped();
    }

    public void EndPlayer()
    {
        _playerContainer.DestroyChildren(player);
        player = null;
    }

    public void SetPlayerPrefab(GameObject newPlayerPrefab)
    {
        _currentPlayerPrefab = newPlayerPrefab;
    }
}
