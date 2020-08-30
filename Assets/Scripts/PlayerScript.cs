using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    // Animation Variables
    public Animator animator;
    public float speed;
    Vector3 lastPosition = Vector3.zero;
    public bool isExtracting;


    public CharacterController controller;
    public GameObject alien;
    public Transform cam;
    public float movementSpeed = 6f;
    public float averageSpeed = 6f;
    public float turnSmoothing = 0.1f;
    public float turnSmoothingVelocity;
    float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    public bool isGrounded;
    public float jumpHeight = 3f;
    public Transform carry;
    public Transform shootTarget;
    public float CAREstolen;
    public Light distanceLight;
    public Text playerCareText;
    public Text tooCloseText;
    public bool walkInput = false; 
    public bool goldieLocks = false;
    public float distanceToClosestNPC;
    private bool jumpBool;
    //public bool Sprint;

    // Start is called before the first frame update
    void Start()
    {
        // playerCareText = ----------------------------------------------------
        //tooCloseText = ---------------------------------------------------

        //get character controller
        Cursor.lockState = CursorLockMode.Locked;
        distanceLight.enabled = false;//.color -= (Color.white / 2.0f) * Time.deltaTime;
        tooCloseText.enabled = false;

        // Get aliens animator
        animator = alien.gameObject.GetComponent<Animator>();

    }

    private void FixedUpdate() {
        speed = (transform.position - lastPosition).magnitude;
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Sprint)
        {
            movementSpeed = movementSpeed + 5f;
        }
        else if (!Sprint)
        {
            movementSpeed = averageSpeed;
        }*/

        if (CAREstolen < 0)
        {
            CAREstolen = 0;
        }

        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //isGrounded = true;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            walkInput = true;
        }
        else
        {
            walkInput = false;
        }

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothingVelocity, turnSmoothing);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * movementSpeed * Time.deltaTime);

            
        }
        /*
        if (Input.GetButtonDown("Sprint") && isGrounded)
        {
            Sprint = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            Sprint = false;
        }*/

            if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpBool = true;
        }
        if (!isGrounded)
        {
            jumpBool = false;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        playerCareText.text = CAREstolen.ToString();
        //carry.transform.positoin = carry positoin
        //if dropkey
        //carry.rb = enabled
        //carry =null

       

        findClosestNPC();

        //float playerSpeedInput = vertical * horizontal;

        //float playerSpeedInputX = ;
        // Animation updates
        animator.SetBool("isGround", isGrounded);
        animator.SetBool("Jump", jumpBool);
        //animator.SetFloat("Speed", playerSpeedInput);
        animator.SetFloat("Speed", (Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal"))));
        animator.SetBool("Extracting", isExtracting);


        //shootTarget.gameobject.material.emmissive = glowing
    }
    //on collision enter
    // if others tag is item
    // carry = other
    //other. RB = disabled

     //if others tag is enemy
    // health -=1
    //send flying

    void findClosestNPC()
    {
         distanceToClosestNPC = Mathf.Infinity;
        NPCScript closestNPC = null;
        NPCScript[] allNPCs = GameObject.FindObjectsOfType<NPCScript>();

        foreach (NPCScript currentNPC in allNPCs)
        {
            float distanceToNPC = (currentNPC.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToNPC < distanceToClosestNPC)
            {
                distanceToClosestNPC = distanceToNPC;
                closestNPC = currentNPC;
                shootTarget = closestNPC.transform;
            }
        }

        Debug.DrawLine(this.transform.position, closestNPC.transform.position);
    }

    

}
