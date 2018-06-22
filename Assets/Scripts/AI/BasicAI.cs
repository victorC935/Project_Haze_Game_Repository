using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour
{

    private enum EnemyState
    {
        Patrol,
        Alert,
        SearchFor,
        SpotPlayer,
        LostSight,
        Hunt,
        Attack,
    }

    private EnemyState behaviourState = new EnemyState();

    private void Awake()
    {
        behaviourState = EnemyState.Patrol;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void BehaviourDecider()
    {
        switch (behaviourState)
        {
            case EnemyState.Patrol:
                OnPatrol();

                break;
            case EnemyState.Alert:
                OnAlert();

                break;
            case EnemyState.SearchFor:
                OnSearchFor();

                break;
            case EnemyState.SpotPlayer:
                OnSpotPlayer();

                break;
            case EnemyState.LostSight:
                OnLostSight();

                break;
            case EnemyState.Hunt:
                OnHunt();

                break;
            case EnemyState.Attack:
                OnAlert();

                break;
            default:
                ();

                break;
        }

    }

    public void OnPatrol()
    {

    }

    public void OnAlert()
    {

    }

    public void OnSearchFor()
    {

    }

    public void OnSpotPlayer()
    {

    }

    public void OnLostSight()
    {

    }

    public void OnHunt()
    {

    }

    public void OnAttack()
    {

    }
}
