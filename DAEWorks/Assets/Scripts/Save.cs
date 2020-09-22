using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class Save : MonoBehaviour
{
    public string FileName = "Orbitron_unity2";
    public bool Run = false;
    public GameObject SaveGameObject;
    void Start ()
    {
        
    }

    void Update()
    {
        if (Run)
        {
            SaveToFile();

            Run = false;
        }
    }

    void SaveToFile()
    {
        ColladaExporter export = new ColladaExporter(FileName+".dae", true);

        for (int i = 0; i < SaveGameObject.transform.childCount; i++)
        {
            var meshFilter = SaveGameObject.transform.GetChild(i).GetComponent<MeshFilter>();
            if (meshFilter.gameObject.activeSelf && meshFilter != null)
            {
                export.AddGeometry(meshFilter.transform.name, meshFilter.mesh);
                Vector3 position = meshFilter.transform.position;
                position.x = -1 * position.x;
                var matrix = Matrix4x4.TRS(position, meshFilter.transform.rotation, Vector3.one);
                export.AddGeometryToScene(meshFilter.transform.name, meshFilter.transform.name, matrix);
            }
        }
        
        export.Save();
    }

}
