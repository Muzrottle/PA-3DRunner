using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    public Transform cameraTarget;
    public float sSpeed = 10.0f;
    public Vector3 dist;
    public Vector3 rot;
    public Transform lookTarget;


    private void LateUpdate()
    {
        if (cameraTarget != null)
        {
            Vector3 dPos = cameraTarget.position + dist;
            Vector3 sPos = Vector3.Lerp(transform.position, dPos, sSpeed * Time.deltaTime);
            transform.position = sPos;
            
            Quaternion targetRotation = Quaternion.Euler(rot);
            Quaternion sRot = Quaternion.Lerp(transform.rotation, targetRotation, sSpeed * Time.deltaTime);
            transform.rotation = sRot;
        }

        if (lookTarget != null)
        {
            transform.LookAt(lookTarget.position);
        }
        
    }
}
