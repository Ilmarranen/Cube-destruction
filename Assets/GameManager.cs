using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameDataScriptable gameData;
    [SerializeField] TextMeshPro scoreText;
    [SerializeField] TextMeshProUGUI cooldownText;
    [SerializeField] List<GameObject> ballSprites;
    private int score;
    [SerializeField] private LayerMask groundLayer;
    public List<GameObject> cubes, balls;
    private float shootTimeElapsed, spawnTimeElapsed;

    public static Action onBallSpawn;

    // Start is called before the first frame update
    void Start()
    {
        SpawnManager.onObjectSpawned += OnObjectSpawned;
        BallController.onObjectDestroy += OnObjectDestroy;

        scoreText.transform.rotation = Camera.main.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Cubes destroyed: " + score;

        //Shooting cooldown
        shootTimeElapsed += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && balls.Count != 0 && shootTimeElapsed >= gameData.ballShootCD)
        {
            balls[0].SetActive(true);
            BallController ballController = balls[0].GetComponent<BallController>();
            ballController.SetTargetPosition(GetMouseWorldPosition());
            shootTimeElapsed = 0;
        }

        //Spawning cooldown
        //Only go on cooldown when we need the new ball
        if (balls.Count < gameData.ballsMax) spawnTimeElapsed += Time.deltaTime;
        cooldownText.text = Mathf.Max(0, gameData.ballSpawnCD - spawnTimeElapsed).ToString("F2");
        if (spawnTimeElapsed >= gameData.ballSpawnCD && balls.Count < gameData.ballsMax)
        {
            onBallSpawn?.Invoke();
            spawnTimeElapsed = 0;
        }
    } 

    private void OnObjectDestroy(GameObject objectDestroyed)
    {
        if (IsCube(objectDestroyed))
        {
            AddScore();
            cubes.Remove(objectDestroyed);
        }
        else
        {
            balls.Remove(objectDestroyed);
            ballSprites[balls.Count].SetActive(false);
        }
    }

    private void OnObjectSpawned(GameObject spawnedObject)
    {
        if (IsCube(spawnedObject))
        {
            cubes.Add(spawnedObject);
        }
        else
        {
            spawnedObject.SetActive(false);
            balls.Add(spawnedObject);
            ballSprites[balls.Count-1].SetActive(true);
        }
    }

    public void AddScore(int scoreToAdd = 1)
    {
        score += scoreToAdd;
    }

    public Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 100f, groundLayer))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public bool IsCube(GameObject objectToCheck)
    {
        return objectToCheck.GetComponent<CubeController>() != null;
    }
}
