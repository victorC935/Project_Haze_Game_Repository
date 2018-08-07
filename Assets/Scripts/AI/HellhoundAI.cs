using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellhoundAI : MonoBehaviour {

    [SerializeField]
    private float distractedHearingReduction; //How reduced hearing is when dog is eating meat
    [SerializeField]
    private float meatDetectionDistance; //Distance at which dog will be distratced by meat
    private bool distracted = false;

	void Start () {
		
	}
	

	void Update () {
		if(GameObject.FindGameObjectWithTag("Meat") != null)
        {
            if(Vector3.Distance(gameObject.transform.position, GameObject.FindGameObjectWithTag("Meat").transform.position) <= meatDetectionDistance)
            {
                distracted = true;
            }
        }

        if (distracted)
        {
            gameObject.GetComponent<BasicEnemyAI>().patrolPoints = new Vector3[1];
            gameObject.GetComponent<BasicEnemyAI>().patrolPoints[0] = GameObject.FindGameObjectWithTag("Meat").transform.position;
            gameObject.GetComponent<BasicEnemyAI>().hearingRadius = gameObject.GetComponent<BasicEnemyAI>().hearingRadius * distractedHearingReduction;
            gameObject.GetComponent<BasicEnemyAI>().viewDistance = 0.0f;
        }
	}
}
