using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private unitManager unitPrefab;
    [SerializeField] private Transform unitParent;

    public float defaultWaveDelay;
    public float defaultUnitTimeToMove;

    public float waveDelay;
    public float unitTimeToMove;
    public float percentDelay;
    public float percentTime;



    public void StartGame()
    {
        waveDelay = defaultUnitTimeToMove;
        unitTimeToMove = defaultUnitTimeToMove;
        InvokeRepeating("SpawnWave", 3f, waveDelay);
    }

    void SpawnWave()
    {
        Debug.Log("SPAWN QUÁI");
        int randomInt = Random.Range(0, 5);
        Vector3 spawnPos = new Vector3(-1.6f + (randomInt * 0.8f), 6, 0);
        unitManager newEnemy = Instantiate(unitPrefab, spawnPos, Quaternion.identity, unitParent);
        newEnemy.MoveTo(-0.37f, unitTimeToMove);
    }

    public void UpdateWaveDelay()
    {
        CancelInvoke("SpawnWave");
        InvokeRepeating("SpawnWave", 3f, waveDelay);
    }

    public void StopGame()
    {
        CancelInvoke("SpawnWave");
    }
}
