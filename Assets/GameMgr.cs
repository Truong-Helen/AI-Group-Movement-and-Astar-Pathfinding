﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    public static GameMgr inst;

    public Transform startArea;
    private void Awake()
    {
        inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = Vector3.zero;
        /*
        foreach(GameObject go in EntityMgr.inst.entityPrefabs) {
            Entity381 ent = EntityMgr.inst.CreateEntity(go.GetComponent<Entity381>().entityType, position, Vector3.zero);
            position.x += 200;
        }
        */
    }

    public Vector3 position;
    public float spread = 20;
    public float colNum = 10;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F12)) {
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 10; j++) {
                    Entity381 ent = EntityMgr.inst.CreateEntity(EntityType.PilotVessel, position, Vector3.zero);
                    position.z += spread;
                }
                position.x += spread;
                position.z = 0;
            }
            DistanceMgr.inst.Initialize();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Spawn10Entities(EntityType.PilotVessel);
        }
        
    }


    public void Spawn10Entities(EntityType entityType)
    {
        Vector3 startPosition = startArea.position - (Vector3.right * 10);
        Entity381 ent = EntityMgr.inst.CreateEntity(entityType, startPosition, Vector3.zero);
        DistanceMgr.inst.Initialize();
        return;
        for (int i = 0; i < 10; i++)
        {
            position.x += 1;
        }

    }
}
