using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    SpawnPoint[] spawnPoins;

    private void Awake()
    {
        instance = this;
        spawnPoins = GetComponentsInChildren<SpawnPoint>();
    }

    public Transform GetSpawnpoint()
    {
        return spawnPoins[Random.Range(0, spawnPoins.Length)].transform;
    }
}
