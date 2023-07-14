using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runningSpeed;
    public float xSpeed;
    public float limitX;

    public GameObject carBody;
    public GameObject driver;

    // Added new codes
    public Animator driverAnim;
    public Animator carAnim;
    public GameObject player;
    
    void Update()
    {
        SwipeCheck();
    }

    void SwipeCheck()
    {
        float newX = 0;
        float touchXDelta = 0;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            //Debug.Log(Input.GetTouch(0).deltaPosition.x / Screen.width);
            touchXDelta = Input.GetTouch(0).deltaPosition.x / Screen.width;
        }
        else if (Input.GetMouseButton(0))
        {
            touchXDelta = Input.GetAxis("Mouse X");
        }

        newX = player.transform.position.x + xSpeed * touchXDelta * Time.deltaTime;
        newX = Mathf.Clamp(newX, -limitX, limitX);

        Vector3 newPosition = new Vector3(newX, player.transform.position.y, player.transform.position.z + runningSpeed * Time.deltaTime);
        player.transform.position = newPosition;
    }

    public void FinishAnimation(bool WinState)
    {
        if (WinState) 
        {
            carAnim.enabled = true;
        }
        else
        {
            driverAnim.enabled = true;
        }

        driverAnim.SetBool("didWin", WinState);
    }

    //If you finished first this function will be called at the end of the car animation.
    public void winEntry()
    {
        carAnim.enabled = false;
        driverAnim.enabled = true;
    }
}
