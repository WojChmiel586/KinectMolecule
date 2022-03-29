using Dummiesman;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MultipleObjFromFile : MonoBehaviour
{
    string objPath = string.Empty;
    string error = string.Empty;
    public GameObject loadedObject;
    string ScriptName;
    //public Text moleculeName;
    GameObject[] Molecules;


    void OnGUI()
    {
        objPath = GUI.TextField(new Rect(0, 0, 256, 32), objPath);

        GUI.Label(new Rect(0, 0, 256, 32), "Obj Path:");
        if (GUI.Button(new Rect(256, 0, 64, 32), "Load File"))
        {
            if (loadedObject != null)
                Destroy(loadedObject);
            loadedObject = new OBJLoader().Load(objPath);

            //loadedObject = new OBJLoader().Load(objPath);

            //this gets the object holder and the name of the file
            GameObject Meem = GameObject.Find("Meem");
            //moleculeName.text = "Molecular Formula:\n" + loadedObject.name;

            //this gets the name of cursor rotation script
            ScriptName = "PreviewObject";
            //fetches the type of the Rotation script
            System.Type MyScriptType = System.Type.GetType(ScriptName + ",Assembly-CSharp");
            //adds the script to the loaded object
            loadedObject.AddComponent(MyScriptType);
            //loadedObject.SetActive(true);

            //gets the overlayer and loads the object so that it will follow your hand
            if (Meem)
            {
                KinectOverlayer kinectOverlayer = Meem.GetComponent<KinectOverlayer>();
                kinectOverlayer.OverlayObject = loadedObject;
            }
        }

        if (!string.IsNullOrWhiteSpace(error))
        {
            GUI.color = Color.red;
            GUI.Box(new Rect(0, 64, 256 + 64, 32), error);
            GUI.color = Color.white;
        }
    }
}



