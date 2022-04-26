using Dummiesman;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ObjFromFile : MonoBehaviour
{
    string objPath = string.Empty;
    string error = string.Empty;
    public GameObject loadedObject;
    string ScriptName;
    //public Text moleculeName;
    public List<GameObject> myList = new List<GameObject>();
    private int currentActiveIndex = 0;
    private SimpleGestureListener simpleGestureListener;
    private ZoomInOut zoomInOut;
    private KinectOverlayer kinectOverlayer;

    void Start()
    {
        simpleGestureListener = GetComponent<SimpleGestureListener>();
        zoomInOut = GetComponent<ZoomInOut>();
        kinectOverlayer = GetComponent<KinectOverlayer>();

    }

    void Update()
    {
                //if ((Input.GetMouseButtonDown(0)) || simpleGestureListener.IsWave())
        if ((Input.GetMouseButtonDown(0)))
        {

            myList[currentActiveIndex].SetActive(false);
            currentActiveIndex++;
            if (currentActiveIndex >= myList.Count)
                currentActiveIndex = 0;
            myList[currentActiveIndex].SetActive(true);
            kinectOverlayer.OverlayObject = myList[currentActiveIndex];
        }

    }

    //void OnGUI()
    //{
    //    objPath = GUI.TextField(new Rect(0, 0, 256, 32), objPath);

    //    GUI.Label(new Rect(0, 0, 256, 32), "Obj Path:");
    //    if (GUI.Button(new Rect(256, 0, 64, 32), "Load File"))
    //    {
    //        DirectoryInfo d = new DirectoryInfo(objPath);//Assuming Test is your Folder
    //        FileInfo[] Files = d.GetFiles("*.obj"); //Getting obj files

    //        foreach (FileInfo file in Files)
    //        {
    //            loadedObject = new OBJLoader().Load(objPath + file.Name);
    //            loadedObject.tag = "Molecule";

    //            //adds the script to the loaded object
    //            System.Type MyScriptType = System.Type.GetType("PreviewObject" + ",Assembly-CSharp");

    //            loadedObject.AddComponent(MyScriptType);
    //            myList.Add(loadedObject);
    //            loadedObject.SetActive(false);
    //        }
    //        //gets the overlayer and loads the object so that it will follow your hand
    //    }

    //    if (!string.IsNullOrWhiteSpace(error))
    //    {
    //        GUI.color = Color.red;
    //        GUI.Box(new Rect(0, 64, 256 + 64, 32), error);
    //        GUI.color = Color.white;
    //    }
    //}
}