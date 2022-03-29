using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInOut : MonoBehaviour
{
    public int zoom = 20;
    public int normal = 60;
    public float smooth = 5;

    private bool isZoomed = false;
    public bool isJump = false;

    private GameObject gameObject;
    public GameObject gestureCam;
    Vector3 originalPos;
    private SimpleGestureListener simpleGestureListener;
    private KinectManager kinectManager;
    private KinectOverlayer kinectOverlayer;

    void Start()
    {
        simpleGestureListener = GetComponent<SimpleGestureListener>();
        kinectManager = GetComponent<KinectManager>();

        gameObject = GameObject.FindWithTag("Molecule");

        originalPos = gameObject.transform.position;

    }

    void Update()
    {
        //if (simpleGestureListener.IsWave())
        //{
        //    originalPos = new Vector3(0, 0, 0);
        //}

        if (simpleGestureListener.IsJump())
        {
            isJump = !isJump;
            
        }

        if (isJump)
        {
            if ((Input.GetMouseButtonDown(1)) || (simpleGestureListener.IsZoomIn()))
            {
                isZoomed = !isZoomed;
            }

            if (isZoomed)
            {
                Camera.main.GetComponent<Camera>().fieldOfView = Mathf.Lerp(Camera.main.GetComponent<Camera>().fieldOfView, zoom, Time.deltaTime * smooth);
            }
            else
            {
                Camera.main.GetComponent<Camera>().fieldOfView = Mathf.Lerp(Camera.main.GetComponent<Camera>().fieldOfView, normal, Time.deltaTime * smooth);
            }
   
            gameObject.transform.position = originalPos;
            gestureCam.GetComponent<KinectOverlayer>().enabled = false;
            kinectManager.ControlMouseCursor = !kinectManager.ControlMouseCursor;        
        }
        else
        {

            gestureCam.GetComponent<KinectOverlayer>().enabled = true;
            //kinectOverlayer.OverlayObject = gameObject;
            kinectManager.ControlMouseCursor = false;
        }
    }
}
