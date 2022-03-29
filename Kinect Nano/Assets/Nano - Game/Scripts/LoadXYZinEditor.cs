using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomType
{
    public string baseName;
    public string name;
    public int atomicNumber;
    public float mass;
    public float bondLength;
    public float radius;
    public string colour;
}

public class LoadXYZinEditor : MonoBehaviour
{
    public GameObject holoplay;
    public GameObject atomPrefab;
    public GameObject bondPrefab;
    public GameObject atomParent;

    public string filename;
    public string objectName;

    public GameObject[] atoms;
    public List<GameObject> bonds;

    public string Filename
    {
        get
        {
            return filename;
        }

        set
        {
            filename = value;
        }
    }

    public bool generalDataLoaded = false;
    public static Dictionary<string, AtomType> types = new Dictionary<string, AtomType>();
    public static Dictionary<string, Material> materials = new Dictionary<string, Material>();

    public Material BaseMaterial;

    public float atomScale = 1.0f;
    public float bondWidth = 0.1f;


    public void setXYZName(string name)
    {
        objectName = name;
    }
    public void LoadXYZ()
    {
        //load types & colours in
        if (!generalDataLoaded || types.Count == 0 || materials.Count == 0)
        {
            generalDataLoaded = true;

            if (types.Count == 0)
            {
                string[] typesData = System.IO.File.ReadAllLines("Assets/Nano - Game/Resources/TypeData/xyz.types");
                int numTypes = (int)System.Convert.ToDouble(typesData[0]);
                for (int i = 0; i < numTypes; i++)
                {
                    string[] data = typesData[i + 1].Split(new char[0], System.StringSplitOptions.RemoveEmptyEntries);
                    AtomType newType = new AtomType();
                    newType.baseName = data[0];
                    newType.name = data[1];
                    newType.atomicNumber = (int)System.Convert.ToSingle(data[2]);
                    newType.mass = System.Convert.ToSingle(data[3]);
                    newType.bondLength = System.Convert.ToSingle(data[4]);
                    newType.radius = System.Convert.ToSingle(data[5]);
                    newType.colour = data[6];
                    if (data.Length > 7)
                    {
                        for (int j = 7; j < data.Length; j++)
                        {
                            newType.colour += ' ' + data[j];
                        }
                    }
                    types[newType.name] = newType;
                }
            }
            if (materials.Count == 0)
            {
                string[] coloursData = System.IO.File.ReadAllLines("Assets/Nano - Game/Resources/TypeData/rgb.txt");
                int numCols = coloursData.Length;
                for (int i = 0; i < numCols; i++)
                {
                    if (coloursData[i].ToCharArray()[0] != '!')
                    {
                        Color newColour = new Color();
                        string[] data = coloursData[i].Split(new char[0], System.StringSplitOptions.RemoveEmptyEntries);
                        newColour.r = System.Convert.ToSingle(data[0]) / 255;
                        newColour.g = System.Convert.ToSingle(data[1]) / 255;
                        newColour.b = System.Convert.ToSingle(data[2]) / 255;
                        newColour.a = 1;
                        string colName = data[3];
                        if (data.Length > 4)
                        {
                            for (int j = 4; j < data.Length; j++)
                            {
                                colName += ' ' + data[j];
                            }
                        }

                        Material newMaterial = new Material(BaseMaterial);
                        newMaterial.SetColor("_Color", newColour);
                        materials[colName] = newMaterial;
                    }
                }
            }
        }

        //this.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        //load XYZ from file
        string[] lines = System.IO.File.ReadAllLines(objectName);

        if (atoms.Length > 0)
        {
            foreach (GameObject o in atoms)
            {
                if (o != null) DestroyImmediate(o);
            }
            foreach (GameObject o in bonds)
            {
                if (o != null) DestroyImmediate(o);
            }
            System.Array.Clear(atoms, 0, atoms.Length);
            bonds.Clear();
        }
        int numAtoms = System.Convert.ToInt32(lines[0]);

        atoms = new GameObject[numAtoms];

        Vector3 avePos = Vector3.zero;

        for (int i = 0; i < numAtoms; i++)
        {
            atoms[i] = Instantiate(atomPrefab);
            string[] data = lines[i + 2].Split(new char[0], System.StringSplitOptions.RemoveEmptyEntries);
            Vector3 position = new Vector3(System.Convert.ToSingle(data[1]), System.Convert.ToSingle(data[2]), System.Convert.ToSingle(data[3]));
            avePos += position;
            atoms[i].name = data[0];
            atoms[i].transform.SetParent(atomParent.transform);
            atoms[i].transform.localPosition = position;

            AtomType AT = types[data[0]];
            atoms[i].transform.localScale = AT.radius * atomScale * Vector3.one;

            atoms[i].GetComponent<Renderer>().material = materials[AT.colour];
        }

        avePos /= numAtoms;

        for (int i = 0; i < numAtoms; i++)
        {
            atoms[i].transform.localPosition = atoms[i].transform.position - avePos;
        }

        for (int i = 0; i < numAtoms - 1; i++)
        {
            for (int j = i + 1; j < numAtoms; j++)
            {
                float dist = Vector3.Distance(atoms[i].transform.position, atoms[j].transform.position);
                if (dist < types[atoms[i].name].bondLength * atomParent.transform.localScale.x)
                {
                    GameObject newBond = Instantiate(bondPrefab);
                    bonds.Add(newBond);
                    newBond.name = atoms[i].name + '-' + atoms[j].name;
                    newBond.transform.SetParent(atomParent.transform);
                    newBond.transform.localScale = new Vector3(bondWidth, 0.5f * dist, bondWidth);//magic half as the primitive is 2 tall
                    newBond.transform.Translate(0.5f * dist * Vector3.up);
                    Vector3 direction = Vector3.Normalize(atoms[j].transform.position - atoms[i].transform.position);
                    newBond.transform.position = atoms[i].transform.position + 0.5f * dist * direction;
                    newBond.transform.up = direction;
                    newBond.GetComponent<Renderer>().material = materials["white"];
                }
            }
        }
    }
}
