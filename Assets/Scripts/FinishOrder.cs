using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishOrder : MonoBehaviour
{
    public CamFollowPlayer camFollowPlayer;
    public GameObject audience;

    public GameObject[] positions;
    public int positionCounter = 0;

    public void SetPosition(Opponent opponent, Transform transform, string runnerType)
    {
        if (positionCounter == 0)
        {
            audience.SetActive(true);
        }

        if (runnerType == "Opponent")
        {
            opponent.Target = positions[positionCounter];
        }
        else if (runnerType == "Player")
        {
            transform.position = positions[positionCounter].transform.position;
        }

        positionCounter++;
    }
}
