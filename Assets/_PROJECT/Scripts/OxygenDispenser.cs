using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenDispenser : MonoBehaviour
{
    [SerializeField] Transform _spawnPosition;
    [SerializeField] float _spawnSeconds = 3f;
    [SerializeField, Range(0, 1)] float _feedbackAnticipation;
    [SerializeField] OxygenGain _oxygenPrefab;
    private OxygenGain _oxygenInside;
    [SerializeField] DropdownUnityEvent _onBeginSpawningProcess;
    [SerializeField] DropdownUnityEvent _onFeedbackAnticipation;
    [SerializeField] DropdownUnityEvent _onEndSpawningProcess;
    
    IEnumerator Start()
    {
        while (true)
        {
            if (_oxygenInside == null)
            {
                _onBeginSpawningProcess?.Invoke();
                float t = 0;
                float deltaDuration = 1 / _spawnSeconds;
                bool feedbackShowed = false;
                while (t < 1)
                {
                    if (t > _feedbackAnticipation && !feedbackShowed)
                    {
                        _onFeedbackAnticipation?.Invoke();
                        feedbackShowed = true;
                    }
                    t += Time.deltaTime * deltaDuration;
                    yield return null;
                }
                _oxygenInside = Instantiate(_oxygenPrefab, transform.parent);
                _oxygenInside.transform.position = _spawnPosition.position;
                _onEndSpawningProcess?.Invoke();
            }
            
            if (_oxygenInside.gameObject.activeSelf == false)
            {
                Destroy(_oxygenInside.gameObject);
            }
            yield return null;
        }
    }
}
