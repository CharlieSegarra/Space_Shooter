using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject [] powerups;
   private bool _stopSpawning = false;

    // Start is called before the first frame update

    public void StartSpawining()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //spawn gameObject every 5 seconds
    // create a coroutine of type IMMnumerator -- Yield Events
    //While loop

    IEnumerator SpawnEnemyRoutine()
    {
        Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);

        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {   //newEnemy neatly organizes Hierarchy 
            GameObject newEnemy =  Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f); // waits 5 seconds
        }

        // then next line is called
        //while loope (infinite loop)
        //instantiate enemy prefab
        //yield wait for 5 seconds
    }
    IEnumerator SpawnPowerupRoutine()
    {
            yield return new WaitForSeconds(3.0f);
            //every 3-7 seoconds spawn in pwer up
            while(_stopSpawning == false)
            {
                Vector3 postToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
                int randomPowerUp = Random.Range(0,3);
                Instantiate (powerups[randomPowerUp], postToSpawn, Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(3,8));
            }

    }
    


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
