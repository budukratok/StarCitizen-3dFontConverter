using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class CalculateWidthPercent : MonoBehaviour
{
    public bool Run = false;
    public GameObject ReferenceObject;
    public List<GameObject> ListOfObjects = new List<GameObject>();
    
    void Update()
    {
        if (Run)
        {
            CalculateWidth();
            Run = false;
        }
    }

    public void CalculateWidth()
    {
        string[] widthList = new string[ListOfObjects.Count];

        float referenceWidth = GetWidth(ReferenceObject.GetComponent<MeshFilter>());

        string resultString = ReferenceObject.transform.name + "="+referenceWidth+"="+(1f).ToString("0.#####")+"\n";
        for (int i = 0; i < ListOfObjects.Count; i++)
        {
            float width = GetWidth(ListOfObjects[i].GetComponent<MeshFilter>());
            widthList[i] = ListOfObjects[i].transform.name + "=" + width.ToString("0.#####")+"="+(width/referenceWidth)+"\n";
            resultString += widthList[i];
        }
        
        Debug.Log(resultString);
    }

    public float GetWidth(MeshFilter meshFilter)
    {
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        
        if (meshFilter != null)
        {
            minX = meshFilter.mesh.vertices.Min(v => v.x);
            maxX = meshFilter.mesh.vertices.Max(v => v.x);
            return maxX - minX;
        }

        return 0f;
    }
}
