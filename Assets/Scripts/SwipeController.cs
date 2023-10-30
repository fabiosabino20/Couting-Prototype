using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer), typeof(BoxCollider))]

public class SwipeController : MonoBehaviour
{
    private bool isSwiping = false;
    private BoxCollider boxCol;
    private Camera cam;
    private float swipeTimer = 0;
    private GameObject ball;
    private Rigidbody ballRb;
    private TrailRenderer trail;
    private Vector3 mouseInitialPos, mouseFinalPos, mousePos; // mousePos used for trail
    public static bool isActive = true; // used to check if there's an active ball waiting to be thrown

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        trail = GetComponent<TrailRenderer>();
        boxCol = GetComponent<BoxCollider>();
        trail.enabled = false;
        boxCol.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isSwiping = true;
            UpdateComponents();
            mouseInitialPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1f));
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isSwiping = false;
            UpdateComponents();
            mouseFinalPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1f));
            float throwStrenght = (mouseFinalPos.y - mouseInitialPos.y)/swipeTimer;
            float throwStrenghtZ = (mouseFinalPos.z - mouseInitialPos.z)/swipeTimer;
            if (ballRb != null)
            {
                ballRb.useGravity = true;
                ballRb.AddForce(new Vector3(-throwStrenght, throwStrenght, throwStrenghtZ), ForceMode.Impulse);
                ballRb.AddTorque(new Vector3(-throwStrenght, throwStrenght, throwStrenghtZ), ForceMode.Impulse);
                ballRb = null;
                isActive = false;
            }
            swipeTimer = 0;
            mousePos = Vector3.zero;
            mouseInitialPos = Vector3.zero;
            mouseFinalPos = Vector3.zero;
        }
        if (isSwiping)
        {
            UpdateMousePosition();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            ball = other.gameObject;
            ballRb = ball.GetComponent<Rigidbody>();
        }
    }

    private void UpdateMousePosition()
    {
        mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.8f));
        transform.position = mousePos;
        //if (ball != null)
            //ball.transform.position = mousePos;
        swipeTimer += Time.deltaTime;
    }

    private void UpdateComponents()
    {
        trail.enabled = isSwiping;
        boxCol.enabled = isSwiping;
    }
}
