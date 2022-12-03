using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ball;

    [SerializeField]
    Vector3 ballSpawnPosition;
    


    private float timePassed = 0.0f;

    private bool firstShot = true;
    void Start()
    {
        PlayerController.spawnNextBall = true;
    }

    
    void Update()
    {

        if (PlayerController.spawnNextBall)
        {
            timePassed += Time.deltaTime;
            if (firstShot || timePassed >= 1.0f)
            {
                firstShot = false;
                GameObject newBall = (GameObject)Instantiate(ball, ballSpawnPosition, Quaternion.identity);
                timePassed = 0;
            }
        }
    }
}
