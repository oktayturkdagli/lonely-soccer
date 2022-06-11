using System;
using Lean.Touch;
using UnityEngine;
using System.Collections.Generic;

public class LineManager : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Camera cam;
    [SerializeField] Transform ball;
    [SerializeField] Transform[] dummies;
    [SerializeField] LayerMask maskLayer;
    List<Vector3> destinationList = new List<Vector3>();
    
    bool canDrawLine = true;

    public void OnDown(LeanFinger finger)
    {
        lineRenderer.enabled = true;
        canDrawLine = true;
    }
    
    public void OnUpdate(LeanFinger finger)
    {
        if (canDrawLine)
        {
            canDrawLine = false;
            DefineFirstNode(finger.GetWorldPosition(0, cam));
            DefineOtherNodes();
            DrawTheLine();
        }
    }
    
    public void OnUp(LeanFinger finger)
    {
        lineRenderer.enabled = false;
    }
    
    private void DefineFirstNode(Vector3 mousePosition)
    {
        destinationList.Clear();
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            Vector3 node = ball.position;
            Vector3 direction = new Vector3(hit.point.x, 1, 7.5f);
            destinationList.Add(node);
            destinationList.Add(direction);
        }
    }
    
    private void DefineOtherNodes()
    {
        if (destinationList == null || destinationList.Count < 2)
            return;

        for (int i = 1; i < destinationList.Count; i++)
        {
            RaycastHit hit = CheckHit(destinationList[i-1], destinationList[i]);
            Transform hitTransform = hit.transform;
            
            if (hitTransform != null)
            {
                Debug.Log(i);
                AddNode(hit.transform, hit);
            }
        }
    }
    
    RaycastHit CheckHit(Vector3 firstPoint, Vector3 direction)
    {
        RaycastHit hit;
        firstPoint.y = 1;
        direction.y = 1;
        Debug.DrawRay(firstPoint, direction, Color.red, 3f);
        if (Physics.Raycast(firstPoint, direction, out hit, 10f, maskLayer))
        {
            Debug.DrawRay(firstPoint, direction, Color.red);
            return hit;
        }
        return new RaycastHit();
    }

    private void AddNode(Transform hitTransform, RaycastHit hit)
    {
        if (hitTransform.gameObject.tag == "Reflector")
        {
            Vector3 reflect = Vector3.Reflect(destinationList[destinationList.Count - 1], hit.normal);
            Vector3 node = hit.point;
            destinationList.RemoveAt(destinationList.Count - 1);
            destinationList.Add(node);
            destinationList.Add(reflect);
        }
        else if (hitTransform.gameObject.tag == "Net")
        {
            Vector3 reflect = Vector3.Reflect(destinationList[destinationList.Count - 1], hit.normal);
            Vector3 node = hit.point;
            destinationList.RemoveAt(destinationList.Count - 1);
            destinationList.Add(node);
        }
    }

    void DrawTheLine()
    {
        if (destinationList == null || destinationList.Count < 2)
            return;
        
        lineRenderer.positionCount = 0;
        lineRenderer.positionCount = destinationList.Count;
        for (int i = 0; i < destinationList.Count; i++)
        {
            lineRenderer.SetPosition(i, destinationList[i]);
        }

        canDrawLine = true;
    }
}
