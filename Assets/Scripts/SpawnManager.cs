using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject spawnPrefab;

    void Update()
    {
        if (!SwipeController.isActive)
        {
            SwipeController.isActive = true;
            StartCoroutine(SpawnBall());
        }
    }

    IEnumerator SpawnBall()
    {
        yield return new WaitForSeconds(2);
        Instantiate(spawnPrefab);
    }
}
