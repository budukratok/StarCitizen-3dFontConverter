using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

// all of this code was a quick sketch to help our team with good lookin' 3d font and don't bother 3d modelers
// with all of this little pivot position and local rotation fixes
// if you have a ready to export 3d model, use Save.cs component directly 

[ExecuteInEditMode] // ExecuteInEditMode to run code in editor without inspector component
public class Process : MonoBehaviour
{
    [Header("Objects to process")]
    public List<GameObject> LettersToFix = new List<GameObject>(); // list of game objects with mesh renderer+filter to perform moditifcations

    [Header("MOVE params (move vertices)")]
    [Header("Objects will be overwritten and serialized info will be saved in the scene data.")]
    [Header("Select operations and check 'Run' to process.")]
    public bool MovePivot = false; // set to true to perform move operation
    [HideInInspector]
    public float PivotX = 0f; // currently not used
    public float PivotY = 0f; // local y pivot position
    public float PivotZ = 0f; // global z pivot position
    
    [Header("ROTATE params (rotate vertices around point)")]
    public bool RotatePivot = false; // set to true to perform rotate around pivot 'RotateAroundPivot' operation
    public Vector3 RotateAroundPivot; // if RotatePivot is 'true' this point will be used as a pivot for rotation
    
    
    [Header("RESIZE params (move vertices relative to pivot)")]
    public bool Resize = false; // set to true to perform resize operation
    public float ResizeModifier = 1f; // resize modifier
    
    //public bool MergeMeshes = false; //not used
    
    // we don't know normals and tangents of original meshes, instead of this we just replace them with the predefined ones on the import step by Defter's code
    [Space]
    [HideInInspector]
    public bool FixNormalsAndTangents = false; //deprecated, see above
    [HideInInspector]
    public bool CalculateNormalsToCamera = false; //deprecated, see above
    

    [Header("Additional params")]
    public bool RemoveWhitespace = true; // remove whitespaces on result objects

    

    [Header("Check 'Run' to process selected operations. Objects will be overwritten!")]
    public bool Run = false; // preform opreations marked as 'true' (MovePivot, RotatePivot, Resize)
    
    
    // Update is called once per frame
    void Update()
    {
        if (Run)
        {
            // deprecated
            // if (MergeMeshes)
            // {
            //     for (int i = 0; i < _newLetterObjects.Count; i++)
            //     {
            //         DestroyImmediate(_newLetterObjects[i]);
            //     }
            // }
            
            for (int i = 0; i < LettersToFix.Count; i++)
            {
                // deprecated - we don't modify normals and tangents anymore
                // if (MergeMeshes)
                // {
                //     MergeLetterMeshes(LettersToFix[i]);
                // }
                // else
                    FixLetter(LettersToFix[i]);
            }
            

            Run = false;
        }
    }

    void FixLetter(GameObject letterObject)
    {
        Vector3 localYPos = new Vector3(PivotX, PivotY, PivotZ);
            
        var meshFilter = letterObject.GetComponent<MeshFilter>();
        Vector3 pivotPos = Vector3.zero;
        if (meshFilter != null)
        {
            var mesh = meshFilter.mesh;

            if(MovePivot)
                pivotPos = GetPivotPos(mesh.vertices, localYPos, letterObject);

            var newVertices = mesh.vertices;
            for (int i = 0; i < newVertices.Length; i++)
            {
                newVertices[i] -= pivotPos;
                
                if(RotatePivot)
                    newVertices[i] = RotatePointAroundPivot(newVertices[i], pivotPos, RotateAroundPivot);

                if (Resize)
                    newVertices[i] =
                        (newVertices[i]-pivotPos) *
                        ResizeModifier;
            }

            mesh.vertices = newVertices;

            if (CalculateNormalsToCamera)
            {
                var newNormals = mesh.normals;

                for (int i = 0; i < mesh.normals.Length; i++)
                {
                    Vector3 toCamera = mesh.vertices[i] - pivotPos;
                    toCamera.x = 0;
                    toCamera.z = 0;
                    toCamera = toCamera.normalized;
                    newNormals[i] = toCamera;
                }

                mesh.normals = newNormals;
                mesh.RecalculateTangents();
            }
            else if (FixNormalsAndTangents)
            {
                mesh.normals = null;
                mesh.RecalculateNormals();
                mesh.RecalculateTangents();
            }

            mesh.RecalculateBounds();
            meshFilter.mesh = mesh;
        }

        letterObject.transform.position = letterObject.transform.TransformPoint(pivotPos);

        if (RemoveWhitespace)
            letterObject.name = letterObject.name.Replace(@" ", "");
    }

