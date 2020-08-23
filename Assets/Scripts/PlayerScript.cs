using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public float movementSpeed = 6f;
    public float turnSmoothing = 0.1f;
    public float turnSmoothingVelocity;
    float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded;
    public float jumpHeight = 3f;
    public Transform carry;
    public Transform shootTarget;
    public float CAREstolen;
    public Light distanceLight;
    public Text playerCareText;
    public Text tooCloseText;

    // Start is called before the first frame update
    void Start()
    {
        //get character controller
        Cursor.lockState = CursorLockMode.Locked;
        distanceLight.color -= (Color.white / 2.0f) * Time.deltaTime;
        tooCloseText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothingVelocity, turnSmoothing);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * movementSpeed * Time.deltaTime);
        }
        

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        playerCareText.text = CAREstolen.ToString();
        //carry.transform.positoin = carry positoin
        //if dropkey
        //carry.rb = enabled
        //carry =null

        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }

        findClosestNPC();
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
        float distanceToClosestNPC = Mathf.Infinity;
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
