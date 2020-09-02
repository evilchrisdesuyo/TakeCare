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
    public enum gameState {Intro, Tutorial, Gameplay}
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
    public GameObject playerModel;
    public bool firstTimeTutorial = true;
    public NPCScript tutorialFirstWoman;
    public NPCScript tutorialSecondMan;
    public NPCScript tutorialAgent;
    public float distanceToSecond;
    public float countDownToStart = 3f;
    //pblic bool forceShowTut;

     float fleeTutorialTimer = 5;
    float hasntSeenYou = 5;
    float lightTut = 5;
    float radiationTut = 5;
    float collectedCARETut = 5;
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
            playerModel.SetActive(false);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("SaucerStopped"))
        {
            player1Script.enabled = true;
            playerModel.SetActive(true);
            currentGameState = gameState.Gameplay;
        }

            timerText.text = timer.ToString();
        careInShipText.text = careInShip + "/" + careCapacity ; //careInShip.ToString("/" + careCapacity);

        distanceToPlayer = Vector3.Distance(Player1.transform.position, this.transform.position);
        playerCare = player1Script.CAREstolen;

        //temp TUTORIAL LOGIC
        //This is chris from the past. I know you're planning on disabling the timer during the tutorial... but this code relies on it... you're welcome

        if (timer >= (timerMax - 10f) && currentGameState == gameState.Gameplay)
        {
            Debug.Log("opening tut showing");
            //tutorial.gameObject.SetActive(true);
            showTutorial();
            tutorial.text = "Welcome to E-Arth. Your mission: Collect C.A.R.E from Humanoids. Dispense it into the ship.";
            
        }
        //790 788
   /*     if (timer <= (timerMax - 10f) && timer >= (timerMax - 12f) && currentGameState == gameState.Gameplay)
        {
            Debug.Log("set opening tut to inactive ");
            // tutorial.gameObject.SetActive(false);
            hideTutorial();

        }*/
        
        //This is chris from the past. I know you're planning on disabling the timer during the tutorial... but this code relies on it... you're welcome
        if (timer <= (timerMax - 20f)  && timer >= (timerMax - 23f) && currentGameState == gameState.Gameplay && playerNeverWalked)
        {
            Debug.Log("wasd tut showing");
            //tutorial.gameObject.SetActive(true);
            showTutorial();
            tutorial.text = "Use WASD to move";

        }
        /*else
            //if (timer <= (timerMax - 20f) && playerNeverWalked == false)
           if (playerNeverWalked == false && tutorial.text == "Use WASD to move")
        {
            Debug.Log("set WASD to inactive ");
            //tutorial.gameObject.SetActive(false);
            tutorial.text = "clear";
            hideTutorial();
        }*/
        float distanceToTutWoman = Vector3.Distance(Player1.transform.position, tutorialFirstWoman.transform.position);
        //float fleeTutorialTimer = 5;
       
       
        if (distanceToTutWoman <= 300 && tutorialFirstWoman != null && tutorialFirstWoman.currentBehavior == NPCScript.behaviorState.Fleeing && fleeTutorialTimer >= 0) //&& tutorial.text != "Humans will flee if they see you.")
        {
            Debug.Log("SHOWING FLEEING TUTORIAL");
            showTutorial();
            tutorial.text = "Humans will flee if they see you.";
            fleeTutorialTimer -= 1 * Time.deltaTime;
        }

        if (tutorialSecondMan != null)
         {
             distanceToSecond = Vector3.Distance(Player1.transform.position, tutorialSecondMan.gameObject.transform.position);
        }
        
      
        if (tutorialSecondMan != null && tutorialSecondMan.drained == false && distanceToSecond <= 25 && hasntSeenYou >= 0)
        {
            showTutorial();
            tutorial.text = "That one hasnt seen you. Walk behind it and harvest some C.A.R.E";
            hasntSeenYou -= 1 * Time.deltaTime;
        }

        
        if (tutorialSecondMan != null && tutorialSecondMan.drained == false && distanceToSecond <= 15 && lightTut >= 5)
        {
            showTutorial();
            tutorial.text = "The light on your gun will turn yellow when in range.";
            lightTut -= 1 * Time.deltaTime;
        }

        if (tutorialSecondMan == null && tutorial.text == "The light on your gun will turn yellow when in range." && radiationTut >= 0)
        {
            showTutorial();
            tutorial.text = "The radiation from your gun will combust humans if you get too close to them...";
            radiationTut -= 1 * Time.deltaTime;
        }

        if (tutorialSecondMan != null && tutorialSecondMan.drained == true && collectedCARETut >= 0)
        {
            showTutorial();
            tutorial.text = "You have collected a humans CARE! Take it back the ship! Dont worry, the human will awaken soon.";
            collectedCARETut -= 1 * Time.deltaTime;
        }
        
        if (tutorialSecondMan != null && tutorialSecondMan.drained == true && distanceToSecond >= 10)
        {
            //move agent to collapsed dude
            Debug.Log("AGENT STUCK");
            tutorialAgent.currentBehavior = NPCScript.behaviorState.Walking;
            tutorialAgent.currentWalkTarget = tutorialSecondMan.gameObject;
        }

        
        //distance between agent and guy facing wrong way
        if (tutorialAgent != null && tutorialSecondMan != null)
        {
            float distanceFromAgentToTut = Vector3.Distance(tutorialAgent.transform.position, tutorialSecondMan.transform.position);

            if (tutorialAgent != null && tutorialSecondMan != null && tutorialSecondMan.drained == true && distanceFromAgentToTut <= 5)
            {
                //move agent to collapsed dude
                tutorialAgent.currentBehavior = NPCScript.behaviorState.Idle;
                //tutorialAgent.currentWalkTarget = tutorialSecondMan.gameObject;
            }
        }
        

        /*
        if (tutorialAgent.currentBehavior == NPCScript.behaviorState.Walking)
        {
            tutorialAgent.currentBehavior = NPCScript.behaviorState.Idle;
        }*/

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
       /* else
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


            /*
            if (timer <= (timerMax - 5f) && currentGameState == gameState.Gameplay)
            {
                Debug.Log("set inactive 4");
                //tutorial.gameObject.SetActive(false);
                hideTutorial();
            }
            */
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
        Debug.Log("showing tutorial FUNCTION");
        tutorial.gameObject.SetActive(true);
        //wait
        StartCoroutine(tutorialHideTimer());
    }

    void hideTutorial()
    {
        Debug.Log("hiding tutorial");
        //tutorial.gameObject.SetActive(false);
    }

    IEnumerator tutorialHideTimer()
    {

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);
        
        hideTutorial();
    }
}
