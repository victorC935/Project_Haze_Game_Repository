using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicAI : MonoBehaviour
{
    #region TempVariables

    private int fieldOfView;
    private float spotDistance;
    private bool playerInSight;

    public Vector3 playerLastKnown;

    #endregion

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

    NavMeshAgent agent;
    GameObject destination;
    GameObject player;

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
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(IsInSight())
        {
            behaviourState = EnemyState.SpotPlayer;
        }
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
        //Do Patrol stuff


        if (playerInSight)
            behaviourState = EnemyState.SpotPlayer;
    }

    public void OnAlert()
    {
        //unsure as to what needs to be done in this state at the moment. 
    }

    public void OnSearchFor()
    {
        //TODO Search for player
        if (!giveUpSearch)
        {
            //search for player based on co-ords
        }

    }

    public void OnSpotPlayer()
    {
        //TODO Become alerted to player presence
        //TODO needs a player sight checker

        //Move to hunt player 
        behaviourState = EnemyState.Hunt;
    }

    public void OnLostSight()
    {
        //TODO hunt down player and search last known location


        if (!playerInSight && !playerSpotted)
        {
            behaviourState = EnemyState.SearchFor;
            //TODO Pass through the last known co-ords for player (approx) for AI to investigate

        }
    }

    bool IsInSight()
    {
        float currentDistance = Vector3.Distance(entityPosition.transform.position, player.transform.position);
        Vector3 fwdDir = entityPosition.transform.forward;
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
    public void OnHunt()
    {

    }

    public void OnAttack()
    {

    }
}
