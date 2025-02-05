using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    [SerializeField] PlayMode _playMode;
    [SerializeField] Animation _animation;
    public void Play()
    {
        _animation.Play(_playMode);
    }

    public void Play(AnimationClip animationClip)
    {
        _animation.Play(animationClip.name, _playMode);
    }
    
}
