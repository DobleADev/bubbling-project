using Assets.BubbleShooterGameToolkit.Scripts.Ads.Networks.PokiWrapper;
using UnityEngine;
using UnityEngine.Events;

public class PokiWrapperComponent : MonoBehaviour
{
    [SerializeField] UnityEvent _onCompleted;
    public void Init()
    {
        PokiWrapper.Init(_onCompleted.Invoke);
    }

    public void StartGameplay()
    {
        PokiWrapper.GameplayStart();
    }

    public void StopGameplay()
    {
        PokiWrapper.GameplayStop();
    }
}
