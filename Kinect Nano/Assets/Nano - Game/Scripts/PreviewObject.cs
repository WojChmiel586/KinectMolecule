using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    Vector3 mPrevPos = Vector3.zero;
    Vector3 mPosDelta = Vector3.zero;
    public float rotSpeed = 30.0f;


    // Update is called once per frame
    void Update()
    {


        mPosDelta = (Input.mousePosition - mPrevPos);


        if (Input.mousePosition.y >= Screen.height - 30)
        {
            transform.Rotate(Vector3.right * Time.deltaTime * rotSpeed, Space.World);
        }
        else if(Input.mousePosition.y < 30)
        {
            transform.Rotate(-Vector3.right * Time.deltaTime * rotSpeed, Space.World);
        }
        else if (Input.mousePosition.x >= Screen.width - 30)
        {
            transform.Rotate(-Vector3.up * Time.deltaTime * rotSpeed, Space.World);
        }
        else if(Input.mousePosition.x < 30)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * rotSpeed, Space.World);
        }
        else
        {
            if (Vector3.Dot(transform.up, Vector3.up) >= 0)
            {
                transform.Rotate(transform.up, -Vector3.Dot(mPosDelta.normalized, Camera.main.transform.right), Space.World);
            }
            else
            {
                transform.Rotate(transform.up, Vector3.Dot(mPosDelta.normalized, Camera.main.transform.right), Space.World);
            }
            transform.Rotate(Camera.main.transform.right, Vector3.Dot(mPosDelta.normalized, Camera.main.transform.up), Space.World);
        }


        mPrevPos = Input.mousePosition;
    }
}
