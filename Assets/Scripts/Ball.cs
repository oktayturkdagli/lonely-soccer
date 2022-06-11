using System;
using UnityEngine;
using System.Collections.Generic;
using Lean.Touch;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    [SerializeField] private LineManager lineManager;
    [SerializeField] private List<Vector3> destinationList;
    [SerializeField] private float animateDuration = 0.5f;
    private int destinationCounter = 0;

    public void OnUp(LeanFinger finger)
    {
        destinationList = lineManager.destinationList;
        MoveBall();
    }

    private void MoveBall()
    {
        if (destinationList.Count < 2) 
            return;
        
        EventManager.current.OnShootBall();
        
        transform.DORotate(new Vector3(1,0,0) * 180, animateDuration-0.2f).SetEase(Ease.Linear).SetRelative(true).SetUpdate(UpdateType.Fixed).SetLoops(-1, LoopType.Incremental);
        TakeAStep();
    }
    
    private void TakeAStep()
    {
        destinationCounter++;
        if (destinationCounter >= destinationList.Count) 
            return;

        Vector3 destinationVector = destinationList[destinationCounter];
        if (destinationCounter == 1)
            destinationVector.y = 2;

        bool isThereJumpEffect = destinationCounter > 1 && destinationCounter < 4;
        if (isThereJumpEffect)
        {
            Vector3 midStop = (transform.position + destinationVector) / 2;
            midStop.y = destinationVector.y;
            transform.DOJump(midStop, 0.2f, 1, animateDuration/2).SetEase(Ease.InOutQuad).SetUpdate(UpdateType.Fixed).OnComplete(() =>
            {
                transform.DOJump(destinationVector, 0.2f, 1, animateDuration/2).SetEase(Ease.InOutQuad).SetUpdate(UpdateType.Fixed).OnComplete(TakeAStep);
            });
        }
        else
        {
            transform.DOMove(destinationVector, animateDuration).SetEase(Ease.InOutQuad).SetUpdate(UpdateType.Fixed).OnComplete(TakeAStep);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Net")
        {
            Debug.Log("Goal!");
        }
    }
}