using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class CheckCollisions : MonoBehaviour
{
    public int coin;
    public int speedBoosterCounter;
    [SerializeField] bool isBumped;
    public TextMeshProUGUI coinText;
    private Coroutine boostCoroutine;

    // Added new codes
    public PlayerController playerController;
    public HealthController healthController;
    public CamFollowPlayer camFollowPlayer;
    public GameManager gameManager;

    public GameObject player;
    Vector3 PlayerStartPos;
    public GameObject speedBoosterIcon;
    private InGameRanking ig;
    
    //For Finish Text
    public GameObject canvas;
    public GameObject showPlacement;
    public TextMeshProUGUI winState;
    public TextMeshProUGUI placement;
    
    //For Tire Trails
    public TrailRenderer leftTire;
    public TrailRenderer rightTire;
    public Material boostTrailMatV1;
    public Material boostTrailMatV2;
    public Material boostTrailMatV3;

    //For Magnet
    public GameObject magnet;
    public GameObject magnetCollider;

    private void Start()
    {
        PlayerStartPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        speedBoosterIcon.SetActive(false);
        ig = FindObjectOfType<InGameRanking>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            //Debug.Log("Coin collected!");
            AddCoin();
            //Destroy(other.gameObject);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Finish"))
        {
            PlayerFinished(other.GetComponent<FinishOrder>().positions[other.GetComponent<FinishOrder>().positionCounter]);
            Transform transform = player.GetComponent<Transform>();
            other.GetComponent<FinishOrder>().SetPosition(null, transform, "Player");

            if (ig.namesTxt[6].text == "Player")
            {
                Debug.Log("Congratz!");
                winState.text = "You Won";
            }
            else
            {
                Debug.Log("You Lose!");
                winState.text = "You Lost";
            }
        }
        else if (other.CompareTag("SpeedBoost"))
        {
            activateBoost();
        }
        else if (other.gameObject.CompareTag("BumperObs"))
        {
            isBumped = true;
            deactivateBoostTrail();
        }
        else if (other.gameObject.CompareTag("Magnet"))
        {
            other.gameObject.SetActive(false);
            magnet.SetActive(true);
            magnetCollider.SetActive(true);
            StartCoroutine(deactivateMagnet());
        }
        else if (other.gameObject.CompareTag("CheckPoint"))
        {
            PlayerStartPos = other.gameObject.transform.position;
        }
    }

    IEnumerator deactivateMagnet()
    {
        yield return new WaitForSeconds(5f);
        magnet.SetActive(false);
        magnetCollider.SetActive(false);
    }

    void PlayerFinished(GameObject positionPlace)
    {
        camFollowPlayer.cameraTarget = null;
        camFollowPlayer.lookTarget = null;

        gameManager.setRank();
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
        playerController.runningSpeed = 0f;

        if (gameManager.finishedPlayerCount == 1)
        {
            placement.text = "1st";
        }
        else if (gameManager.finishedPlayerCount == 2)
        {
            placement.text = "2nd";
        }
        else if (gameManager.finishedPlayerCount == 3)
        {
            placement.text = "3rd";
        }
        else
        {
            placement.text = gameManager.finishedPlayerCount.ToString() + "th";
        }

        StartCoroutine(enableRestart(placement.text, positionPlace));

        placement.text = placement.text + "  Place";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            //Debug.Log("Touched Obstacle!..");
            // Added new codes
            healthController.cartDamaged();

            if (healthController.cartHPCount != 0)
            {
                player.transform.position = PlayerStartPos;
            }
            else
            {
                winState.text = "You Lost";
                placement.text = "DNF";
                StartCoroutine(enableRestart(placement.text, null));
            }
        }
    }

    IEnumerator enableRestart(string placement, GameObject positionPlace)
    {
        playerController.enabled = false;
        showPlacement.GetComponent<TextMeshProUGUI>().text = placement;

        foreach (Transform child in canvas.transform)
            if (!child.gameObject.CompareTag("FinishText"))
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                showPlacement.SetActive(true);
                
                // Increase font size
                float elapsedTime = 0f;
                float duration = 2f;
                float initialFontSize = 0f;
                float targetFontSize = 280f;
                
                while (elapsedTime < duration)
                {
                    elapsedTime += 0.02f;
                    float t = elapsedTime / duration;
                    showPlacement.GetComponent<TextMeshProUGUI>().fontSize = Mathf.Lerp(initialFontSize, targetFontSize, t);
                    yield return null;
                }

                yield return new WaitForSeconds(1f);

                // Decrease font size
                elapsedTime = 0f;
                duration = 2f;
                initialFontSize = 280f;
                targetFontSize = 0f;
                
                while (elapsedTime < duration)
                {
                    elapsedTime += 0.02f;
                    float t = elapsedTime / duration;
                    showPlacement.GetComponent<TextMeshProUGUI>().fontSize = Mathf.Lerp(initialFontSize, targetFontSize, t);
                    yield return null;
                }
                
                showPlacement.SetActive(false);
                child.gameObject.SetActive(true);

                if (placement == "1st")
                {
                    playerController.FinishAnimation(true);
                }
                else
                {
                    playerController.FinishAnimation(false);
                }

                if (positionPlace != null)
                {
                    camFollowPlayer.cameraTarget = positionPlace.GetComponent<Transform>();
                    camFollowPlayer.dist = new Vector3(-0.8f, 1f, 1.5f);
                    camFollowPlayer.rot = new Vector3(15, 160, 0);
                }

                
            }
    }

    public void AddCoin()
    {
        coin++;
        coinText.text = coin.ToString();
    }

    void activateBoost()
    {
        playerController.runningSpeed = playerController.runningSpeed + 3f;
        speedBoosterIcon.SetActive(true);
        speedBoosterCounter++;

        do
        {
            //If the booster is already enabled and booster V3 is not active then upgrade its visuals to V2.
            if (leftTire.enabled == true)
            {
                //Upgrade visuals to V3.
                if (speedBoosterCounter >= 3)
                {
                    leftTire.material = boostTrailMatV3;
                    rightTire.material = boostTrailMatV3;
                    break;
                }

                leftTire.material = boostTrailMatV2;
                rightTire.material = boostTrailMatV2;
                break;
            }

            leftTire.enabled = true;
            rightTire.enabled = true;
            break;

        } while (false);

        boostCoroutine = StartCoroutine(SlowAfterAWhileCoroutine());
    }

    private IEnumerator SlowAfterAWhileCoroutine()
    {
        Debug.Log("Kontrol1");
        yield return new WaitForSeconds(2.0f);
        if (speedBoosterCounter != 0)
        {
            Debug.Log("Kontrol2");
            playerController.runningSpeed = playerController.runningSpeed - 3f;
            speedBoosterCounter--;
        }

        if (speedBoosterCounter == 0)
        {
            deactivateBoostTrail();
        }
    }

    void deactivateBoostTrail()
    {
        if (isBumped)
        {
            Debug.Log("Kontrol3");
            StopCoroutine(boostCoroutine);
            isBumped = false;
            playerController.runningSpeed = playerController.runningSpeed - speedBoosterCounter * 3f;
            speedBoosterCounter = 0;
        }

        leftTire.enabled = false;
        rightTire.enabled = false;
        leftTire.material = boostTrailMatV1;
        rightTire.material = boostTrailMatV1;
        speedBoosterIcon.SetActive(false);
    }
}
