using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthController : MonoBehaviour
{
    public PlayerController playerController;
    
    //For Cart HP
    public GameObject car;
    public GameObject driver;
    public int cartHPCount = 3;
    public Image cartHP1;
    public Image cartHP2;
    public Image cartHP3;
    public Sprite cartLife;
    public Sprite cartDead;
    public TrailRenderer cartDamagedTrail;

    public void cartDamaged()
    {
        cartHPCount--;

        if (cartHPCount == 2)
        {
            cartHP3.overrideSprite = cartDead;
        }
        else if (cartHPCount == 1)
        {
            cartHP2.overrideSprite = cartDead;
            cartDamagedTrail.enabled = true;
        }
        else
        {
            cartHP1.overrideSprite = cartDead;
        }

        if (cartHPCount == 0)
        {
            playerController.runningSpeed = 0;

            GetComponent<BoxCollider>().enabled = false;

            driver.AddComponent<Rigidbody>();

            CapsuleCollider driverCollider = driver.AddComponent<CapsuleCollider>();
            driverCollider.radius = 2;
            driverCollider.height = 10;
            driverCollider.center = new Vector3(0, 5, 0);

            // Get all child objects within the parent object
            Transform[] childObjects = car.GetComponentsInChildren<Transform>();

            // Iterate through each child object
            foreach (Transform child in childObjects)
            {
                if (child != transform) // Exclude the parent object itself
                {
                    // Add a Rigidbody component if it doesn't already exist
                    if (child.GetComponent<Rigidbody>() == null)
                    {
                        Rigidbody childRigidbody = child.gameObject.AddComponent<Rigidbody>();
                        child.gameObject.AddComponent<BoxCollider>();
                        // Set any additional properties for the rigidbody as needed
                        childRigidbody.mass = 1f;
                        childRigidbody.drag = 0.5f;
                        childRigidbody.angularDrag = 0.5f;
                    }
                }
            }
        }
    }

    public void carRepaired()
    {
        cartHPCount++;

        if (cartHPCount == 2)
        {
            cartHP2.overrideSprite = cartLife;
            cartDamagedTrail.enabled = false;
        }
        else if (cartHPCount == 3)
        {
            cartHP3.overrideSprite = cartLife;
        }
    }
}
