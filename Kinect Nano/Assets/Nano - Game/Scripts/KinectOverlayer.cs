using UnityEngine;
using System.Collections;

public class KinectOverlayer : MonoBehaviour 
{
//	public Vector3 TopLeft;
//	public Vector3 TopRight;
//	public Vector3 BottomRight;
//	public Vector3 BottomLeft;

	public GUITexture backgroundImage;
	public KinectWrapper.NuiSkeletonPositionIndex TrackedRightHand = KinectWrapper.NuiSkeletonPositionIndex.HandRight;
	public KinectWrapper.NuiSkeletonPositionIndex TrackedLeftHand = KinectWrapper.NuiSkeletonPositionIndex.HandLeft;
	public GameObject OverlayObject;
	public float smoothFactor = 5f;
	public bool followHands = false;
	
	public GUIText debugText;

	[SerializeField]private float distanceToCamera = 20f;


	void Start()
	{
		if(OverlayObject)
		{
			//distanceToCamera = (OverlayObject.transform.position - Camera.main.transform.position).magnitude;
		}
	}
	
	void Update() 
	{
		KinectManager manager = KinectManager.Instance;
		
		if(manager && manager.IsInitialized())
		{
			//backgroundImage.renderer.material.mainTexture = manager.GetUsersClrTex();
			if(backgroundImage && (backgroundImage.texture == null))
			{
				backgroundImage.texture = manager.GetUsersClrTex();
			}
			
//			Vector3 vRight = BottomRight - BottomLeft;
//			Vector3 vUp = TopLeft - BottomLeft;
			//hi
			int iJointIndex = (int)TrackedRightHand;
			
			if(manager.IsUserDetected())
			{
				uint userId = manager.GetPlayer1ID();
				
				if(manager.IsJointTracked(userId, iJointIndex))
				{
					Vector3 posJoint = manager.GetRawSkeletonJointPos(userId, iJointIndex);
					Vector3 posJointL = manager.GetRawSkeletonJointPos(userId, (int)TrackedLeftHand);

					if(posJoint != Vector3.zero)
					{
						// 3d position to depth
						Vector2 posDepth = manager.GetDepthMapPosForJointPos(posJoint);
						Vector2 posDepth2 = manager.GetDepthMapPosForJointPos(posJointL);
						
						// depth pos to color pos
						Vector2 posColor = manager.GetColorMapPosForDepthPos(posDepth);
						Vector2 posColor2 = manager.GetColorMapPosForDepthPos(posDepth2);
						
						float scaleX = ((float)posColor.x + ((float)posColor2.x - (float)posColor.x)/2) / KinectWrapper.Constants.ColorImageWidth;
						float scaleY = 1.0f - ((float)posColor.y +((float)posColor2.y - (float)posColor.y)/2)/ KinectWrapper.Constants.ColorImageHeight;
						
//						Vector3 localPos = new Vector3(scaleX * 10f - 5f, 0f, scaleY * 10f - 5f); // 5f is 1/2 of 10f - size of the plane
//						Vector3 vPosOverlay = backgroundImage.transform.TransformPoint(localPos);
						//Vector3 vPosOverlay = BottomLeft + ((vRight * scaleX) + (vUp * scaleY));

						if(debugText)
						{
							debugText.GetComponent<GUIText>().text = "Tracked user ID: " + userId;  // new Vector2(scaleX, scaleY).ToString();
						}
						
						if(OverlayObject && followHands)
						{
							Vector3 vPosOverlay = Camera.main.ViewportToWorldPoint(new Vector3(scaleX, scaleY, distanceToCamera));
							OverlayObject.transform.position = Vector3.Lerp(OverlayObject.transform.position, vPosOverlay, smoothFactor * Time.deltaTime);
						}
					}
				}
				
			}
			
		}
	}
}
