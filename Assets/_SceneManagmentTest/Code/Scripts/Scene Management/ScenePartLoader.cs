using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CheckMethod
{
    Distance,
    Trigger
}

public class ScenePartLoader : MonoBehaviour
{
    public string playerTag = "Player"; 
    public Transform player;
    public CheckMethod checkMethod;
    public float loadRange;

    //Scene state
    private bool isLoaded;
    private bool shouldLoad;

    void Start()
    {
        if (SceneManager.sceneCount > 0)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == gameObject.name)
                {
                    isLoaded = true;
                }
            }
        }
    }

    void Update()
    {
        switch (checkMethod)
        {
            case CheckMethod.Distance: DistanceCheck(); break;
            case CheckMethod.Trigger: TriggerCheck(); break;
        }
    }

    void DistanceCheck()
    {
        if (player == null)
        {
            Debug.Log("Player not assigned");
            return;
        }

        if (Vector3.Distance(player.position, transform.position) < loadRange)
        {
            LoadScene();
        }
        else 
        {
            UnLoadScene();
        }
    }

    private void LoadScene()
    {
        if (!isLoaded)
        {
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            isLoaded = true;
        }
    }


    private void UnLoadScene()
    {
        if (isLoaded)
        {
            SceneManager.UnloadSceneAsync(gameObject.name);
            isLoaded = false;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag(playerTag))
        {
            shouldLoad = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag(playerTag))
        {
            shouldLoad = false;
        }
    }

    void TriggerCheck()
    {
        if (shouldLoad)
        {
            LoadScene();
        }
        else
        {
            UnLoadScene();
        }
    }
}
