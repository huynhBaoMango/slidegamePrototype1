using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private unitManager unitPrefab;
    [SerializeField] private Transform unitParent;
    public float heart = 5;
    public float score = 0;
    public float unitTimeToMove;
    public float defaultUnitTimeToMove;

    public void SpawnUnit(int col)
    {
        Vector3 spawnPos = new Vector3(-1.6f + (col * 0.8f), -0.37f, 0f);
        unitManager newUnit = Instantiate(unitPrefab, spawnPos, Quaternion.identity, unitParent);
        newUnit.MoveTo(5.5f, unitTimeToMove);
    }

    public void TakeDamage(float damage)
    {
        heart -= damage;
    }

    public void StartGame()
    {
        heart = 5;
        score = 0;
        unitTimeToMove = defaultUnitTimeToMove;
    }

}
