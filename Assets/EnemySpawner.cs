using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemy;

    public static GameObject newEnemy;

    public float spawnInterval;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy(spawnInterval, enemy));
    }

    // Update is called once per frame
    private IEnumerator SpawnEnemy(float i, GameObject e)
    {
        yield return new WaitForSeconds(i);

        while (true)
        {
            int yRand = Random.Range(-42, 42);
            int xRand = Random.Range(-30, 30);

            if ((xRand > 28 || xRand < -28) & (yRand > 15 || yRand < -15))
            {
                newEnemy = Instantiate(e, new Vector3(xRand, yRand, 0), Quaternion.identity);
                break;
            }
        }
        StartCoroutine(SpawnEnemy(i, e));
    }
} 
