using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Astar : MonoBehaviour
{
    Node[,] nodes;
    int rows, cols;
    float xMin, xMax, zMin, zMax;
    int nodeCountX, nodeCountZ;

    // Start is called before the first frame update
    void Start()
    {
        nodes = new Node[rows, cols];
        // code from slides
        
        for (int i = 0; i < nodeCountX; i++)
        {
            for (int j = 0; j < nodeCountZ; j++)
            {
                nodes[i,j] = new Node();
                float xPos = xMin + i * (xMax - xMin) / (nodeCountX - 1);
                float zPos = zMin + j * (zMax - zMin) / (nodeCountZ - 1);
                nodes[i,j].pos = new Vector3(xPos, 0, zPos);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
