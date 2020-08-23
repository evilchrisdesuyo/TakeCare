using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float timer = 800f;
    public bool timerActive;
    public float careInShip = 0f;
    public float careCapacity = 1000;
    public enum gameState {Intro, Gameplay}
    public gameState currentGameState;
    public GameObject Player1;
    public PlayerScript player1Script;
    public float distanceToPlayer;
    public bool dispensing;
    public float playerCare;
    // Start is called before the first frame update
    void Awake()
    {
        //player1 = findgamobjectwithtag("player")
        player1Script = GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(Player1.transform.position, this.transform.position);
        playerCare = player1Script.CAREstolen;

        if (timerActive)
        {
            timer -= 1 * Time.deltaTime;
            if (timer <= 0f)
            {
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
            playerCare -= 1 * Time.deltaTime;
            careInShip += 1 * Time.deltaTime;
        }
    }

    void timeUp()
    {
        if (careInShip < careCapacity)
        {
            //gameover
        }
        else if(careInShip < careCapacity)
        {
            //winner is you
        }
    }
}
