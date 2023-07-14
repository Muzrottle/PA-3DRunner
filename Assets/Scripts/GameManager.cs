using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public GameObject countDownText;
    public int timer = 3;

    private PlayerController playerController;
    private Opponent[] opponents;

    public static GameManager Instance;
    private InGameRanking ig;
    public int finishedPlayerCount = 0;

    RunnerColors runnerColors;

    private GameObject[] runner;
    List<RankingSystem> sortArray = new List<RankingSystem>();

    private void Awake()
    {
        Application.targetFrameRate = 60;

        Instance = this;
        runner = GameObject.FindGameObjectsWithTag("Runner");
        ig = FindObjectOfType<InGameRanking>();
        playerController = FindObjectOfType<PlayerController>();
        opponents = FindObjectsOfType<Opponent>();
        runnerColors = FindAnyObjectByType<RunnerColors>();
    }

    void Start()
    {
        int opponentCounter = runner.Length - 1;
        int randomColor;
        List<Material> carMaterialList = runnerColors.runnerCarMaterials.ToList();
        List<Material> driverMaterialList = runnerColors.runnerMaterials.ToList();

        for (int i = 0; i < runner.Length; i++)
        {
            sortArray.Add(runner[i].GetComponent<RankingSystem>());

            if (runner[i].name == "Player")
            {
                foreach (Renderer renderer in runner[i].GetComponent<PlayerController>().GetComponentsInChildren<Renderer>())
                {
                    if (renderer.GetType() != typeof(TrailRenderer))
                    {
                        if (renderer.material.name != "Tires (Instance)" && renderer.material.name != "Default-Particle (Instance)")
                        {
                            renderer.material = runnerColors.playerCarMaterial;
                        }
                    }
                }
                
                runner[i].GetComponent<PlayerController>().driver.GetComponent<Renderer>().material = runnerColors.playerMaterial;
            }
            else
            {
                randomColor = Random.Range(0,opponentCounter);

                //This part is didnt use foreach because we dont want to change parent's material because it changes opponent name
                //text color and we skip it.

                Renderer[] renderers = runner[i].GetComponent<Opponent>().GetComponentsInChildren<Renderer>();

                for (int j = 1; j < renderers.Length; j++)
                {
                    Renderer renderer = renderers[j];

                    if (renderer.GetType() != typeof(TrailRenderer))
                    {
                        if (renderer.material.name != "Tires (Instance)" && renderer.material.name != "Default-Particle (Instance)")
                        {
                            renderer.material = carMaterialList[randomColor];
                        }
                    }
                }
                //

                runner[i].GetComponent<Opponent>().driver.GetComponent<Renderer>().material = driverMaterialList[randomColor];

                driverMaterialList.Remove(driverMaterialList[randomColor]);
                carMaterialList.Remove(carMaterialList[randomColor]);

                opponentCounter--;
            }
        }

        countDownText.SetActive(true);
        StartCoroutine(CountDown(timer));
    }

    void Update()
    {
        CalculateRanking();
    }

    IEnumerator CountDown(int time)
    {
        countDownText.GetComponent<TextMeshProUGUI>().text = time.ToString();

        // Increase font size
        float elapsedTime = 0f;
        float duration = 2f;
        float initialFontSize = 0f;
        float targetFontSize = 280f;

        while (elapsedTime < duration)
        {
            elapsedTime += 0.1f;
            float t = elapsedTime / duration;
            countDownText.GetComponent<TextMeshProUGUI>().fontSize = Mathf.Lerp(initialFontSize, targetFontSize, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        // Decrease font size
        elapsedTime = 0f;
        duration = 2f;
        initialFontSize = 280f;
        targetFontSize = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += 0.1f;
            float t = elapsedTime / duration;
            countDownText.GetComponent<TextMeshProUGUI>().fontSize = Mathf.Lerp(initialFontSize, targetFontSize, t);
            yield return null;
        }

        time--;

        if (time == 0)
        {
            playerController.enabled = true;
            countDownText.SetActive(false);
            playerController.runningSpeed = 9f;

            foreach (Opponent opponent in opponents)
            {
                opponent.gameObject.GetComponent<NavMeshAgent>().speed = 9f;
            }
        }
        else
        {
            yield return StartCoroutine(CountDown(time));
        }
    }

    void CalculateRanking()
    {
        int runners = sortArray.Count();

        sortArray = sortArray.OrderBy(x => x.distance).ToList();
        for (int i = 0;i < runners;i++)
        {
            sortArray[i].rank = i+1;

            if (sortArray[i].name == "Player")
            {
                ig.placementBackground[i + finishedPlayerCount].color = new Color32(102,110,250,150);
            }
            else
            {
                ig.placementBackground[i + finishedPlayerCount].color = new Color32(255, 255, 255, 150);
            }
        }

        if (runners == 7)
        {
            ig.a = sortArray[6].name;
            ig.b = sortArray[5].name;
            ig.c = sortArray[4].name;
            ig.d = sortArray[3].name;
            ig.e = sortArray[2].name;
            ig.f = sortArray[1].name;
            ig.g = sortArray[0].name;
        }
        else if (runners == 6)
        {
            ig.a = sortArray[5].name;
            ig.b = sortArray[4].name;
            ig.c = sortArray[3].name;
            ig.d = sortArray[2].name;
            ig.e = sortArray[1].name;
            ig.f = sortArray[0].name;
        }
        else if (runners == 5)
        {
            ig.a = sortArray[4].name;
            ig.b = sortArray[3].name;
            ig.c = sortArray[2].name;
            ig.d = sortArray[1].name;
            ig.e = sortArray[0].name;
        }
        else if (runners == 4)
        {
            ig.a = sortArray[3].name;
            ig.b = sortArray[2].name;
            ig.c = sortArray[1].name;
            ig.d = sortArray[0].name;
        }
        else if (runners == 3)
        {
            ig.a = sortArray[2].name;
            ig.b = sortArray[1].name;
            ig.c = sortArray[0].name;
        }
        else if (runners == 2)
        {
            ig.a = sortArray[1].name;
            ig.b = sortArray[0].name;
        }
        else if (runners == 1)
        {
            ig.a = sortArray[0].name;
        }
    }

    public void setRank()
    {
        int runners = sortArray.Count();
        Debug.Log(runners);

        if (runners == 7)
        {
            ig.g = sortArray[0].name;
        }
        else if (runners == 6)
        {
            ig.f = sortArray[0].name;
        }
        else if (runners == 5)
        {
            ig.e = sortArray[0].name;
        }
        else if(runners == 4)
        {
            ig.d = sortArray[0].name;
        }
        else if(runners == 3)
        {
            ig.c = sortArray[0].name;
        }
        else if(runners == 2)
        {
            ig.b = sortArray[0].name;
        }
        else if (runners == 1)
        {
            ig.a = sortArray[0].name;
        }

        finishedPlayerCount++;

        sortArray.RemoveAt(0);
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
