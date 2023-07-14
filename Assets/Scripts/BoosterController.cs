using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BoosterController : MonoBehaviour
{
    [SerializeField] int amount;
    [SerializeField] GameObject amountTxt;
    public GameObject boostersContainer;
    public GameObject[] boosters;

    public CheckCollisions checkCollisions;

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
            if (checkCollisions.coin >= amount)
            {
                StartCoroutine(ActivateBoosters());
            }
        }
    }

    IEnumerator ActivateBoosters()
    {
        foreach (GameObject booster in boosters)
        {
            yield return new WaitForSeconds(0.2f);
            booster.SetActive(true);
        }

        checkCollisions.coin = checkCollisions.coin - amount;
        checkCollisions.coinText.text = checkCollisions.coin.ToString();
    }
}
