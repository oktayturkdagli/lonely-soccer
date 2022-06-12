using System;
using UnityEngine;
using System.Collections.Generic;
using Lean.Touch;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    [SerializeField] private LineManager lineManager;
    [SerializeField] private List<Vector3> destinationList;
    [SerializeField] private float animateDuration = 0.45f;
    [SerializeField] private ParticleSystem ballParticleEffect;
    private int destinationCounter = 0;
    public bool isGoal = false;

    private void Start()
    {
        EventManager.current.onKickTheBall += MoveTheBall;
    }

    private void MoveTheBall()
    {
        destinationList = lineManager.destinationList;
        if (destinationList.Count < 2)
        {
            StopTheBall();
            return;
        }
        transform.DORotate(new Vector3(1,0,0) * 180, animateDuration-0.2f).SetEase(Ease.Linear).SetRelative(true).SetUpdate(UpdateType.Fixed).SetLoops(-1, LoopType.Incremental);
        TakeAStep();
    }
    
    private void TakeAStep()
    {
        destinationCounter++;
        if (destinationCounter >= destinationList.Count)
        {
            StopTheBall();
            return;
        }
        
        Vector3 destinationVector = destinationList[destinationCounter];
        if (destinationCounter == 1)
            destinationVector.y = 1.8f;

        bool isThereJumpEffect = destinationCounter > 1 && destinationCounter < 4;
        if (isThereJumpEffect)
        {
            Vector3 midStop = (transform.position + destinationVector) / 2;
            midStop.y = destinationVector.y;
            transform.DOJump(midStop, 0.2f, 1, animateDuration/2).SetEase(Ease.Linear).SetUpdate(UpdateType.Fixed).OnComplete(() =>
            {
                ballParticleEffect.transform.position = midStop;
                ballParticleEffect.Play();
                transform.DOJump(destinationVector, 0.2f, 1, animateDuration/2).SetEase(Ease.Linear).SetUpdate(UpdateType.Fixed).OnComplete(TakeAStep);
            });
        }
        else
        {
            transform.DOMove(destinationVector, animateDuration).SetEase(Ease.Linear).SetUpdate(UpdateType.Fixed).OnComplete(TakeAStep);
        }
    }

    private void StopTheBall()
    {
        transform.DOKill();
        EventManager.current.OnStopTheBall();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dummy"))
        {
            other.transform.DOShakeRotation(2f, 10f, 30);
        }
        else if (other.gameObject.CompareTag("Goal"))
        {
            isGoal = true;
        }
    }

    private void OnDestroy()
    {
        EventManager.current.onKickTheBall -= MoveTheBall;
    }
    
}