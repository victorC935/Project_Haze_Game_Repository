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

    public IEnumerator TrackPlayer()
    {
        while (playerInSight)
        {
            yield return new WaitForSeconds(2);
            playerLastKnown = player.transform.position; 
        }
    }
}
