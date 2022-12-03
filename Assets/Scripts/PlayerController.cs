using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{


    [SerializeField]
    private GameObject basket;
    private Camera mainCamera;
    private SphereCollider ballCollider;

    [SerializeField]
    private Vector3 initVelocity;
    [SerializeField]
    private Vector3 gravityAccel;
    private Vector3 lastTouchPoint;

    private TouchControls touchControls;
    
    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    private bool isBallTouched = false;
    private bool ballShot = false;
    private bool touchStarted = false;
    private bool scored = false;
    public static bool spawnNextBall;

    public static int score = 0;
    
    public static int highScore = 0;

    [SerializeField]
    private float projectileTime;
    [SerializeField]
    private float boardBounceFactor;
    private float distSwiped;
    private float swipeStartTime;
    private float swipeEndTime;
    


    private void Awake()
    {
        touchControls = new TouchControls();
        mainCamera = Camera.main;
        ballCollider = GetComponent<SphereCollider>();
    }
    void Start()
    {
        spawnNextBall = false;
        touchControls.Touches.PrimaryContact.started += isTouched;
        touchControls.Touches.PrimaryContact.canceled += notTouched;
    }

    private void OnEnable()
    {
        touchControls.Enable();
    }
    private void OnDisable()
    {
        touchControls.Disable();
    }
    private void isTouched(InputAction.CallbackContext context)
    {
        swipeStartTime = Time.time;
        touchStarted = true;
        StartCoroutine(UpdatePosition(context));
    }
    private void notTouched(InputAction.CallbackContext context)
    {
        touchStarted = false;
        if (!ballShot && isBallTouched)
        {
            swipeEndTime = Time.time;
            spawnNextBall = true;
            distSwiped = Vector3.Distance(mainCamera.WorldToScreenPoint(transform.position), lastTouchPoint);
            initVelocity.y = distSwiped*(3 - 3*(swipeEndTime - swipeStartTime) ) / 30;
            ballShot = true;
            timePassed = 0.0f;
        }
    }

    private IEnumerator UpdatePosition(InputAction.CallbackContext context)
    {
        isBallTouched = false;
        while (touchStarted)
        {
            Ray ray = mainCamera.ScreenPointToRay(touchControls.Touches.PrimaryPosition.ReadValue<Vector2>());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if ((hit.collider == ballCollider) || isBallTouched)
                {
                    isBallTouched = true;
                }
                lastTouchPoint = touchControls.Touches.PrimaryPosition.ReadValue<Vector2>();
            }
            yield return waitForFixedUpdate;
        }
    }


    float timePassed = 0.0f;
    private void FixedUpdate()
    {
        //Throwing Ball
        if (ballShot)
        {
            CollisionPhysicsHandler();
            timePassed += Time.deltaTime;
            transform.position += initVelocity * Time.deltaTime;
            initVelocity -= gravityAccel * Time.deltaTime;

            //if (timePassed >= projectileTime) ballShot = false;
        }
       
    }


    private void CollisionPhysicsHandler()
    {
        Vector3 Origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        Ray forwardRay = new Ray(Origin, transform.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(forwardRay, out hitInfo, ballCollider.bounds.extents.z))
        {
            if (!hitInfo.collider.CompareTag("Net")) initVelocity = (hitInfo.normal.normalized)* initVelocity.magnitude * boardBounceFactor;
            if (hitInfo.collider.CompareTag("Player"))
            {
                hitInfo.collider.gameObject.GetComponent<PlayerController>().initVelocity += initVelocity/2;
                initVelocity -= initVelocity / 2;
            }
        }

        Origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Ray backwardRay = new Ray(Origin, -transform.forward);
        if (Physics.Raycast(backwardRay, out hitInfo, ballCollider.bounds.extents.z))
        {
            if (!hitInfo.collider.CompareTag("Net")) initVelocity = (hitInfo.normal.normalized) * initVelocity.magnitude * boardBounceFactor;
            if (hitInfo.collider.CompareTag("Player"))
            {
                hitInfo.collider.gameObject.GetComponent<PlayerController>().initVelocity -= initVelocity / 2;
                initVelocity += initVelocity / 2;
            }
        }

        Origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Ray UpRay = new Ray(Origin, transform.up);
        if (Physics.Raycast(UpRay, out hitInfo, ballCollider.bounds.extents.y))
        {
            if (!hitInfo.collider.CompareTag("Net")) initVelocity = (hitInfo.normal.normalized) * initVelocity.magnitude * boardBounceFactor;
            if (hitInfo.collider.CompareTag("Player"))
            {
                hitInfo.collider.gameObject.GetComponent<PlayerController>().initVelocity += initVelocity / 2;
                initVelocity -= initVelocity / 2;
            }
        }

        Origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Ray downRay = new Ray(Origin, -transform.up);
        if (Physics.Raycast(downRay, out hitInfo, ballCollider.bounds.extents.y))
        {
            if (!scored && hitInfo.collider.CompareTag("Net"))
            {
                score += 1;
                scored = true;
                if(score > highScore)
                {
                    highScore = score;
                }
            }
            if (hitInfo.collider.CompareTag("Player"))
            {
                hitInfo.collider.gameObject.GetComponent<PlayerController>().initVelocity -= initVelocity / 2;
                initVelocity += initVelocity / 2;
            }
            if (hitInfo.collider.CompareTag("Floor"))
            {
                ballShot = false;
                Destroy(gameObject);
            }
            else if (!hitInfo.collider.CompareTag("Net")) initVelocity = (hitInfo.normal.normalized) * initVelocity.magnitude * boardBounceFactor;

        }
    }
}
