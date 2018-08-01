using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyAI : MonoBehaviour
{
    [SerializeField]
    private int fieldOfView; //Degrees
    [SerializeField]
    private float viewDistance;
    [SerializeField]
    private float darknessViewReduction;
    [SerializeField]
    private float crouchViewReduction;
    [SerializeField]
    private float playerCrouchedHeight;

    private int enemyAlert = 0; //Scale from 0-X describing how alert enemy is to location, not sure how to implement this yet but I'll think of something

    private Vector3 playerLastKnown;
    [SerializeField]
    private float searchRadius;
    private int currentSearchObject = 0;
    private float searchStartTime = 0.0f;
    [SerializeField]
    private int timeForSearch = 5; //time that the entity takes before giving up search for the other entity, when sight lost

    [SerializeField]
    private float walkingSpeed;
    [SerializeField]
    private float runningSpeed;

    [SerializeField]
    private Vector3[] patrolPoints;
    private int activePatrolPoint;

    [SerializeField]
    private float attackDistance;

    private NavMeshAgent agent;
    private GameObject destination;
    private GameObject player;
    private Vector3 targetPosition;
    private Animator enemyAnim;

    //Enum of Enemy behaviours for basic FSM
    private enum EnemyState
    {
        Patrol,
        Investigate,
        SearchFor,
        Chase,
        Attack,
    }
    private EnemyState behaviourState = new EnemyState();
    void Awake()
    {

        targetPosition = gameObject.transform.position;
        behaviourState = EnemyState.Patrol;

        player = GameObject.FindGameObjectWithTag("Player");
        if (!player)
        {
            Debug.LogWarning("No player is in scene");
        }
        agent = GetComponent<NavMeshAgent>();
        enemyAnim = GetComponent<Animator>();

        agent.speed = walkingSpeed;
    }
    
    void Start()
    {
        //prevents calling player tracking every frame
        InvokeRepeating("TrackPlayer", 1, 0.1f);
    }


    void Update()
    {
        //Set target position and call behavior state machine each frame
        agent.destination = targetPosition;

        //Chase player from any state when seen
        if (IsInSight() && behaviourState != EnemyState.Attack)
        {
            searchStartTime = 0.0f;
            behaviourState = EnemyState.Chase;
        }

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
            case EnemyState.Investigate:
                OnInvestigate();

                break;
            case EnemyState.SearchFor:
                OnSearchFor();

                break;
            case EnemyState.Chase:
                OnChase();

                break;
            case EnemyState.Attack:
                OnAttack();

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
        enemyAlert = 0;
        agent.speed = walkingSpeed;
        enemyAnim.SetBool("Walking", true);
        enemyAnim.SetBool("Running", false);

        //fairly simple, just go to each location in sequence as specified in public "Patrol Points" array
        if (gameObject.transform.position.x == agent.destination.x && gameObject.transform.position.z == agent.destination.z)
        {
            activePatrolPoint++;
            if(activePatrolPoint == patrolPoints.Length)
            {
                activePatrolPoint = 0;
            }
        }

        targetPosition = patrolPoints[activePatrolPoint];
        //TODO add some head movement, and looking to emulate searching.

    }

    public void OnInvestigate()
    {
        //Set speed based on alert-ness
        if(enemyAlert <= 2)
        {
            agent.speed = walkingSpeed;
            enemyAnim.SetBool("Walking", true);
            enemyAnim.SetBool("Running", false);
        }
        else
        {
            agent.speed = runningSpeed;
            enemyAnim.SetBool("Running", true);
            enemyAnim.SetBool("Walking", false);
        }
        //Go to last known player location and start looking around
        targetPosition = playerLastKnown;
        if(gameObject.transform.position.x == agent.destination.x && gameObject.transform.position.z == agent.destination.z)
        {
            behaviourState = EnemyState.SearchFor;
        }
    }

    /// <summary>
    /// When lost sight of the player, it searches the last known location. 
    /// <para>Also responsible for setting patrol points when lost sight of target and not found.</para>
    /// </summary>
    public void OnSearchFor()
    {

        if (enemyAlert <= 2)
        {
            agent.speed = walkingSpeed;
            enemyAnim.SetBool("Walking", true);
            enemyAnim.SetBool("Running", false);
        }
        else
        {
            agent.speed = runningSpeed;
            enemyAnim.SetBool("Running", true);
            enemyAnim.SetBool("Walking", false);
        }

        //Set system time that the search started
        if (searchStartTime == 0.0f)
        {
            searchStartTime = Time.time;
        }
        //The following is pseudocode templating the enemy searching around objects the player can interact with / hide in (Closet, Door, etc.); will be implemented fully when this gameplay system is designed
        /*List<GameObject> allNearObjects = new List<GameObject>();
         * foreach object in scene
         * {
         *      if (Vector3.Distance(object, playerLastKnown) <= searchRadius)
         *      {
         *          allNearObjects.Add(object);
         *      }
         * }
         * 
         * targetPosition = allNearObjects.Get(currentSearchObject).transform.position;
         * if(gameObject.transform.position = targetPosition)
         * {
         *      InteractWith(allNearObjects.Get(currentSearchObject);
         *      currentSearchObject++;
         * }
         */

        //If not objects left to search, walk around randomly checking area
        if (true/*To be replaced with "currentSearchObject == allNearObjects.Length()" pending above code's implementation*/)
        {
            if (gameObject.transform.position.x == agent.destination.x && gameObject.transform.position.z == agent.destination.z)
            {
                targetPosition = new Vector3(UnityEngine.Random.Range(gameObject.transform.position.x - searchRadius, gameObject.transform.position.x + searchRadius), 0.0f, UnityEngine.Random.Range(gameObject.transform.position.y - searchRadius, gameObject.transform.position.y + searchRadius));
            }
        }

        //When specified time has elapsed, go back to patrolling
        if(Time.time - searchStartTime >= timeForSearch)
        {
            behaviourState = EnemyState.Patrol;
            searchStartTime = 0.0f;
        }

    }

    public void OnChase()
    {
        enemyAlert = 5;
        agent.speed = runningSpeed;
        enemyAnim.SetBool("Running", true);
        enemyAnim.SetBool("Walking", false);
        //Makes agent follow player
        targetPosition = player.transform.position;

        if (!IsInSight())
        {
            behaviourState = EnemyState.Investigate;
        }

        if(Vector3.Distance(gameObject.transform.position, player.transform.position) <= attackDistance)
        {
            behaviourState = EnemyState.Attack;
        }
        //fairly simple, could add more functionality later
    }


    public void OnAttack()
    {
        agent.speed = runningSpeed;
        if (!enemyAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            enemyAnim.SetTrigger("Attack");
        }

        targetPosition = player.transform.position;
        //Pseudocode for player health reduction, will likely be place in a OnCollision() function upon implementation
        //player.GetComponent<HealthScript>().health -= attackDamage;
    }

    /// <summary>
    /// line of sight check for the player gameobject from, the current entity
    /// </summary>
    bool IsInSight()
    {
        RaycastHit hit;
        //Check if player is in enemy FOV, if player is close enough to see, and if a direct line of sight is established
        float tempAngle = Vector3.Angle(gameObject.transform.forward, player.transform.position - gameObject.transform.position);
        if ((!player.GetComponent<PlayerMovement>().crouched && tempAngle <= (fieldOfView / 2) && Physics.Raycast(new Ray(gameObject.transform.position, player.transform.position - gameObject.transform.position), out hit, Mathf.Infinity)) || (player.GetComponent<PlayerMovement>().crouched && tempAngle <= (fieldOfView / 2) && Physics.Raycast(new Ray(gameObject.transform.position, new Vector3(player.transform.position.x, player.transform.position.y + playerCrouchedHeight, player.transform.position.z) - gameObject.transform.position), out hit, Mathf.Infinity)))
        {
            //Check if player is in dark and reduce view distance on enemy
            float activeViewDistance = viewDistance;
            if(player.GetComponent<PlayerMovement>().isInDark){
                   activeViewDistance -= darknessViewReduction;
             }
            //Check if player is crouched and reduce view distance of enemy
            if (player.GetComponent<PlayerMovement>().crouched)
            {
                activeViewDistance -= crouchViewReduction;
            }
            //Return true if player is within view distance
            if (hit.distance <= viewDistance && hit.collider.gameObject.tag == "Player")
            {
                return true;
            }
        }
        player.GetComponent<MeshRenderer>().material.color = Color.white;
        return false;
    }

    /// <summary>
    /// Tracks the players last known location. 
    /// Advised to run from start via repeat invoker or coroutine
    /// </summary>
    public void TrackPlayer()
    {
        if (IsInSight())
        {
            playerLastKnown = player.transform.position;
        }
    }
}
