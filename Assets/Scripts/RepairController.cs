using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RepairController : MonoBehaviour
{
    [SerializeField] int amount;
    [SerializeField] GameObject amountTxt;

    void Start()
    {
        //boosters = boostersContainer.GetComponentsInChildren<GameObject>();
        //boosters = boosters.Where(child => child.tag == "SpeedBoost").ToArray();
        amountTxt.GetComponent<TextMeshPro>().text = amount.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            CheckCollisions checkCollisions = other.GetComponent<CheckCollisions>();
            HealthController healthController = other.GetComponent<HealthController>();

            if ( checkCollisions.coin >= amount && healthController.cartHPCount < 3)
            {
                healthController.carRepaired();
                checkCollisions.coin = checkCollisions.coin - amount;
                checkCollisions.coinText.text = checkCollisions.coin.ToString();
            }
        }
    }

}
