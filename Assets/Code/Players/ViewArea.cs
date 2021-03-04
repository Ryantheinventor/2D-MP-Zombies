using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewArea : MonoBehaviour
{
    public float viewDist = 10f;
    public int rayCount = 4;

    Vector3[] newVertices;
    Vector2[] newUV;
    int[] newTriangles;

    void Update()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        newVertices = new Vector3[rayCount + 1];
        newUV = new Vector2[newVertices.Length];
        newTriangles = new int[rayCount * 3];

        newVertices[0] = Vector3.zero;


        float angleIncrease = 360f / rayCount;

        //find verts
        for (int i = 0; i < rayCount; i++) 
        {
            int layerMask = 1 << 8;
            float curAngle = angleIncrease * i;
            float angleRad = curAngle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * viewDist;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, viewDist, layerMask);
            if (hit.collider != null)
            {
                newVertices[i + 1] = new Vector3(hit.point.x,hit.point.y) - transform.position;
            }
            else 
            {
                newVertices[i + 1] = direction;
            }
        }

        //make triangles
        for (int i = 0; i < rayCount; i++)
        {
            int offset = i * 3;
            newTriangles[0 + offset] = 0;
            newTriangles[1 + offset] = i + 1;
            if (i == rayCount - 1)
            {
                newTriangles[2 + offset] = 1;
            }
            else 
            {
                newTriangles[2 + offset] = i + 2;
            }
            
        }


        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;
    }
}
