﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class NPCScript : MonoBehaviour
{
    // NPC Animations
    public Animator animator;
    public float speed;
    Vector3 lastPosition = Vector3.zero;


    public CharacterController NPCcontroller;
    public GameObject Player1;
    public PlayerScript playerScript1;
    public float distanceToPlayer;
    public float distanceToPlayerConstant;
    public bool spottedPlayer = false;
    public float tooCloseDistance = 3f;
    public float tooFarDistance = 6f;
    bool tooClose;
    bool tooFar;
    public float secsTilPassout = 10f;
    public float tooCloseMeter = 0f;

    public bool draining = false;
    public bool drained = false;
    public float CARE = 10;
    public List<GameObject> allWalkTargets;
    public GameObject currentWalkTarget;
    public List<GameObject> fleeTargets;
    public GameObject currentFleeTarget;
    public enum behaviorState {Idle, Walking, Inspecting, Fleeing, debug};
    public behaviorState currentBehavior;
    public NavMeshAgent agent;
    public float idleTimer;
    public float distanceToTarget;

    public float FOV = 90f;
    public bool seesPlayer;
    private SphereCollider col;
    public Transform head;
    public Vector3 lastPlayerLocation;
    public bool hostile = false;
    public enum SightSensitivity { STRICT, LOOSE};
    public SightSensitivity Sensitivtee = SightSensitivity.STRICT;
    public bool chasingPlayer = false;
    public LineRenderer sightLine;
    public bool useIdleTimer = true;
    public float cullingDistance = 150f;
    public GameController currentGameState;
    public AudioSource sourceOfAudio;
    float screamTimer;
    public AudioClip[] screamSoundArray;
    AudioClip lastClip;
    // Start is called before the first frame update
    private void Awake()
    {
        //Player1 = GetComponent.
        //currentState = GameObject.Find("Saucer(GameController)").GameController.;
        Player1 = GameObject.FindGameObjectWithTag("Player");
        playerScript1 = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        allWalkTargets = new List<GameObject>();
        allWalkTargets.AddRange(GameObject.FindGameObjectsWithTag("WalkTarget"));
        fleeTargets = new List<GameObject>();
        fleeTargets.AddRange(GameObject.FindGameObjectsWithTag("FleeTarget"));

        // Get NPC Animator
        animator = this.gameObject.GetComponent<Animator>();
        
        col = GetComponent<SphereCollider>();
        currentFleeTarget = fleeTargets[Random.Range(0, fleeTargets.Count)];

        if (currentWalkTarget == null)
        {
            currentWalkTarget = allWalkTargets[Random.Range(0, allWalkTargets.Count)];
        }
    }

    private void FixedUpdate() {
        speed = (transform.position - lastPosition).magnitude;
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Animation updates
        animator.SetFloat("Speed", speed);
        animator.SetBool("isDrained", drained);
        animator.SetBool("isScared", currentBehavior.Equals(behaviorState.Fleeing));



        if (seesPlayer)
        {
            sightLine.enabled = true;
            //render line
            Debug.LogError("enemy sees you!");
            sightLine.SetPosition(0, new Vector3(head.position.x, head.position.y, head.position.z));
            sightLine.SetPosition(1, new Vector3(playerScript1.gameObject.transform.position.x, playerScript1.gameObject.transform.position.y, playerScript1.gameObject.transform.position.z));

            lastPlayerLocation = new Vector3(Player1.transform.position.x, Player1.transform.position.y, Player1.transform.position.z);
            if (hostile)
            {
                 chasingPlayer = true;
            }
            else
                if (!hostile)
            {
                currentBehavior = behaviorState.Fleeing;
                idleTimer = 0;

                    screamTimer += Time.deltaTime;
                if (screamTimer > 5)
                {
                    sourceOfAudio.PlayOneShot(randomScream());
                    screamTimer = 0;
                }
            }
        }
        else if (!seesPlayer)
        {
            sightLine.enabled = false;
        }

        if (distanceToPlayer >= 20 && !seesPlayer || distanceToPlayer > 50)
        {
            chasingPlayer = false;
        }

        //agent moves towards 
        if(chasingPlayer)
        {
            Debug.LogError("ENEMY IS CHASING PLAYER");
            agent.SetDestination(playerScript1.transform.position);
            agent.speed = 5;
        }

        // lose sight of the player go to his last known location
        if (chasingPlayer && !seesPlayer)
        {
            Debug.LogError("ENEMY IS CHASING PLAYER");
            agent.SetDestination(lastPlayerLocation);
            agent.speed = 5;
        }

        //gives up chasing the player
        if (chasingPlayer && distanceToPlayerConstant > 20 && !seesPlayer)
        {
            chasingPlayer = false;
            currentBehavior = behaviorState.Walking;
        }

        //if agent catches you go into failure state
        if (chasingPlayer && distanceToPlayerConstant < 1)//distanceToPlayer < 0.5f)
        {
            SceneManager.LoadScene("Failure-Captured");
        }

        if (chasingPlayer && distanceToPlayerConstant > 10)//distanceToPlayer < 0.5f)
        {
            currentBehavior = behaviorState.Walking;
        }
        if (currentBehavior == behaviorState.Fleeing && currentBehavior != behaviorState.Walking)
        {
            currentWalkTarget = null;
           // agent.prio
            Vector3 target = new Vector3(currentFleeTarget.transform.position.x, currentFleeTarget.transform.position.y, currentFleeTarget.transform.position.z);
            agent.speed = 15;
            agent.SetDestination(target);
        }
        //currentBehavior = behaviorState.Walking;
        if (NPCcontroller.velocity.z < 0.01f && currentBehavior == behaviorState.Walking)
        {
            // Debug.LogError("NPC STUCK AT" + transform.position);
        }
        //
        if (useIdleTimer && idleTimer > 0)
        {
            idleTimer -= 1 * Time.deltaTime;
        }
        else if (useIdleTimer)
        {
            idleTimer = 5;
        }

        if (useIdleTimer && currentBehavior == behaviorState.Idle && idleTimer <= 0)
        {
            currentBehavior = behaviorState.Walking;
            currentWalkTarget = allWalkTargets[Random.Range(0, allWalkTargets.Count)];
        }


        if (currentBehavior == behaviorState.Walking && currentBehavior != behaviorState.Fleeing)
        {
            Vector3 target = new Vector3(currentWalkTarget.transform.position.x, currentWalkTarget.transform.position.y, currentWalkTarget.transform.position.z);
            //agent.SetDestination(target);
             if (!hostile && !drained && useIdleTimer)
             {
                 agent.SetDestination(target);
             }
             else if (drained)
             {
                 this.enabled = false;
                Destroy(agent);
             }
                 if (hostile && !chasingPlayer)
             {
                agent.speed = 2.5f;
                agent.SetDestination(target);
             }


            distanceToTarget = Vector3.Distance(currentWalkTarget.transform.position, transform.position);

            if (distanceToTarget < 5f)
            {
                currentBehavior = behaviorState.Idle;
                //MAKE THIS A RANDOM RANGE
                idleTimer = Random.Range(10, 20);
            }
        }

        if ( distanceToPlayerConstant > cullingDistance)
        {
            Destroy(this.gameObject);
        }

        if (CARE <= 0)
        {
            drained = true;
        }

        //DO THE LIGHT LOGIC ONLY ONE NPC AT A TIME AND ONLY IF IT CAN BE DRAINED
        if (tooCloseMeter >= secsTilPassout)
        {
            Debug.Log("BOOM");
            Destroy(this.gameObject);
           
        }

        distanceToPlayerConstant = Vector3.Distance(Player1.transform.position, this.gameObject.transform.position);

        if (playerScript1.shootTarget == this.gameObject.transform)
        {
            distanceToPlayer = Vector3.Distance(Player1.transform.position, this.gameObject.transform.position);
        }

        if (distanceToPlayer < tooFarDistance + 1)
        {
            if (distanceToPlayer <= tooCloseDistance && playerScript1.shootTarget == this.gameObject.transform)
            {
                tooClose = true;
                draining = false;
                playerScript1.distanceLight.enabled = true;
                playerScript1.distanceLight.intensity = 1;
                playerScript1.distanceLight.color = Color.red;//(Color.red / 1f) * Time.deltaTime;
                playerScript1.goldieLocks = false;
                playerScript1.tooCloseText.enabled = true;
                playerScript1.isExtracting = draining;
            }
            else
                if (distanceToPlayer > tooCloseDistance)
            {
                tooClose = false;
                playerScript1.tooCloseText.enabled = false;
            }

            if (!hostile && tooClose && tooCloseMeter < secsTilPassout)
            {
                tooCloseMeter += 1f * Time.deltaTime;
            }
            else
                if (!tooClose && tooCloseMeter < 0f)
            {
                tooCloseMeter -= 1f * Time.deltaTime;
            }

            if (distanceToPlayer >= tooFarDistance || CARE <= 0)
            {
                tooFar = true;
                draining = false;
                tooCloseMeter = 0f;
                //playerScript1.distanceLight.color -= (Color.white);// / 2.0f) * Time.deltaTime;
                playerScript1.distanceLight.enabled = true;
                playerScript1.distanceLight.color = Color.white;
                playerScript1.distanceLight.intensity = 0.5f;
                playerScript1.isExtracting = draining;
            }
            else
            if (distanceToPlayer < tooFarDistance)
            {
                tooFar = false;
            }
            if (!tooClose && !tooFar && playerScript1.shootTarget == this.gameObject.transform && CARE > 0)
            {
                playerScript1.distanceLight.enabled = true;
                playerScript1.distanceLight.intensity = 1;
                playerScript1.distanceLight.color = Color.yellow;
               playerScript1.goldieLocks = true;
            }
        }

        if (draining)
        {
            CARE -= 1 * Time.deltaTime;
            //player score += scoreamount
            playerScript1.CAREstolen += 1 * Time.deltaTime;

        }
        //select runpoint at random
        // AI state = calm
        //sees player
        //AI state = run
        //if AI state = run
        //navigate to run point

        if (Input.GetButton("Fire1"))
        {
            Debug.Log("Player trying to drain");

            // if not too close and not too far and //not too far from cube
            if (!tooClose && CARE > 0 && distanceToPlayerConstant < tooFarDistance)
            {
                playerScript1.distanceLight.enabled = true;
                //assign that NPC to target
                playerScript1.shootTarget = this.transform;
                Debug.Log("Setting drain to true");
                //start draining
                draining = true;
                playerScript1.distanceLight.intensity = 1;
                playerScript1.distanceLight.color = Color.green;
                playerScript1.isExtracting = draining;
                playerScript1.goldieLocks = false;

            }


        }
        if (Input.GetButtonUp("Fire1") || CARE <= 0 || distanceToPlayer >= tooFarDistance)
        {
            draining = false;
            playerScript1.isExtracting = draining;

        }
        // void OnMouseDown()
        //   {
        //assign that NPC to target
        //    playerScript1.shootTarget = this.transform;
        // if not too close and not too far and //not too far from cube
        //    if (!tooClose && !tooFar)
        //   {
        //       //start draining
        //      draining = true;
        //  }


        //set rotation of model look at target
        // cinemachine lookat = shoottarget
    
        }

    bool InFOV()
    {
        Vector3 DirToTarget = playerScript1.transform.position - head.position;

        float Angle = Vector3.Angle(head.forward, DirToTarget);

        if (Angle <= FOV)
            return true;

        return false;
    }

    bool ClearLineOfSight()
    {
        RaycastHit Info;
        if (Physics.Raycast(head.transform.position, (playerScript1.transform.position - head.transform.position).normalized, out Info, col.radius))
        {
            if (Info.transform.CompareTag("Player"))
                return true;
        }
        return false;
    }

    void UpdateSight()
    {
        switch (Sensitivtee)
        {
            case SightSensitivity.STRICT:
                seesPlayer = InFOV() && ClearLineOfSight();
                break;
            case SightSensitivity.LOOSE:
                seesPlayer = InFOV() || ClearLineOfSight();
                break;
        }
    }

     void OnTriggerStay(Collider other)
    {
        UpdateSight();

        if (seesPlayer)
            lastPlayerLocation = playerScript1.transform.position;
    }

    void OnTriggerExit(Collider other)
    {

        if (!other.CompareTag("Player")) return;
        seesPlayer = false;
    }

    AudioClip randomScream()
    {
        int attempts = 3;
        AudioClip newClip = screamSoundArray[Random.Range(0, screamSoundArray.Length - 1)];

        while (newClip == lastClip && attempts > 0)
        {
            newClip = screamSoundArray[Random.Range(0, screamSoundArray.Length - 1)];
            attempts--;
        }

        lastClip = newClip;
        return newClip;
    }
    /*
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == Player1)
        {
            // seesPlayer = false;
            Debug.Log("Player entered sphere");

            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < FOV * 0.5f)
            {
                Debug.Log("Player within Angle");

                RaycastHit hit;

                

                if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius))
                {
                    Debug.Log("Raycast successful");

                    if (hit.collider.gameObject == Player1)
                    {
                        seesPlayer = true;
                        Debug.Log("NPC SEES THE PLAYER");

                    }
                }
            }
        }

    }*/
}
