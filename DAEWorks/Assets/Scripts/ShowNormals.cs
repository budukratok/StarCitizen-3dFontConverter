using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class ShowNormals : MonoBehaviour
{
    public float Length = 1f;

    private MeshFilter _mf;
    private Mesh _mesh;
    private Vector3[] normals;
    private Vector4[] tangents;
    private Vector3[] verts;

    public bool ColorGradient = false;

    public Vector3 EulerAngles;
    public Vector3 LocalEulerAngles;

    public bool HighlightIndex = false;
    public int Index = 0;

    // Use this for initialization
    void Start ()
    {
	    
	}

#if UNITY_EDITOR
    // Update is called once per frame
    void Update ()
    {
        //if (!Application.isPlaying)
        //{
            if (_mf == null)
            {
                _mf = GetComponent<MeshFilter>();
            }

            _mesh = _mf.sharedMesh;

            normals = _mesh.normals;
            tangents = _mesh.tangents;
            verts = _mesh.vertices;

        EulerAngles = _mf.transform.eulerAngles;
        LocalEulerAngles = _mf.transform.localEulerAngles;

        /*for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = verts[i].normalized;
            }

            _mf.sharedMesh.normals = normals;
             */

        /*for (int i = 0; i < tangents.Length; i++)
        {
            tangents[i] = -Vector3.left;
        }

        _mf.sharedMesh.tangents = tangents;*/
        //}
    }

    void OnDrawGizmos()
    {
        Handles.color = Color.green;
        if (normals != null)
        {
            for (int i = 0; i < normals.Length; i++)
            {
                if (ColorGradient)
                {
                    Handles.color = Color.Lerp(Color.green, Color.red, (float)i/normals.Length);
                    Handles.DrawLine(transform.TransformPoint(verts[i]),
                        transform.TransformPoint(verts[i] + normals[i] * Length)); 
                }
                else
                {
                    Handles.DrawLine(transform.TransformPoint(verts[i]),
                        transform.TransformPoint(verts[i] + normals[i] * Length));
                }
            }
        }

        Handles.color = Color.blue;
        if (tangents != null)
        {
            for (int i = 0; i < tangents.Length; i++)
            {
                Handles.DrawLine(transform.TransformPoint(verts[i]),
                    transform.TransformPoint(verts[i] +
                                             new Vector3(tangents[i].x, tangents[i].y, tangents[i].z)*Length));
            }
        }
        
        Handles.color = Color.red;
        if (normals != null && HighlightIndex)
        {
            if (Index < normals.Length && Index > 0)
            {
                Handles.DrawLine(transform.TransformPoint(verts[Index]),
                    transform.TransformPoint(verts[Index] + normals[Index] * Length));
            }

        }
    }
#endif
}
