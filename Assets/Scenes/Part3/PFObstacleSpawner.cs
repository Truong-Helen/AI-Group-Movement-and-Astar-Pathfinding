using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFObstacleSpawner : MonoBehaviour
{

    public GameObject PFObstaclePrefab;
    [SerializeField] Transform obstaclesParent;
    [SerializeField] Transform startSquare;
    [SerializeField] Transform endSquare;
    
    public int spawnAmount = 20;
    [SerializeField] float border = 10f;

    float spawnXStart;
    float spawnXEnd;
    float spawnZStart;
    float spawnZEnd;
    // Start is called before the first frame update
    void Start()
    {
        //spawnAmount = SceneMgr.inst.spawnAmount;
        //Debug.Log("Spawner: " + spawnAmount);
        spawnXStart = startSquare.position.x + border;
        spawnXEnd = endSquare.position.x - border;
        spawnZStart = startSquare.position.z;
        spawnZEnd = endSquare.position.z;

        //UnitTest1();
        SpawnPFObstacles(spawnAmount);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearObstacles();
            SpawnPFObstacles(spawnAmount);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            ClearObstacles();
            UnitTest1();
        }
    }

    public void ClearObstacles()
    {
        foreach (Transform sphere in obstaclesParent.transform)
        {
            Destroy(sphere.gameObject);
        }
    }
    public void SpawnPFObstacles(int spawnAmount)
    {

        
        for (int i = 0; i < spawnAmount; i++)
        {
            //GameObject obstacle = Instantiate(PFObstaclePrefab, new Vector3(Random.Range(spawnXStart, spawnXEnd), 0, Random.Range(spawnZStart,spawnZEnd)), Quaternion.identity);
            Entity381 ent = EntityMgr.inst.CreateEntity(EntityType.Obstacle,
                new Vector3(Random.Range(spawnXStart, spawnXEnd), 0, Random.Range(spawnZStart, spawnZEnd)),
                Vector3.zero);
            float randomScale = Random.Range(1f, 1.5f);
            ent.gameObject.transform.localScale *= randomScale;
            ent.mass *= randomScale;
            //obstacle.transform.parent = obstaclesParent;
        }

        //DistanceMgr.inst.Initialize();

    }

    public void UnitTest1()
    {
        EntityMgr.inst.CreateEntity(EntityType.Obstacle, new Vector3(100, 0 , 100), Vector3.zero);
        DistanceMgr.inst.Initialize();
    }
}
