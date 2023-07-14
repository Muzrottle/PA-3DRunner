using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Opponent : MonoBehaviour
{
    public NavMeshAgent OpponentAgent;
    public GameObject Target;
    public GameManager gameManager;

    Vector3 OpponentStartPos;
    public GameObject speedBoosterIcon;
    public int speedBoosterCounter;

    public GameObject carBody;
    public GameObject driver;

    public TrailRenderer leftTire;
    public TrailRenderer rightTire;
    public Material boostTrailMatV1;
    public Material boostTrailMatV2;
    public Material boostTrailMatV3;

    // Start is called before the first frame update
    void Start()
    {
        OpponentAgent = GetComponent<NavMeshAgent>();
        OpponentStartPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        speedBoosterIcon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        OpponentAgent.SetDestination(Target.transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            transform.position = OpponentStartPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpeedBoost"))
        {
            OpponentAgent.speed = OpponentAgent.speed + 3f;
            speedBoosterIcon.SetActive(true);
            speedBoosterCounter++;
            StartCoroutine(SlowAfterAWhileCoroutine());
        }
        else if (other.CompareTag("Finish"))
        {
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            boxCollider.enabled = false;
            other.GetComponent<FinishOrder>().SetPosition(GetComponent<Opponent>(), null , "Opponent");
            gameManager.setRank();
        }
        else if (other.gameObject.CompareTag("CheckPoint"))
        {
            OpponentStartPos = other.gameObject.transform.position;
        }
    }

    private IEnumerator SlowAfterAWhileCoroutine()
    {
        do
        {
            //If the booster is already enabled and booster V3 is not active then upgrade its visuals to V2.
            if (leftTire.enabled == true)
            {
                //Upgrade visuals to V3.
                if (speedBoosterCounter == 3)
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

        yield return new WaitForSeconds(2.0f);
        OpponentAgent.speed = OpponentAgent.speed - 3f;
        speedBoosterCounter--;

        if (speedBoosterCounter == 0)
        {
            leftTire.enabled = false;
            rightTire.enabled = false;
            leftTire.material = boostTrailMatV1;
            rightTire.material = boostTrailMatV1;
            speedBoosterIcon.SetActive(false);
        }
    }
}
