using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceController : MonoBehaviour
{
    Animator audienceAnim;

    private void Start()
    {
        audienceAnim = GetComponent<Animator>();
    }

    public void RandomAnimation()
    {
        bool randomAnim;
        int randomNum;
        
        randomNum = Random.Range(0, 2);

        Debug.Log(randomNum);

        if (randomNum == 1)
        {
            randomAnim = true;
        }
        else
        {
            randomAnim = false;
        }
        
        audienceAnim.SetBool("randomAnim", randomAnim);
    }
}
