using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance { get; private set; }
    [SerializeField] ScriptableTimeScale[] timeScales;
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    void Update()
    {
        if (instance != this) return;

        float totalTimeScale = 1;

        for (int i = 0; i < instance.timeScales.Length; i++)
        {
            totalTimeScale *= instance.timeScales[i].Multiplier;
        }
        Time.timeScale = totalTimeScale;
    }
}
