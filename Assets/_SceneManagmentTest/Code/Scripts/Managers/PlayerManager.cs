using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance { get; private set; }
    public GameObject player { get; private set; }
    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void SetPlayerReference(GameObject playerReference)
    {
        instance.player = playerReference;
    }
}
