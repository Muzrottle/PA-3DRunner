using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetController : MonoBehaviour
{
    public float pullForce = 10f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Debug.Log("Topladým.");
            Vector3 directionToMagnet = (transform.position - other.transform.position).normalized;
            other.transform.position += directionToMagnet * pullForce * Time.deltaTime;
        }
    }
}
