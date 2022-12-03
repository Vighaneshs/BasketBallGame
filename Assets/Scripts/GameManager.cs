using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ball;
    [SerializeField]
    private GameObject background;
    private GameObject nextBall;

    [SerializeField]
    private Button restartBtn;

    [SerializeField]
    Vector3 ballSpawnPosition;
    
    private float timePassed = 0.0f;

    [SerializeField]
    private float totalTime;
    private float totalTimeLeft;

    public TMP_Text timer;

    private Vector3 initBgPos;
    
    HighScore highScore = new HighScore();
    private bool firstShot;

    // 1 is for running game
    // 0 is for ended or not started    
    public static int gameState  = 0;


    public void initScene()
    {
        PlayerController.spawnNextBall = true;
        PlayerController.score = 0;
        restartBtn.gameObject.SetActive(false);
        totalTimeLeft = totalTime;
        background.transform.position = initBgPos;
        firstShot = true;
        gameState = 1;
    }

    void Start()
    {
        initBgPos = background.transform.position;
        initScene();
        try
        {
            var file = System.IO.File.OpenText(Application.persistentDataPath + "/highscore.json");
            string jsonText = file.ReadToEnd();
            HighScore previousHigh = JsonUtility.FromJson<HighScore>(jsonText);
            PlayerController.highScore = previousHigh.value;
        }
        catch
        {
            PlayerController.highScore = 0;
        }
    }

    
    void Update()
    {
        
        if(totalTimeLeft <= 0)
        {
            totalTimeLeft = 0.0f;
            timer.text = totalTimeLeft.ToString("0.00");
            if (nextBall != null)
            {
                Destroy(nextBall);
            }
            if (PlayerController.highScore > highScore.value)
            {
                highScore.value = PlayerController.score;
                SaveIntoJson();
            }
            restartBtn.onClick.AddListener(RestartGame);
            restartBtn.gameObject.SetActive(true);
            Vector3 bgFinal = initBgPos;
            bgFinal.z -= 10;
            background.transform.position = bgFinal;
            gameState = 0;
        }
        else
        {
            timer.text = totalTimeLeft.ToString("0.00");
            totalTimeLeft -= Time.deltaTime;
        }
        if (PlayerController.spawnNextBall)
        {
            timePassed += Time.deltaTime;
            if (firstShot || timePassed >= 1.0f)
            {
                firstShot = false;
                nextBall = (GameObject)Instantiate(ball, ballSpawnPosition, Quaternion.identity);
                timePassed = 0;
            }
        }
    }

    void RestartGame()
    {
        background.transform.position = initBgPos;
        totalTimeLeft = totalTime;
        PlayerController.score = 0;
        initScene();
    }

    public void SaveIntoJson()
    {
        string highScoreStr = JsonUtility.ToJson(highScore);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/highscore.json", highScoreStr);
    }

    

}
[System.Serializable]
public class HighScore
{
    public int value;
}