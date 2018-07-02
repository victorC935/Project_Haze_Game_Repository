using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateMachine : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector3 PatrolPoint(Vector3 startPosition, float patrolRange, bool isWalking)
    {
        if (!isWalking)
        {
            Vector3 randomPoint = startPosition + Random.insideUnitSphere * patrolRange;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, patrolRange, NavMesh.AllAreas))
            {
                Vector3 result = hit.position;
                
                return result;
            }
        }
        return Vector3.zero;
    }

}
