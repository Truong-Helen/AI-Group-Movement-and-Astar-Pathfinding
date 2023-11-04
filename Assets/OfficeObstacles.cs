using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeObstacles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
            EntityMgr.inst.entities.Add(child.gameObject.GetComponent<Entity381>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
