using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DobleADev.Core;
using UnityEngine;
using static SceneLoader;

public class TransitionController : MonoBehaviour
{
    [SerializeField] private CheckpointData _currentPointContainer;
    // [SerializeField] private CheckpointData _unloadPointContainer;
    [SerializeField] private SceneLoader _loader;
    [SerializeField] private DropdownUnityEvent _onTransitionStart;
    [SerializeField] private DropdownUnityEvent _onNextPointLoaded;
    [SerializeField] private DropdownUnityEvent _onTransitionEnd;
    bool _isTransitioning, _nextUnloadConfirmed;

    public void GoToPoint(CheckpointData nextPoint)
    {
        if (_isTransitioning)
        {
            Debug.LogWarning("");
            return;
        }
        StartCoroutine(StartTransition(nextPoint, false));
    }

    public void GoToRoom(SceneContainer nextRoom)
    {
        if (_isTransitioning)
        {
            Debug.LogWarning("");
            return;
        }
        var nextPoint = ScriptableObject.CreateInstance<CheckpointData>();
        nextPoint.SetDataFromOther(_currentPointContainer);
        nextPoint.room = nextRoom;
        StartCoroutine(StartTransition(nextPoint, true));
    }

    public void EndTransition()
    {
        _nextUnloadConfirmed = true;
    }

    IEnumerator StartTransition(CheckpointData nextPoint, bool roomsOnly)
    {
        _isTransitioning = true;
        _nextUnloadConfirmed = false;
        _onTransitionStart.Invoke();
        Debug.Log("STARTED");

        var unloadProcess = new Queue<SceneLoadSetting>();

        // Loop through unloadScenes to add to queue
        SceneReference[] unloadScenes = _currentPointContainer.room == null? null : _currentPointContainer.room.content;
        if (!roomsOnly && _currentPointContainer.area != null) 
        {
            if (unloadScenes != null) unloadScenes = unloadScenes.Concat(_currentPointContainer.area.content).ToArray();
            else unloadScenes = _currentPointContainer.area.content;
        }

        for (int i = 0; i < unloadScenes.Length; i++)
        {
            unloadProcess.Enqueue(new SceneLoadSetting(unloadScenes[i], SceneLoadSetting.ProcessType.UnLoad));
        }

        // Set currentScenes as nextScenes
        _currentPointContainer.SetDataFromOther(nextPoint);

        var loadProcess = new Queue<SceneLoadSetting>();

        // Loop through currentScenes to add to queue
        SceneReference[] loadScenes = _currentPointContainer.room == null? null : _currentPointContainer.room.content;
        if (!roomsOnly && _currentPointContainer.area != null) 
        {
            if (loadScenes != null) loadScenes = loadScenes.Concat(_currentPointContainer.area.content).ToArray();
            else loadScenes = _currentPointContainer.area.content;
        }

        for (int i = 0; i < loadScenes.Length; i++)
        {
            loadProcess.Enqueue(new SceneLoadSetting(loadScenes[i], SceneLoadSetting.ProcessType.Load));
        }


        yield return _loader.DoSceneProcess(loadProcess);
        _onNextPointLoaded.Invoke();

        Debug.Log("Loaded, now");
        yield return new WaitWhile(() => _nextUnloadConfirmed == false);

        yield return _loader.DoSceneProcess(unloadProcess);
        Debug.Log("FINISHED");

        _isTransitioning = false;
        _onTransitionEnd.Invoke();
    }

}
