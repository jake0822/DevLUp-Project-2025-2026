using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Script_Spawners : MonoBehaviour
{
    public GameObject obstacle;
    private List<GameObject> spawners = new List<GameObject>();
    private float timer = 0;
    public float spawnTimer = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform spawner in transform)
        {
            spawners.Add(spawner.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > spawnTimer)
        {
            GameObject chosenSpawner = spawners[Random.Range(0, spawners.Count)];
            Instantiate(obstacle, chosenSpawner.transform.position, Quaternion.identity);
            timer = 0;
        }
    }
}
