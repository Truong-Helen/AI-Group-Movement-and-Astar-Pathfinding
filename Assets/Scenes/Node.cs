using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 worldPosition;
    public int gridPositionX;
    public int gridPositionZ;

    public Node parentNode = null;

    public float hCost;
    public float gCost;
    public float fCost;

    // Start is called before the first frame update
    void Start()
    {
        hCost = 0;
        gCost = float.MaxValue;
        fCost = hCost + gCost;
        parentNode = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
