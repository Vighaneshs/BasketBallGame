using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUpdater : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text highScoreText;
    void Update()
    {
        scoreText.text = "Score: " + PlayerController.score;
        highScoreText.text = "High Score: " + PlayerController.highScore;
    }
}
