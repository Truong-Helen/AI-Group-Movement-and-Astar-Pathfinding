using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] Transform obstaclesParent;
    [SerializeField] Transform startSquare;
    [SerializeField] Transform endSquare;
    [SerializeField] int spawnAmount;
    [SerializeField] float border = 10f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnObstacles(obstaclePrefab, spawnAmount));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach (Transform sphere in obstaclesParent.transform)
            {
                Destroy(sphere.gameObject);
            }
            StartCoroutine(spawnObstacles(obstaclePrefab, spawnAmount));
        }
    }

    IEnumerator spawnObstacles(GameObject obstaclePrefab, int spawnAmount)
    {

        float spawnXStart = startSquare.position.x + border;
        float spawnXEnd = endSquare.position.x - border;
        float spawnZStart = startSquare.position.z;
        float spawnZEnd = endSquare.position.z;
        for (int i = 0; i < spawnAmount; i++)
        {
            GameObject obstacle = Instantiate(obstaclePrefab, new Vector3(Random.Range(spawnXStart, spawnXEnd), 0, Random.Range(spawnZStart,spawnZEnd)), Quaternion.identity);
            obstacle.transform.localScale *= Random.Range(1f, 1.5f);
            obstacle.transform.parent = obstaclesParent;
        }

        yield return null;

    }
}
