using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AirLauncherFacade : MonoBehaviour
{
    [SerializeField] float _impulseAmount = 1;
    [SerializeField] float _initialDelay = 0.6f;
    [SerializeField] bool _waitForInput = false;
    [SerializeField] Vector3 _attachOffset;
    [SerializeField] string _playerTag = "Player";
    [SerializeField] UnityEvent _onPlayerEnter;
    [SerializeField] UnityEvent _onPlayerLaunch;
    Rigidbody _playerInside;
    bool _canLaunch = false;

    IEnumerator WaitBeforeLaunch()
    {
        float t = 0;
        while (t < _initialDelay)
        {
            if (_playerInside == null) break;
            _playerInside.Sleep();
            t += Time.deltaTime;
            yield return null;
        }
        // yield return new WaitForSeconds(_initialDelay);
        _canLaunch = true;
    }

    private void Update() 
    {
        if (!_canLaunch) return;

        if (!_waitForInput)
        {
            Launch();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (!other.CompareTag(_playerTag) || _playerInside != null || !enabled)
        {
            return;
        }

        if (other.TryGetComponent(out Rigidbody player))
        {
            _playerInside = player;
            _playerInside.transform.position = transform.TransformPoint(_attachOffset);
            _playerInside.velocity = Vector3.zero;
            // _playerInside.gameObject.SetActive(false);
            StartCoroutine(WaitBeforeLaunch());
            _onPlayerEnter?.Invoke();
        }
    }

    public void Launch()
    {
        // _playerInside.gameObject.SetActive(true);
        if (_playerInside != null) 
        {
            _playerInside.AddForce(_impulseAmount * transform.right, ForceMode.Impulse);
            _playerInside = null;
        }
        _canLaunch = false;
        _onPlayerLaunch?.Invoke();
    }

}
