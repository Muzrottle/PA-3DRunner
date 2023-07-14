using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunnerColors : MonoBehaviour
{
    public Material playerCarMaterial;
    public Material playerMaterial;
    public Material[] runnerCarMaterials;
    public Material[] runnerMaterials;


    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void startRace()
    {
        ColorChange[] colorChangeObjects = FindObjectsOfType<ColorChange>();
        runnerCarMaterials = new Material[colorChangeObjects.Length-1];
        runnerMaterials = new Material[colorChangeObjects.Length-1];
        int opponentColorCounter = 0;

        for (int i = 0; i < colorChangeObjects.Length; i++)
        {
            if (colorChangeObjects[i].isSelected)
            {
                playerCarMaterial = colorChangeObjects[i].carMaterial;
                playerMaterial = colorChangeObjects[i].driverMaterial;
            }
            else
            {
                runnerCarMaterials[opponentColorCounter] = colorChangeObjects[i].carMaterial;
                runnerMaterials[opponentColorCounter] = colorChangeObjects[i].driverMaterial;
                opponentColorCounter++;
            }
        }

        SceneManager.LoadScene("RaceScene");
    }
}
