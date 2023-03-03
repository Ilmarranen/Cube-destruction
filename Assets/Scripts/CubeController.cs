using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    [SerializeField] GameDataScriptable gameData;
    private Vector3 targetPosition, targetDirection;
    private float speed, speedChangeTime, timeElapsed = 0;

    void Start()
    {
        targetPosition = gameData.GenerateRandomPosition();
        speed = gameData.cubeBaseSpeed;
        speedChangeTime = GetRandomSpeedChangeTime();
    }

    void FixedUpdate()
    {
        //First turn, only then move toward the target, or it looks too chaotical
        targetDirection = (targetPosition - transform.position).normalized;
        if(Vector3.Distance(targetDirection,transform.forward) < 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, gameData.cubeRotationSpeed * Time.fixedDeltaTime, 0));
        }

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            targetPosition = gameData.GenerateRandomPosition();
        }

        timeElapsed += Time.fixedDeltaTime;
        if (timeElapsed >= speedChangeTime)
        {
            ChangeSpeed();
            timeElapsed = 0;
            speedChangeTime = GetRandomSpeedChangeTime();
        }

    }



    private float GetRandomSpeedChangeTime()
    {
        return Random.Range(gameData.speedChangeTimeLower, gameData.speedChangeTimeUpper);
    }

    private void ChangeSpeed()
    {
        if (speed == gameData.cubeBaseSpeed)
        {
            speed += Random.Range(0,gameData.cubeSpeedOffset);
        }
        else
        {
            speed = gameData.cubeBaseSpeed;
        }
    }
}
