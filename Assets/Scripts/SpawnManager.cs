using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameDataScriptable gameData;
    private GameManager gameManager;
    [SerializeField] private GameObject cubeParent, ballParent;
    [SerializeField] private GameObject cubePrefab, ballPrefab;

    public static Action<GameObject> onObjectSpawned;

    // Start is called before the first frame update
    void Start()
    {

        gameManager = FindAnyObjectByType<GameManager>();

        for (int i = 0; i < gameData.cubesRequired; i++)
        {
            SpawnObject(cubePrefab, gameData.GenerateRandomPosition(), cubeParent);
        }
        for (int i = 0; i < gameData.ballsMax; i++)
        {
            SpawnObject(ballPrefab, ballParent.transform.position, ballParent);
        }

        GameManager.onBallSpawn += () => SpawnObject(ballPrefab, ballParent.transform.position, ballParent);
        BallController.onObjectDestroy += DestroyObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator SpawnCube()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(gameData.minSpawnTime,gameData.maxSpawnTime));
        SpawnObject(cubePrefab, gameData.GenerateRandomPosition(), cubeParent);
    }

    public GameObject SpawnObject(GameObject objectPrefab, Vector3 spawnPoint, GameObject objectParent)
    {
        GameObject spawnedObject = Instantiate(objectPrefab, spawnPoint, Quaternion.identity, objectParent.transform);
        onObjectSpawned?.Invoke(spawnedObject);
        return spawnedObject;
    }

    public void DestroyObject(GameObject objectToDestroy)
    {
        //This is cube
        if(gameManager.IsCube(objectToDestroy))
        {
            //We will need to create new after the delay
            StartCoroutine(SpawnCube());
        }
        Destroy(objectToDestroy);   
    }
}
