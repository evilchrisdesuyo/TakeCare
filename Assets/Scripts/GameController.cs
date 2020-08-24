using System.Collections;
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
    public Animator anim;
    //public List<Vector3> animPosition;
    //public int currentAnimPosition;

    // Start is called before the first frame update
    void Awake()
    {
        //player1 = findgamobjectwithtag("player")
        player1Script = Player1.GetComponent<PlayerScript>();
        //if (currentGameState == gameState.Intro)
       // {
            //move ship
            //this.gameObject.transform.position = animPosition[0];
            //introLogic();

      //  }
    }

    // Update is called once per frame
    void Update()
    {

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


            //Scene sceneToLoad = SceneManager.GetSceneByName("Success");//SceneManager.GetSceneByBuildIndex(4);
            //SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetSceneByName("Success"));
            // SceneManager.MoveGameObjectToScene(Player1, SceneManager.GetSceneByName("Success"));

             SceneManager.LoadScene("Success");
           // LoadYourAsyncScene();
        }

        /*
        IEnumerator LoadYourAsyncScene()
        {
            // Set the current Scene to be able to unload it later
            Scene currentScene = SceneManager.GetActiveScene();

            // The Application loads the Scene in the background at the same time as the current Scene.
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(m_Scene, LoadSceneMode.Additive);

            // Wait until the last operation fully loads to return anything
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
            SceneManager.MoveGameObjectToScene(m_MyGameObject, SceneManager.GetSceneByName(m_Scene));
            // Unload the previous Scene
            SceneManager.UnloadSceneAsync(currentScene);
        }*/
    }



}
