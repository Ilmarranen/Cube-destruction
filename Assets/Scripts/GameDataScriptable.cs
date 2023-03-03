using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable objects/Game data", order = 1)]
public class GameDataScriptable : ScriptableObject
{
    //General
    public float leftBound, rightBound, upBound, downBound; 
    //Cube
    public float cubeYOffset = 0.5f;
    public float cubeBaseSpeed=3, cubeSpeedOffset=2, cubeRotationSpeed=2;
    public float speedChangeTimeLower = 3f, speedChangeTimeUpper = 6f;
    //Ball
    public float ballSpeed = 1, ballTrajectoryHeight = 20;
    public float ballExplosionRadius = 3;
    public float ballSpawnCD = 1, ballShootCD = 0.5f;
    //Spawn
    public int cubesRequired = 3, ballsMax = 5;
    public float minSpawnTime = 1, maxSpawnTime = 3;

    public Vector3 GenerateRandomPosition()
    {
        return new Vector3(Random.Range(leftBound, rightBound), cubeYOffset, Random.Range(downBound, upBound));
    }
}
