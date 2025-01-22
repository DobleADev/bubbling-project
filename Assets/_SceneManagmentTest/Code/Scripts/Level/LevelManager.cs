using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] SceneContainer _area;
    [SerializeField] SceneContainer _room;
    public event Action<SceneContainer> onRoomChanged;
    public SceneContainer area { get => _area; set => _area = value; }
    public SceneContainer room { get => _room; set => onRoomChanged(_room = value); }

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

}
