using UnityEngine;
using System.Collections;
using System;
using TMPro;
using UnityEngine.UI;

public class SimpleGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
	[Serializable]
	public enum ApplicationState
    {
		NoUser,
		Calibration,
		Follow,
		Locked,
		Null
    }
	// GUI Text to display the gesture messages.
	public Text GestureInfo;
	[SerializeField]public ApplicationState currentState { get; private set; }

	public ZoomInOut zoomRef;

	// private bool to track if progress message has been displayed
	KinectManager manager;
	private bool progressDisplayed;
	private KinectOverlayer overlayer;

	[SerializeField]private GameObject NoUserText;
	[SerializeField]private GameObject calibrationText;
	[SerializeField]private GameObject FollowingText;
	[SerializeField]private GameObject LockedText;


    private bool jump;
    private bool zoom;
    private bool wave;

	private bool Tpose;
	private bool swipeLeft;
	private bool swipeRight;
	private bool raiseLeft;
	private bool raiseRight;

	private uint activeUserId;
	private int activeUserIndex;


    private void Start()
    {
		NoUserText.SetActive(true);
		overlayer = GetComponent<KinectOverlayer>();

	}

    private void Update()
    {
		Debug.Log(currentState);
		IsTpose();
        if (manager!= null)
        {
            if (manager.AllPlayersCalibrated && currentState == ApplicationState.Calibration)
            {
				SwitchState(ApplicationState.Follow);
            }
        }
    }
    public bool IsZoomIn()
    {
        if (zoom)
        {
            zoom = false;
            return true;
        }

        return false;
    }

    public bool IsWave()
    {
        if (wave)
        {
            wave = false;
            return true;
        }

        return false;
    }

    public bool IsJump()
    {
        if (jump)
        {
			jump = false;
            return true;
        }

        return false;
    }
	public bool IsTpose()
	{
		if (Tpose)
		{
			Debug.Log("TPOSE");

			SwitchState(currentState == ApplicationState.Follow ? ApplicationState.Locked : ApplicationState.Follow);

			Tpose = false;
			return true;
		}

		return false;
	}
	public bool isRaiseLeft()
	{
		if (raiseLeft)
		{
			raiseLeft = false;
			return true;
		}
		return false;
	}
	public bool isRaiseRight()
	{
		if (raiseRight)
		{
			raiseRight = false;
			return true;
		}
		return false;
	}

	public bool isSwipeLeft()
	{
        if (swipeLeft)
        {
			swipeLeft = false;
			return true;
        }
		return false;
	}
	public bool isSwipeRight()
	{
        if (swipeRight)
        {
			swipeRight = false;
			return true;
        }
		return false;
	}

	public void UserDetected(uint userId, int userIndex)
	{
		// as an example - detect these user specific gestures
		Debug.Log("USER DETECTED");
		SwitchState(ApplicationState.Calibration);
		manager = GetComponent<KinectManager>();
		manager.Player1Gestures.Add(KinectGestures.Gestures.Tpose);
		activeUserId = userId;
		activeUserIndex = userIndex;

        //manager.DetectGesture(userId, KinectGestures.Gestures.Jump);
        //manager.DetectGesture(userId, KinectGestures.Gestures.Squat);
        //manager.DetectGesture(userId, KinectGestures.Gestures.ZoomIn);
        //manager.DetectGesture(userId, KinectGestures.Gestures.Wave);
        //manager.DetectGesture(userId, KinectGestures.Gestures.Tpose);
        //manager.DetectGesture(userId, KinectGestures.Gestures.SwipeLeft);
        //manager.DetectGesture(userId, KinectGestures.Gestures.SwipeRight);
        //manager.DetectGesture(userId, KinectGestures.Gestures.RaiseLeftHand);
        //manager.DetectGesture(userId, KinectGestures.Gestures.RaiseRightHand);
        //manager.DetectGesture(userId, KinectGestures.Gestures.Push);
        //manager.DetectGesture(userId, KinectGestures.Gestures.Pull);

        //manager.DetectGesture(userId, KinectWrapper.Gestures.SwipeUp);
        //manager.DetectGesture(userId, KinectWrapper.Gestures.SwipeDown);
        if (manager.AllPlayersCalibrated)
        {
			SwitchState(ApplicationState.Follow);
        }
	}
	
	public void UserLost(uint userId, int userIndex)
	{
		if(GestureInfo != null)
		{
			GestureInfo.text = string.Empty;
		}
	}

	public void GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              float progress, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
	{

        GestureInfo.text = string.Format("{0} Progress: {1:F1}%", gesture, (progress * 100));


        if (gesture == KinectGestures.Gestures.Click && progress > 0.3f)
        {
            string sGestureText = string.Format("{0} {1:F1}% complete", gesture, progress * 100);
            if (GestureInfo != null)
                GestureInfo.text = sGestureText;

            progressDisplayed = true;
        }

        if (currentState == ApplicationState.Locked)
        {
			if ((gesture == KinectGestures.Gestures.ZoomOut || gesture == KinectGestures.Gestures.ZoomIn) && progress > 0.5f)
			{
				string sGestureText = string.Format("{0} detected, zoom={1:F1}%", gesture, screenPos.z * 100);

				if (GestureInfo != null)
				{
					//Debug.Log("CHECK: " + screenPos.z);
					GestureInfo.text = sGestureText;
					zoomRef.zoomValue = screenPos.z;

				}

				progressDisplayed = true;

			}
			else if (gesture == KinectGestures.Gestures.Wheel && progress > 0.5f)
			{
				string sGestureText = string.Format("{0} detected, angle={1:F1} deg", gesture, screenPos.z);
				if (GestureInfo != null)
					GestureInfo.text = sGestureText;

				progressDisplayed = true;
			}
		}

    }

	public bool GestureCompleted (uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
	{
		string sGestureText = gesture + " detected";

		//if(gesture == KinectGestures.Gestures.Click)
		//	sGestureText += string.Format(" at ({0:F1}, {1:F1})", screenPos.x, screenPos.y);
		
		if(GestureInfo != null)
			GestureInfo.text = sGestureText;
		
		progressDisplayed = false;

		if (gesture == KinectGestures.Gestures.Wave)
            wave = true;

        if (gesture == KinectGestures.Gestures.Jump)
            jump = true;

        if (gesture == KinectGestures.Gestures.SwipeLeft)
        {
			swipeLeft = true;
        }
        if (gesture == KinectGestures.Gestures.SwipeRight)
        {
			swipeRight = true;
        }

		if (gesture == KinectGestures.Gestures.RaiseLeftHand)
		{
			raiseLeft = true;
		}

		if (gesture == KinectGestures.Gestures.RaiseRightHand)
		{
			raiseRight = true;
		}
		if (gesture == KinectGestures.Gestures.Tpose)
		{
			Tpose = true;
		}

		return true;
	}

	public bool GestureCancelled (uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectWrapper.NuiSkeletonPositionIndex joint)
	{
		if(progressDisplayed)
		{
			// clear the progress info
			if(GestureInfo != null)
				GestureInfo.text = String.Empty;
			
			progressDisplayed = false;

        }

        if (gesture == KinectGestures.Gestures.ZoomIn)
            zoom = true;

        return true;
	}
	public void SwitchState(ApplicationState newState)
    {
        switch (newState)
        {
            case ApplicationState.Calibration:
				disableTextPrompts();
				calibrationText.SetActive(true);
				currentState = newState;

				break;
            case ApplicationState.Follow:
				disableTextPrompts();
				FollowingText.SetActive(true);
				overlayer.followHands = true;
				//Gestures to add
				//manager.Player1Gestures.Add(KinectGestures.Gestures.Tpose);
				manager.DetectGesture(activeUserId, KinectGestures.Gestures.Tpose);

				//Gestures to remove
				manager.DeleteGesture(activeUserId, KinectGestures.Gestures.RaiseLeftHand);
				manager.DeleteGesture(activeUserId, KinectGestures.Gestures.RaiseRightHand);
				manager.DeleteGesture(activeUserId, KinectGestures.Gestures.ZoomIn);

				currentState = newState;

				break;
            case ApplicationState.Locked:
				disableTextPrompts();
				LockedText.SetActive(true);
				overlayer.followHands = false;
				//Gestures to add
				manager.DetectGesture(activeUserId, KinectGestures.Gestures.RaiseLeftHand);
				manager.DetectGesture(activeUserId, KinectGestures.Gestures.RaiseRightHand);
				manager.DetectGesture(activeUserId, KinectGestures.Gestures.ZoomIn);
				//Gestures to remove

				currentState = newState;
				break;
            case ApplicationState.Null:
                break;
            default:
                break;
        }
    }
	private void disableTextPrompts()
	{
		NoUserText.SetActive(false);
		calibrationText.SetActive(false);
		FollowingText.SetActive(false);
		LockedText.SetActive(false);
	}
}
