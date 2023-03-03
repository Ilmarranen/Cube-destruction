using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] GameDataScriptable gameData;
    private Vector3 startPosition, targetPosition;
    private float time=0;
    [SerializeField] private LayerMask groundLayer;
    public static Action<GameObject> onObjectDestroy;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(targetPosition != Vector3.zero)
        {
            transform.position = Parabola(startPosition, targetPosition, gameData.ballTrajectoryHeight, gameData.ballSpeed * time);
            time += Time.deltaTime;
        }  
    }

    public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        float y = -1*height*t*t + height*t;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, y, mid.z); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, gameData.ballExplosionRadius);
            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Cube")) onObjectDestroy?.Invoke(col.gameObject);
            }
            onObjectDestroy?.Invoke(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, gameData.ballExplosionRadius);
    }

    public void SetTargetPosition(Vector3 target)
    {
        targetPosition = target;
    }
}
