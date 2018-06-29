using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour
{
    #region TempVariables

    bool playerSpotted = false;
    bool playerInSight = false;
    bool giveUpSearch = false;

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
    #region temp variables

    private int fieldOfView;
    private GameObject player;
    private GameObject entityPosition;
    private float spotDistance;

    #endregion

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
