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

        BehaviourDecider();

    }

    /// <summary>
    /// This is used to attempt to control the entity behaviour through a basic state machine
    /// </summary>
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

    /// <summary>
    /// Action executed when player was unable to be found 
    /// </summary>
    public void OnPatrol()
    {
        while (!playerInSight)
        {
            //TODO remove magic number
            if (agent.remainingDistance < 0.5f)
                agent.SetDestination(PatrolPointSelector());

        }
        //TODO add some head movement, and looking to emulate searching.

        if (playerInSight)
            behaviourState = EnemyState.SpotPlayer;
    }

    /// <summary>
    /// Basic Patrol point selector to allow the entity to choice between the two patrol points
    /// </summary>
    /// <returns></returns>
    private Vector3 PatrolPointSelector()
    {
        if (agent.destination == patrolPointOne)
            return patrolPointTwo;
        else
            return patrolPointOne;
    }

    public void OnAlert()
    {
        //unsure as to what needs to be done in this state at the moment. 
    }

    /// <summary>
    /// When lost sight of the player, it searches the last known location. 
    /// <para>Also responsible for setting patrol points when lost sight of target and not found.</para>
    /// </summary>
    public void OnSearchFor()
    {

        patrolPointOne = transform.position;
        patrolPointTwo = player.transform.position;

        agent.SetDestination(playerLastKnown);
        //TODO when at pos, stop and look around

        if (Vector3.Distance(transform.position, playerLastKnown) <= 1)
        {

        }
        //TODO search for target code. look around for target


    }

    /// <summary>
    /// Is run when the player is spotted by an entity
    /// </summary>
    public void OnSpotPlayer()
    {
        agent.SetDestination(player.transform.position);

        //Move to hunt player 
        //behaviourState = EnemyState.Hunt;

        if (!IsInSight())
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

    /// <summary>
    /// line of sight check for the player gameobject from, the current entity
    /// </summary>
    /// <returns></returns>
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
