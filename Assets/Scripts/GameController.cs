using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
     float timer;
    public float timerMax = 800f;
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
    public Animator anim;
    public string theLevel;
    public Text tutorial;
   public bool playerNeverWalked = true;
    public NPCScript targetNPC;
    //public bool forceShowTut;
   
    //public List<Vector3> animPosition;
    //public int currentAnimPosition;

    // Start is called before the first frame update
    void Awake()
    {
        

        timer = timerMax;
        //player1 = findgamobjectwithtag("player")
        
        //if (currentGameState == gameState.Intro)
        // {
        //move ship
        //this.gameObject.transform.position = animPosition[0];
        //introLogic();

        //  }
        // tutorial.enabled = false;
        Debug.Log("set to inactive 1");
        tutorial.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        player1Script = Player1.GetComponent<PlayerScript>();

        if (player1Script.shootTarget != null)
        {
            targetNPC = player1Script.shootTarget.gameObject.GetComponent<NPCScript>();
        }
        

        if (currentGameState == gameState.Gameplay && player1Script.walkInput == true)
        {
            playerNeverWalked = false;
        }
        // if (currentGameState == gameState.Intro)
        // {
        // Debug.Log("player should be moving");
        //  Player1.transform.position = this.transform.position;
        //}
       
        if (currentGameState == gameState.Intro)
        {
            player1Script.enabled = false;
            Debug.Log("player should be moving");
            Player1.transform.position = this.gameObject.transform.position;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("SaucerStopped"))
        {
            player1Script.enabled = true;
            currentGameState = gameState.Gameplay;
        }

            timerText.text = timer.ToString();
        careInShipText.text = careInShip + "/" + careCapacity ; //careInShip.ToString("/" + careCapacity);

        distanceToPlayer = Vector3.Distance(Player1.transform.position, this.transform.position);
        playerCare = player1Script.CAREstolen;
        
        //temp TUTORIAL LOGIC
        if (timer >= (timerMax - 10f) && currentGameState == gameState.Gameplay)
        {
            Debug.Log("opening tut showing");
            //tutorial.gameObject.SetActive(true);
            showTutorial();
            tutorial.text = "Collect C.A.R.E from Humanoids. Dispense it into Ship. Avoid Hostiles...";
            
        }
        //790 788
        if (timer <= (timerMax - 10f) && timer >= (timerMax - 12f) && currentGameState == gameState.Gameplay)
        {
            Debug.Log("set opening tut to inactive ");
            // tutorial.gameObject.SetActive(false);
            hideTutorial();

        }
        
        if (timer <= (timerMax - 20f)  && timer >= (timerMax - 23f) &&currentGameState == gameState.Gameplay && playerNeverWalked)
        {
            Debug.Log("wasd tut showing");
            //tutorial.gameObject.SetActive(true);
            showTutorial();
            tutorial.text = "Use WASD to move";

        }else
            //if (timer <= (timerMax - 20f) && playerNeverWalked == false)
           if (playerNeverWalked == false && tutorial.text == "Use WASD to move")
        {
            Debug.Log("set WASD to inactive ");
            //tutorial.gameObject.SetActive(false);
            tutorial.text = "clear";
            hideTutorial();
        }

        if (distanceToPlayer < 5 && player1Script.CAREstolen > 0 && !dispensing)
        {
            Debug.Log("dispense tut showing");
            //tutorial.gameObject.SetActive(true);
            showTutorial();
            tutorial.text = "Stand under your ship and hold Right Mouse Button to dispense C.A.R.E";

        }
        else
            if (dispensing)
        {
            Debug.Log("dispense tut closed");
            //tutorial.gameObject.SetActive(false);
            hideTutorial();
        }

        if (player1Script.goldieLocks && player1Script.distanceToClosestNPC < 50 && targetNPC.drained == false)
        {
            Debug.Log("close to harvest tut showing");
            // tutorial.gameObject.SetActive(true);
            showTutorial();
            tutorial.text = "Use Left Mouse button to harvest C.A.R.E";

        }
        else
        if(tutorial.text == "Use Left Mouse button to harvest C.A.R.E")
        {
            Debug.Log("tut closed");
            hideTutorial();
            //tutorial.gameObject.SetActive(false);

        }
        /*
            if (timer <= (timerMax - 5f) && currentGameState == gameState.Gameplay)
        {
            tutorial.gameObject.SetActive(false);

        }*/


        if (timerActive && currentGameState == gameState.Gameplay)
        {
            timer -= 1 * Time.deltaTime;
           
            if (timer < 60)
            {
                timerText.color = Color.red;
            }

                if (timer <= 0)
            {
                Debug.Log("TIME UP");

                timer = 0;
                timeUp();
                
            }
        }


     // if ()

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

            

            if (timer <= (timerMax - 5f) && currentGameState == gameState.Gameplay)
            {
                Debug.Log("set inactive 4");
                //tutorial.gameObject.SetActive(false);
                hideTutorial();
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


            //Scene sceneToLoad = SceneManager.GetSceneByName(Success);//SceneManager.GetSceneByBuildIndex(4);
            // SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetSceneByName(theLevel));
            //SceneManager.MoveGameObjectToScene(Player1, SceneManager.GetSceneByName(theLevel));

              SceneManager.LoadScene("Success");
            // LoadYourAsyncScene();
            //StartCoroutine(LoadYourAsyncScene());
        }

        
    }

    void showTutorial()
    {
        Debug.Log("showing tutorial");
        tutorial.gameObject.SetActive(true);
    }

    void hideTutorial()
    {
        Debug.Log("hiding tutorial");
        tutorial.gameObject.SetActive(false);
    }

}
