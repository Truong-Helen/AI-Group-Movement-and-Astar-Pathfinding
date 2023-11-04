using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFBoatSpawner : MonoBehaviour
{
    
    [SerializeField] Transform startSquare;
    [SerializeField] int spawnAmount = 5;
    List<Transform> boats;

    // Start is called before the first frame update
    void Start()
    {
        boats = new List<Transform>();
        SpawnBoats(spawnAmount);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach (Transform boat in boats)
            {
                Destroy(boat.gameObject);
            }
            SpawnBoats(spawnAmount);
        }
    }
    
    public void SpawnBoats(int amount)
    {

        float spawnXStart = startSquare.position.x - 7.5f;
        float spawnXEnd = startSquare.position.x + 7.5f;
        float spawnZStart = startSquare.position.z + 7.5f;
        float spawnZEnd = startSquare.position.z - 7.5f;

        for (int i = 0; i < amount; i++)
        {
            //Vector3 randomAngle = new Vector3(0f, Random.Range(1f, 360f), 0f);
            //Debug.Log(randomAngle);

            Entity381 ent = EntityMgr.inst.CreateEntity(EntityType.PilotVessel, new Vector3(Random.Range(spawnXStart, spawnXEnd), 0, Random.Range(spawnZStart, spawnZEnd)), Vector3.zero);
   
            //ent.gameObject.transform.parent = boatsParent;
            //ent.transform.Rotate(randomAngle);
        }
        DistanceMgr.inst.Initialize();
    }
    
    
    
}
