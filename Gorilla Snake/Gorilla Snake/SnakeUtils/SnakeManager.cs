using Gorilla_Snake;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeManager : MonoBehaviour
{
    public GameObject GameStart;
    public GameObject GamePause;
    public GameObject SnakeParent;
    public GameObject SnakeHead;
    public List<GameObject> SpawnableFood = new List<GameObject>();
    public List<Transform> spawnPos = new List<Transform>();
    public static SnakeManager Main;
    public List<GameObject> foodObjects = new List<GameObject>();
    public static int currentRoundScore = 0;
    public static int allTimeHighsScore = 0;
    private bool Highscore;
    public int LastSpawnPos = -1;
    private Color OriginalColor;
    private Material BlackInstance;
    public Vector3 OriginalPos;
    public GameStates CurrentGameState = GameStates.StartScreen;

    public void Update()
    {
        if (Plugin.roundScore.text != "Score: " + currentRoundScore)
        {
            Plugin.roundScore.text = "Score: " + currentRoundScore;
        }

        if (Plugin.Highscore.text != "Highscore: " + allTimeHighsScore)
        {
            Plugin.Highscore.text = "Highscore: " + allTimeHighsScore;
        }


        if (CurrentGameState == GameStates.Started)
        {
            if (!Highscore && currentRoundScore > allTimeHighsScore)
            {
                Highscore = true;
              GameObject r1 =  GameObject.Instantiate(Plugin.EffectObjects[0], Plugin.ActiveArcade.transform);
              GameObject r2 =  GameObject.Instantiate(Plugin.EffectObjects[1], Plugin.ActiveArcade.transform);
                r1.SetActive(true);
                r2.SetActive(true);
                Debug.Log("Rockets Launched!");
            }
        }
    }
    public void Start()
    {
        BlackInstance = new Material(SnakeHead.GetComponent<Renderer>().material);
        SnakeHead.GetComponent<Renderer>().material = BlackInstance;
        OriginalPos = SnakeHead.transform.localPosition;
        OriginalColor = SnakeHead.GetComponent<Renderer>().material.color;
        Main = this;
        PlayerPrefs.SetInt("GSHS", 2);
        allTimeHighsScore = PlayerPrefs.GetInt("GSHS", 0);
    }

    public void AddScore(int score)
    {
        currentRoundScore++;
    }

    public void CheckHighscore(int Value)
    {
        if (Value > allTimeHighsScore)
        {
            allTimeHighsScore = Value;
       PlayerPrefs.SetInt("GSHS", allTimeHighsScore);
        }

    }

    public void SpawnFoodItem()
    {
        if (foodObjects.Count != 0)
        {
            for (int i = 0; i < foodObjects.Count; i++)
            {
                Destroy(foodObjects[i]);
            }

            foodObjects.Clear();

        }
        int randomPos = UnityEngine.Random.Range(0, spawnPos.Count);
        int randomFood = UnityEngine.Random.Range(0, SpawnableFood.Count);

        if (randomPos != LastSpawnPos)
        {
            LastSpawnPos = randomPos;
        GameObject selectedFood = SpawnableFood[randomFood];
        Transform selectedPos = spawnPos[randomPos];

        GameObject spwndfood = Instantiate(selectedFood, SnakeHead.transform.parent);
        foodObjects.Add(spwndfood);
        spwndfood.transform.localPosition = selectedPos.transform.localPosition;
        Vector3 NewPos = new Vector3(spwndfood.transform.localPosition.x, spwndfood.transform.localPosition.y, SnakeHead.transform.localPosition.z);
        spwndfood.transform.localPosition = NewPos;
        spwndfood.SetActive(true);
        spwndfood.name = spwndfood.name + " Food";
        }
        else
        {
            SpawnFoodItem();
            return;
        }
    }

    public void StartGame()
    {
        SnakeParent.SetActive(true);
        GameStart.SetActive(false);
        CurrentGameState = GameStates.Started;
        SpawnFoodItem();
    }

    public void ResetGame()
    {
        SnakeParent.SetActive(false);
        GameStart.SetActive(true);
        SnakeHead.transform.localPosition = OriginalPos;
        CurrentGameState = GameStates.StartScreen;
        foreach (GameObject obj in foodObjects)
        {
            Destroy(obj);
        }

        foodObjects.Clear();
        if (SnakeHead.GetComponent<snakeController>()._segments.Count > 1)
        {
            for (int i = 1; i < SnakeHead.GetComponent<snakeController>()._segments.Count; i++)
            {
                Destroy(SnakeHead.GetComponent<snakeController>()._segments[i].gameObject);
            }

            SnakeHead.GetComponent<snakeController>()._segments.Clear();
            SnakeHead.GetComponent<snakeController>()._segments.Add(SnakeHead.transform);
        }
        CheckHighscore(currentRoundScore);
        currentRoundScore = 0;
        LastSpawnPos = -1;
        Highscore = false;
    }

    public void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    public void PauseGame()
    {
        Color NewColor = SnakeHead.GetComponent<Renderer>().material.color;
        NewColor.a = 0.4f;

        for (int i = 0; i < SnakeHead.GetComponent<snakeController>()._segments.Count; i++)
        {
            SnakeHead.GetComponent<snakeController>()._segments[i].GetComponent<Renderer>().material.color = NewColor;
        }
        SnakeHead.GetComponent<snakeController>().enabled = false;
        GamePause.SetActive(true);
        CurrentGameState = GameStates.Paused;
    }

    public void UnPauseGame()
    {
        for (int i = 0; i < SnakeHead.GetComponent<snakeController>()._segments.Count; i++)
        {
            SnakeHead.GetComponent<snakeController>()._segments[i].GetComponent<Renderer>().material.color = OriginalColor;
        }
        SnakeHead.GetComponent<snakeController>().enabled = true;
        GamePause.SetActive(false);
        CurrentGameState= GameStates.Started;
    }
}

public enum GameStates
{
    StartScreen,
    Started,
    Paused
}