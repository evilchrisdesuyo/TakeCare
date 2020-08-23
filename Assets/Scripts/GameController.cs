﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public float timer = 800f;
    public bool timerActive = true;
    public float careInShip = 0f;
    public float careCapacity = 1000;
    public enum gameState {Intro, Gameplay}
    public gameState currentGameState;
    public GameObject Player1;
    public PlayerScript player1Script;
    public float distanceToPlayer;
    public bool dispensing;
    public float playerCare;
    public Collider dispensorVolume;
    public Text timerText;
    public Text careInShipText;

    // Start is called before the first frame update
    void Awake()
    {
        //player1 = findgamobjectwithtag("player")
        player1Script = Player1.GetComponent<PlayerScript>();
        if (currentGameState == gameState.Intro)
        {
            introLogic();

        }
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = timer.ToString();
        careInShipText.text = careInShip + "/" + careCapacity ; //careInShip.ToString("/" + careCapacity);

        distanceToPlayer = Vector3.Distance(Player1.transform.position, this.transform.position);
        playerCare = player1Script.CAREstolen;

        if (timerActive)
        {
            timer -= 1 * Time.deltaTime;
            if (timer <= 0)
            {
                Debug.Log("TIME UP");

                timer = 0;
                timeUp();

            }
        }


      

        if (Input.GetButton("Fire2"))
        {
           

            // if not too close and not too far and //not too far from cube
            if (distanceToPlayer < 5 && playerCare > 0f)
            {
                //assign that NPC to target
                //playerScript1.shootTarget = this.transform;
                Debug.Log("Dispencing CARE");
                //start draining
                dispensing = true;
                //playerScript1.distanceLight.color = Color.green;

            }


        }
        if (Input.GetButtonUp("Fire2") || playerCare <= 0 || distanceToPlayer >= 5f)
        {
            dispensing = false;

        }

        if (dispensing)
        {
            player1Script.CAREstolen -= 5 * Time.deltaTime;
            careInShip += 5 * Time.deltaTime;
        }

         //timeUp()
       // {
            
       // }
    }

    private void OnCollisionStay(Collision collision)
    {
        //check if player is in collision on spotlight
    }

    void introLogic()
    {
        //desaturate
        //move cam
        //move ship
        //play video?
        // hold until cine is done
        //fade back to saturate
        //move to dynamic angle
        //move ship to groud
        // change angles
        //spawn player
        //hold for a few seconds
        // correct cam
        //currentGameState = gameState.Gameplay;
    }
        void timeUp()
    {
        if (careInShip < careCapacity)
        {
            Debug.Log("Running fail logic");
            //gameover
            SceneManager.LoadScene("Failure-NotEnough");
        }
        if (careInShip >= careCapacity)
        {
            Debug.Log("Running win logic");
            //winner is you
            SceneManager.LoadScene("Success");
        }
    }


}