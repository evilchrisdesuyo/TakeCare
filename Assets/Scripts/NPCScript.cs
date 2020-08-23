﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCScript : MonoBehaviour
{
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
    public bool dead = false;
    public bool draining = false;
    public bool drained = false;
    public float CARE = 10;
    public List<Transform> allWalkTargets;
    public Transform currentWalkTarget;
    public enum behaviorState {Idle, Walking, Inspecting, Fleeing};
    public behaviorState currentBehavior;
    public NavMeshAgent agent;
    public float idleTimer;
    public float distanceToTarget;

    // Start is called before the first frame update
    void Start()
    {
        //Player1 = GetComponent.
        //currentBehavior = behaviorState.Walking;

        
    }

    // Update is called once per frame
    void Update()
    {
        //if idle timer = 0 
        //currentBehavior = behaviorState.Walking;
        if (NPCcontroller.velocity.z < 0.01f && currentBehavior == behaviorState.Walking)
       {
           // Debug.LogError("NPC STUCK AT" + transform.position);
        }
    //
        if (idleTimer > 0)
        {
            idleTimer -= 1 * Time.deltaTime;
        }

        if (currentBehavior == behaviorState.Idle && idleTimer <= 0)
        {
            currentBehavior = behaviorState.Walking;
            currentWalkTarget = allWalkTargets[Random.Range(0, allWalkTargets.Count)];
        }


        if (currentBehavior == behaviorState.Walking)
        {
            Vector3 target = new Vector3(currentWalkTarget.position.x, currentWalkTarget.position.y, currentWalkTarget.position.z);
            agent.SetDestination(target);

            distanceToTarget = Vector3.Distance(currentWalkTarget.position, transform.position);

            if (distanceToTarget < 2f)
            {
                currentBehavior = behaviorState.Idle;
                //MAKE THIS A RANDOM RANGE
                idleTimer = Random.Range(10, 30);
            }
        }

        if (distanceToPlayer > 100)
        {
            Destroy(gameObject);
        }

        if (CARE == 0)
        {
            drained = true;
        }

        //DO THE LIGHT LOGIC ONLY ONE NPC AT A TIME AND ONLY IF IT CAN BE DRAINED
        if (tooCloseMeter >= secsTilPassout)
        {
            dead = true;
        }

        distanceToPlayerConstant = Vector3.Distance(Player1.transform.position, transform.position);

        if (playerScript1.shootTarget == this.gameObject.transform)
        {
            distanceToPlayer = Vector3.Distance(Player1.transform.position, transform.position);
        }

        if (distanceToPlayer < tooFarDistance + 1)
        { 
            if (distanceToPlayer <= tooCloseDistance && playerScript1.shootTarget == this.gameObject.transform)
            {
                tooClose = true;
                draining = false;
                playerScript1.distanceLight.color = Color.red;//(Color.red / 1f) * Time.deltaTime;
                playerScript1.tooCloseText.enabled = true;
            }
            else
                if (distanceToPlayer > tooCloseDistance)
            {
                tooClose = false;
                playerScript1.tooCloseText.enabled = false;
            }

            if (tooClose && tooCloseMeter < secsTilPassout)
            {
                tooCloseMeter += 1f * Time.deltaTime;
            }
            else
                if (!tooClose && tooCloseMeter < 0f)
                {
                    tooCloseMeter -= 1f * Time.deltaTime;
                }

            if (distanceToPlayer >= tooFarDistance)
                {
                    tooFar = true;
                    draining = false;
                    tooCloseMeter = 0f;
                    playerScript1.distanceLight.color -= (Color.white);// / 2.0f) * Time.deltaTime;
                }
            else
            if (distanceToPlayer < tooFarDistance)
                {
                    tooFar = false;
                }
            if (!tooClose && !tooFar && playerScript1.shootTarget == this.gameObject.transform)
                {
                    playerScript1.distanceLight.color = Color.yellow;
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
                //assign that NPC to target
                playerScript1.shootTarget = this.transform;
                Debug.Log("Setting drain to true");
                //start draining
                draining = true;
                playerScript1.distanceLight.color = Color.green;

            }
    
        
        }
        if (Input.GetButtonUp("Fire1") || CARE <= 0 || distanceToPlayer >= tooFarDistance)
        {
            draining = false;
            
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
}
