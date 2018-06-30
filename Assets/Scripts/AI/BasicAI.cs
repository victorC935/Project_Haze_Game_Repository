using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicAI : MonoBehaviour
{
    #region TempVariables
    //Unsure if these variables are going to be transitioned to be permanant

    private int fieldOfView;
    private float spotDistance;
    private bool playerInSight;
    private int timeforSearch = 5; //time that the entity takes before giving up search for the other entity, when sight lost

    int approxStopDistance = 1; //the approximate distance check to be used when checking current and target distance
    public Vector3 playerLastKnown; //stores the last known player location

    private Vector3 patrolPointOne;
    private Vector3 patrolPointTwo;

    #endregion

    NavMeshAgent agent;
    GameObject destination;
    GameObject player;

    //Enum of Enemy behaviours for basic FSM
    private enum EnemyState
    {
        Patrol,
        Alert,
        SearchFor,
        SpotPlayer,
        Hunt,
        Attack,
    }
    private EnemyState behaviourState = new EnemyState();


    void Awake()
    {
        behaviourState = EnemyState.Patrol;

        player = GameObject.FindGameObjectWithTag("Player");
        if (!player)
        {
            Debug.LogWarning("No player is in scene");
        }
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        //prevents calling player tracking every frame
        InvokeRepeating("TrackPlayer", 1, 2);
    }


    void Update()
    {
        if (IsInSight())
            playerInSight = true;
        else
            playerInSight = false;

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
                Debug.Break();

                break;
        }

    }

    public void OnPatrol()
    {
        //Do patrol stuff

        if (playerInSight)
            behaviourState = EnemyState.SpotPlayer;
    }

    public void OnAlert()
    {
        //unsure as to what needs to be done in this state at the moment. 
    }

    public void OnSearchFor()
    {
        agent.SetDestination(playerLastKnown);
        //TODO when at pos, stop and look around

    }

    public void OnSpotPlayer()
    {
        agent.SetDestination(player.transform.position);
        StartCoroutine(TrackPlayer());
        //TODO Become alerted to player presence
        //TODO needs a player sight checker

        //Move to hunt player 
        //behaviourState = EnemyState.Hunt;


        if (!playerInSight)
        {
            behaviourState = EnemyState.SearchFor;
        }
    }

    public void OnHunt()
    {

    }

    public void OnAttack()
    {

    }

    bool IsInSight()
    {
        float currentDistance = Vector3.Distance(transform.position, player.transform.position);
        Vector3 fwdDir = transform.forward;
        float tempangle = Vector3.Angle(fwdDir, player.transform.position);
        if (tempangle <= (fieldOfView / 2) && currentDistance <= spotDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Tracks the players last known location. 
    /// Advised to run from start via repeat invoker or coroutine
    /// </summary>
    public void TrackPlayer()
    {
        while (playerInSight)
        {
            playerLastKnown = player.transform.position;
        }

        //NOTE I am not sure what this is doing, but it seems to work...
        //yield return new WaitUntil(() => playerInSight == true );
    }
}
