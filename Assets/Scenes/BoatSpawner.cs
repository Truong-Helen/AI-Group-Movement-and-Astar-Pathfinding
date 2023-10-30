using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSpawner : MonoBehaviour
{
    [SerializeField] GameObject boatPrefab;
    [SerializeField] Transform boatsParent;
    [SerializeField] Transform startSquare;
    [SerializeField] int spawnAmount;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnBoats(boatPrefab, spawnAmount));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach (Transform boat in boatsParent.transform)
            {
                Destroy(boat.gameObject);
            }
            StartCoroutine(spawnBoats(boatPrefab, spawnAmount));
        }
    }
    
    IEnumerator spawnBoats(GameObject boatPrefab, int spawnAmount)
    {

        float spawnXStart = startSquare.position.x - 7.5f;
        float spawnXEnd = startSquare.position.x + 7.5f;
        float spawnZStart = startSquare.position.z + 7.5f;
        float spawnZEnd = startSquare.position.z - 7.5f;

        for (int i = 0; i < spawnAmount; i++)
        {
            Entity381 ent = EntityMgr.inst.CreateEntity(EntityType.PilotVessel, new Vector3(Random.Range(spawnXStart, spawnXEnd), 0, Random.Range(spawnZStart, spawnZEnd)), Vector3.zero);
            //GameObject boat = Instantiate(boatPrefab, new Vector3(Random.Range(spawnXStart, spawnXEnd), 0, Random.Range(spawnZStart, spawnZEnd)), Quaternion.identity);
            ent.gameObject.transform.parent = boatsParent.transform;
        }

        yield return null;

    }
    
}
