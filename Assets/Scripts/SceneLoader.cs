using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private TMP_Text highScore;
    private void Start()
    {
        try
        {
            var file = System.IO.File.OpenText(Application.persistentDataPath + "/highscore.json");
            string jsonText = file.ReadToEnd();
            HighScore previousHigh = JsonUtility.FromJson<HighScore>(jsonText);
            highScore.text = "High Score: " + previousHigh.value;
        }
        catch
        {
            highScore.text = "High Score: 0";
        }
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
