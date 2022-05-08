using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFiles : MonoBehaviour
{
    GameObject loaderObject;
    public int objectCount = 1;
    public int usableFIles = 0;
    public string[] fileName;
    public List<string> filesToUse;

    [SerializeField]
    private SimpleGestureListener simpleGestureListener;

    void Awake()
    {
        if (!Application.isEditor)
        {
            Debug.LogError(Application.dataPath);
        }
        LoadFilesInFolder();
        simpleGestureListener = FindObjectOfType<SimpleGestureListener>();
    }
    public void LoadFilesInFolder()
    {


        loaderObject = GameObject.FindGameObjectWithTag("LoaderObject");
        string directory = null;
        if (Application.isEditor)
        {
            directory = Application.dataPath + "/Nano - Game/Resources/Data";
        }
        else
        {
            directory = Application.dataPath + "/Data";
        }
        if (objectCount > System.IO.Directory.GetFiles(directory).Length)
        {
            objectCount = 0;
        }

        if (objectCount <= 0)
        {
            objectCount = System.IO.Directory.GetFiles(directory).Length;

        }

        fileName = System.IO.Directory.GetFiles(directory);


        foreach (string file in fileName)
        {
            if (!file.Contains("meta"))
            {
                filesToUse.Add(file);
                //Debug.Log(file + " added!");
            }
        }
        usableFIles = filesToUse.Count - 1;
        LoadObject();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightBracket) || simpleGestureListener.isRaiseLeft())
        {
            NextMol();
        }
        if (Input.GetKeyDown(KeyCode.LeftBracket) || simpleGestureListener.isRaiseRight())
        {
            PreviousMol();
        }
    }
    public void LoadObject()
    {
        //loaderObject.GetComponent<LoadXYZinEditor>().objectName = filesToUse[usableFIles];
        loaderObject.GetComponent<LoadXYZinEditor>().setXYZName(filesToUse[usableFIles]);
        loaderObject.GetComponent<LoadXYZinEditor>().LoadXYZ();
    }
    public void NextMol()
    {
        usableFIles += 1;
        if (usableFIles >= filesToUse.Count)
        {
            usableFIles = 0;
        }
        LoadObject();
    }

    public void PreviousMol()
    {
        usableFIles -= 1;
        if (usableFIles < 0)
        {
            usableFIles = filesToUse.Count - 1;
        }
        LoadObject();
    }
}
