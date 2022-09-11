using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public class BossSpawner : MonoBehaviour
{
    [SerializeField]private GameObject enemyToSpawn;
    [SerializeField]private int numberOfEnemiesToSpawn = 3;
    [SerializeField] private float spawnDelay = 5f;
    [SerializeField] private float rangeToSpawn = 10f;
    [SerializeField] private bool autoSpawnEnemies = false;

    private List<GameObject> _managedEnemies = new List<GameObject>();

    private bool waitingToSpawn = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //only do this if we should auto spawn enemies when they are dead.
        if (!autoSpawnEnemies) return;
        
        for (int i = 0; i < _managedEnemies.Count; i++)
        {
            if (_managedEnemies[i] == null)
            {
                _managedEnemies.RemoveAt(i);
            }
        }

        if (_managedEnemies.Count == 0 && !waitingToSpawn)
        {
            StartCoroutine(SpawnDelayedEnemies());
        }
    }

    public void SpawnEnemies()
    {
        StartCoroutine(SpawnDelayedEnemies());
    }
    private IEnumerator SpawnDelayedEnemies()
    {
        waitingToSpawn = true;
        yield return new WaitForSeconds(spawnDelay);
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            Vector3 spawnPosition = Random.insideUnitSphere * rangeToSpawn;
            spawnPosition.y = 0;
            spawnPosition += transform.position;
            GameObject newEnemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
            _managedEnemies.Add(newEnemy);
        }

        waitingToSpawn = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,rangeToSpawn);
    }
}
