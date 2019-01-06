using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour {

    public int xSize = 20;
    public int zSize = 20;
    public const int ySize = 10;
    private int[] triangles;
    private Vector3[] vertices;
    private Mesh mesh;

    private Vector3 prevPosition = Vector3.negativeInfinity;
    private Vector3 prevSize = Vector3.zero;
	// Use this for initialization
	void Start () {
        CreateMesh();
        UpdateMesh();
    }

    #region Mesh 

    void CreateMesh()
    {
        var mf = GetComponent<MeshFilter>();
        mesh = mf.mesh;
        if (mesh == null)
        {
            mesh = new Mesh();
            mf.mesh = mesh;
            //mesh.MarkDynamic();
        }
        else
            Debug.Assert(false, "Add support!");
    }

    void ConfigureVertices()
    {
        var bounds = mesh.bounds;
        var sizeX = bounds.size.x * transform.localScale.x;
        var sizeY = bounds.size.y * transform.localScale.y;
        var sizeZ = bounds.size.z * transform.localScale.z;


        var xRows = xSize != 0 ? xSize + 1 : 0;
        var zRows = zSize != 0 ? zSize + 1 : 0;

        var yPosition = (float)ySize * transform.localScale.y;
        var xScale = System.Math.Abs(sizeX) > 1e-6 && xSize != 0 ? sizeX/xSize : transform.localScale.x;
        var zScale = System.Math.Abs(sizeZ) > 1e-6 && zSize != 0 ? sizeZ/zSize : transform.localScale.z;
        var yScale = transform.localScale.y;

        vertices = new Vector3[xRows * zRows];

        var index = 0;

        for (var z = 0; z < zRows; z++)
        {
            for (var x = 0; x < xRows; x++, index++)
            {
                var y = Mathf.PerlinNoise(x / (float)xRows, z / (float)zRows) * yPosition + yPosition / 5; //Random.Range(vertices.Length * 0.5f, vertices.Length);
                var vertice = new Vector3(x * xScale, y * yScale, z * zScale) + transform.localPosition;
                vertices[index] = vertice;
            }
        }

        if (triangles != null)
            return; //triangles are calculated once...

        xRows -= 1;
        zRows -= 1;

        var trisCount = 6;
        triangles = new int[xRows * zRows * trisCount];
        var pos = 0;
        var tris = 0;

        for (var z = 0; z < zRows; z++, pos++)
        {
            for (var x = 0; x < xRows; x++, tris += trisCount, pos++)
            {
                var tris2_3 = pos + 1;
                var tris1_4 = tris2_3 + xSize;

                triangles[tris + 0] = pos;
                triangles[tris + 1] = tris1_4;
                triangles[tris + 2] = tris2_3;
                triangles[tris + 3] = tris2_3;
                triangles[tris + 4] = tris1_4;
                triangles[tris + 5] = tris1_4 + 1;
            }
        }
    }

    void UpdateMesh()
    {
        if (transform.position == prevPosition &&
            ((mesh?.bounds.size ?? Vector3.negativeInfinity) == prevSize))
            return; //not changed position and size

        ConfigureVertices();
        RedefineMesh();

        prevSize = mesh.bounds.size;
        prevPosition = transform.position;
    }

    void RedefineMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    #endregion

    /*private void OnDrawGizmos()
    {
        if (vertices == null || vertices.Length == 0)
            return;

        var enumerator = vertices.GetEnumerator();
    
        while (enumerator.MoveNext())
        {
            var vertex = (Vector3)enumerator.Current;
            Gizmos.DrawSphere(vertex, 0.1f);
        }
    }*/

    // Update is called once per frame
    void Update () {
        //UpdateMesh();
    }
}