    // void MergeLetterMeshes(GameObject letterObject)
    // {
    //     letterObject.gameObject.SetActive(true);
    //     for (int i = 0; i < letterObject.transform.childCount; i++)
    //     {
    //         letterObject.transform.GetChild(i).gameObject.SetActive(true);
    //     }
    //     
    //     GameObject newLetterObject = new GameObject(letterObject.transform.name.Replace("_side", ""));
    //     _newLetterObjects.Add(newLetterObject);
    //
    //     var newLetterMeshFilter = newLetterObject.AddComponent<MeshFilter>();
    //     var newLetterMeshRenderer = newLetterObject.AddComponent<MeshRenderer>();
    //
    //     var sideObject = letterObject.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name.Contains("_side"));
    //     var frontObject = letterObject.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name.Contains("_front"));
    //     var backObject = letterObject.GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name.Contains("_back"));
    //
    //     var sideMesh = GetMeshWithFixedNormals(sideObject, false); //к камере
    //     var frontMesh = GetMeshWithFixedNormals(frontObject, true); //от камеры
    //     var backMesh = GetMeshWithFixedNormals(backObject,true); //от камеры
    //
    //     var meshesToMerge = new Mesh[3];
    //     meshesToMerge[0] = sideMesh;
    //     meshesToMerge[1] = frontMesh;
    //     meshesToMerge[2] = backMesh;
    //
    //     var transformsToMerge = new Transform[3];
    //     transformsToMerge[0] = sideObject;
    //     transformsToMerge[1] = frontObject;
    //     transformsToMerge[2] = backObject;
    //     
    //     CombineInstance[] combine = new CombineInstance[3];
    //     for (int i = 0; i < 3; i++)
    //     {
    //         combine[i].mesh = meshesToMerge[i];
    //         combine[i].transform = transformsToMerge[i].localToWorldMatrix;
    //         transformsToMerge[i].gameObject.SetActive(false);
    //     }
    //     
    //     newLetterMeshFilter.mesh = new Mesh();
    //     newLetterMeshFilter.mesh.CombineMeshes(combine, true, true);
    //
    //     newLetterMeshRenderer.sharedMaterial = MergedMeshMaterial;
    //     
    //     newLetterObject.transform.SetParent(transform);
    //     
    //     if (LetterReference != null)
    //     {
    //         var meshReference = LetterReference.GetComponent<MeshFilter>().sharedMesh;
    //         Vector3 referenceNormalGlobal = LetterReference.transform.TransformDirection(meshReference.normals[0]);
    //         FixLetter(newLetterObject, referenceNormalGlobal);
    //     }
    // }

    // Mesh GetMeshWithFixedNormals(Transform obj, bool toCamera)
    // {
    //     Vector3 vectorToCamera = new Vector3(0f, -1f, 0f);
    //     Vector3 vectorFromCamera = new Vector3(0f, 1f, 0f);
    //
    //     Vector3 currentNormal = toCamera ? vectorToCamera : vectorFromCamera;
    //     
    //     var meshFilter = obj.GetComponent<MeshFilter>();
    //     if (meshFilter != null)
    //     {
    //         var mesh = meshFilter.mesh;
    //         var newNormals = mesh.normals;
    //
    //         for (int i = 0; i < mesh.vertexCount; i++)
    //         {
    //             newNormals[i] = currentNormal;
    //         }
    //
    //         mesh.normals = newNormals;
    //         mesh.RecalculateBounds();
    //         meshFilter.mesh = mesh;
    //         
    //         return mesh;
    //     }
    //
    //     return null;
    // }

    Vector3 GetPivotPos(Vector3[] vertices, Vector3 localYPos, GameObject letterObject)
    {
        float maxX = vertices.Max(v => v.x);
        float minX = vertices.Min(v => v.x);
        float centerX = (maxX + minX) / 2;
        
        float maxY = vertices.Max(v => v.y);
        float minY = vertices.Min(v => v.y);
        float centerY = (maxY + minY) / 2;
        
        float maxZ = vertices.Max(v => v.z);
        float minZ = vertices.Min(v => v.z);
        float centerZ = (maxZ + minZ) / 2;
        
        Vector3 position = new Vector3(centerX, centerY, letterObject.transform.InverseTransformPoint(localYPos).z); 
        
        return position;
    }
    
    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }
}
