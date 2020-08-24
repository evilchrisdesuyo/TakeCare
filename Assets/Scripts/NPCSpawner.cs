using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject Player1;
    public List<GameObject> spawnList;
    public float distanceToPlayer;
    public float spawnFrequency = 5f;
    public float currentSpawnTimer;
    public float spawnRange = 75;
    // Start is called before the first frame update
    void Start()
    {
        currentSpawnTimer = spawnFrequency;
        Player1 = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
       
        currentSpawnTimer -= 1 * Time.deltaTime;
        if (currentSpawnTimer < 0f)
        {
            currentSpawnTimer = spawnFrequency;
        }
            //check distance to player
            distanceToPlayer = Vector3.Distance(Player1.transform.position, transform.position);
        // if player distance is > startSpawningDistance
        if (distanceToPlayer > spawnRange && currentSpawnTimer <= 0.2f)
        {
            Debug.Log("Spawning NPC");
            spawnNPC();
        }
        //start spawning()
    }

   void spawnNPC()
    {
        
        Instantiate(spawnList[Random.Range(0, spawnList.Count)], this.transform.position, Quaternion.identity);
        currentSpawnTimer = spawnFrequency;
    }
    //start spawning
    //spawn NPC
    //wait 1 minute
    //repeat
}
