using System;
using UnityEngine;

public class FootballerManager: MonoBehaviour
{
    private Animator footballerAnimator;
    
    private void Start()
    {
        EventManager.current.onDrawLine += PlayFootballerAnimation;
        footballerAnimator = transform.GetComponent<Animator>();
    }

    void PlayFootballerAnimation()
    {
        footballerAnimator.SetTrigger("Kick");
        Invoke("AnimationCompleted", 0.5f);
    }
    
    void AnimationCompleted()
    {
        EventManager.current.OnKickTheBall();
    }

    private void OnDestroy()
    {
        EventManager.current.onDrawLine -= PlayFootballerAnimation;
    }
}
