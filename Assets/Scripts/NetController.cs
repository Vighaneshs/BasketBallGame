using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetController : MonoBehaviour
{

    // Can make perfomance better by instantiating net from GameManager
    // such that it doesn't set position to initPos


    [SerializeField]
    private float hVelocity;
    [SerializeField]
    private float horizontalRange;

    private Vector3 initPos;
    private Vector3 velocity;
    void Start()
    {
        initPos = transform.position;
        velocity = hVelocity*Vector3.right;    
    }

    void FixedUpdate()
    {
        if(GameManager.gameState == 0)
        {
            transform.position = initPos;
        }
        if(transform.position.x >= horizontalRange)
        {
            velocity = -velocity;
        }
        else if(transform.position.x <= -horizontalRange)
        {
            velocity = -velocity;
        }
        transform.position += velocity * Time.deltaTime;
    }
}
