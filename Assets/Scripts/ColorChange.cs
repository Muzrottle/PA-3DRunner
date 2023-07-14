using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChange : MonoBehaviour
{
    public GameObject playerCar;
    public GameObject driver;
    public Material carMaterial;
    public Material driverMaterial;
    public GameObject colors;
    public bool isSelected;

    public void changeColor()
    {
        driver.GetComponent<Renderer>().material = driverMaterial;

        foreach (Renderer renderer in playerCar.GetComponentsInChildren<Renderer>())
        {
            Debug.Log(renderer.material.name);

            if (renderer.material.name != "Tires (Instance)" && renderer.material.name != "Default-Particle (Instance)")
            {
                renderer.material = carMaterial;
            }
        }

        foreach (ColorChange colorChange in colors.GetComponentsInChildren<ColorChange>())
        {
            if (colorChange.gameObject != this.gameObject)
            {
                colorChange.isSelected = false;
                colorChange.gameObject.GetComponent<Outline>().enabled = false;
            }
            else
            {
                colorChange.isSelected = true;
                gameObject.GetComponent<Outline>().enabled = true;
            }
        }

        isSelected = true;
        driver.GetComponentInParent<Animator>().SetTrigger("isColorChanged");
    }
}
