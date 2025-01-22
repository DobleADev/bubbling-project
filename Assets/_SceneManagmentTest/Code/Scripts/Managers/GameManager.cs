using System.Collections;
using System.Collections.Generic;
using DobleADev;
using DobleADev.BootEnemyTest;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Camera cameraPrefab;
    public GameObject player { get; private set; }
    public Camera mainCamera { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    [ContextMenu("_EnableGameplay")]
    public void _EnableGameplay() 
    {
        if (Instance.player != null)
        {
            Debug.LogError("Player already exists!");
            return;
        }
        Instance.player = Instantiate(playerPrefab);
        Instance.player.transform.position = transform.position;
        // if (playerSpawnPosition != null) player.transform.position = playerSpawnPosition.position;
        Instance.mainCamera = Instantiate(cameraPrefab);

        DontDestroyOnLoad(Instance.player);
        DontDestroyOnLoad(Instance.mainCamera);
    }

    [ContextMenu("_DisableGameplay")]
    public void _DisableGameplay() 
    {
        if (Instance.player == null)
        {
            Debug.LogError("Player doesnt exists!");
            return;
        }
        Destroy(Instance.player.gameObject);
        Destroy(Instance.mainCamera.gameObject);
    }

    public void _EnablePlayer()
    {
        if (Instance.player == null)
        {
            Debug.LogError("Player doesnt exists!");
            return;
        }
        Instance.player.gameObject.SetActive(true);
    }

    public void _WarpPlayerTo(Vector3 newPosition)
    {
        if (Instance.player == null)
        {
            Debug.LogError("Player doesnt exists!");
            return;
        }
        Instance.player.transform.position = newPosition;
    }

    // public void _WarpPlayerToSpawn()
    // {
    //     if (player == null)
    //     {
    //         Debug.LogError("Player doesnt exists!");
    //         return;
    //     }
    //     if (playerSpawnPosition == null)
    //     {
    //         Debug.LogError("spawn position doesnt exists!");
    //         return;
    //     }
    //     player.transform.position = playerSpawnPosition.position;
    // }

    public void _EndGameplay()
    {
        _DisableGameplay();
        Destroy(Instance.gameObject);
    }
}
