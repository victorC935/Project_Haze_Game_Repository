using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

    private Vector3 spawnTransform;
    private GameObject player;
    [SerializeField]
    private float SpawnDistance;

	void Start () {
        spawnTransform = gameObject.transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
	}
	

	void Update () {
		if(Vector3.Distance(player.transform.position, spawnTransform) <= SpawnDistance)
        {
            GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnerNPC>().IsActive = true;
        } else
        {
            GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnerNPC>().IsActive = false;
        }
	}
}
