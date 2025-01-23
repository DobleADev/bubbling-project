using System.Collections;
using System.Collections.Generic;
using DobleADev.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] UnityEvent _onStartLoading;
    [SerializeField] UnityEvent _onEndLoading;
    // [SerializeField] bool _waitLoad;
    [System.Serializable] class FloatEvent : UnityEvent<float> { }
    [SerializeField] FloatEvent _onLoadProgress;
    public struct SceneLoadSetting
    {
        public SceneReference scene;
        public enum ProcessType { Load, UnLoad }
        public ProcessType setting;
        public SceneLoadSetting(SceneReference scene, ProcessType setting) : this()
        {
            this.scene = scene;
            this.setting = setting;
        }
    }
    Queue<SceneLoadSetting> sceneQueue = new Queue<SceneLoadSetting>();
    bool _isLoading;

    public void EnqueueSceneLoad(SceneReference scene)
    {
        sceneQueue.Enqueue(new SceneLoadSetting(scene, SceneLoadSetting.ProcessType.Load));
    }

    public void EnqueueSceneLoad(SceneContainer sceneToLoad)
    {
        if (sceneToLoad == null) return;
        for (int i = 0; i < sceneToLoad.content.Length; i++)
        {
            EnqueueSceneLoad(sceneToLoad.content[i]);
        }
    }

    public void EnqueueSceneUnload(SceneReference scene)
    {
        sceneQueue.Enqueue(new SceneLoadSetting(scene, SceneLoadSetting.ProcessType.UnLoad));
    }

    public void EnqueueSceneUnload(SceneContainer sceneToUnload)
    {
        if (sceneToUnload == null) return;
        for (int i = 0; i < sceneToUnload.content.Length; i++)
        {
            EnqueueSceneUnload(sceneToUnload.content[i]);
        }
    }

    public void StartProcess()
    {
        if (_isLoading)
        {
            Debug.LogWarning("FAIL - Another process running");
            return;
        }

        if (sceneQueue.Count == 0)
        {
            Debug.LogWarning("FAIL - Empty scene queue");
            return;
        }
        StartCoroutine(DoSceneProcess(new Queue<SceneLoadSetting>(sceneQueue)));
        sceneQueue.Clear();
    }

    public IEnumerator DoSceneProcess(Queue<SceneLoadSetting> currentQueue)
    {
        _isLoading = true;
        _onStartLoading?.Invoke();
        // int count = sceneQueue.Count;
        float normalizedCount = 1 / currentQueue.Count;

        while (currentQueue.Count > 0)
        {
            var nextScene = currentQueue.Dequeue();

            if (nextScene.scene.buildIndex == -1)
            {
                Debug.Log("Scene on queue not found, SKIPPED >>");
                continue;
            }

            AsyncOperation operation;
            if (nextScene.setting == SceneLoadSetting.ProcessType.Load)
            {
                // Debug.Log("Loading " + nextScene.scene.sceneName);
                if (SceneManager.GetSceneByName(nextScene.scene.sceneName).isLoaded) continue;
                operation = SceneManager.LoadSceneAsync(nextScene.scene.sceneName, LoadSceneMode.Additive);
            }
            else
            {
                // Debug.Log("Unloading " + nextScene.scene.sceneName);
                if (!SceneManager.GetSceneByName(nextScene.scene.sceneName).isLoaded) continue;
                operation = SceneManager.UnloadSceneAsync(nextScene.scene.sceneName);
            }

            while (!operation.isDone)
            {
                _onLoadProgress?.Invoke(operation.progress * normalizedCount * 100f);
                yield return null;
            }

        }
        _onLoadProgress?.Invoke(100f);
        yield return new WaitForEndOfFrame();

        _isLoading = false;
        _onEndLoading?.Invoke();
    }

}
