﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject saucer;
    public Transform saucerEndPoint;
    public bool newGameLogic;
    public float distanceToTarget;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        
       
    }

    // Update is called once per frame
    void Update()
    {

        distanceToTarget = Vector3.Distance(saucer.transform.position, saucerEndPoint.position);
        //move ship
        if (saucer != null && newGameLogic)
        {
            float step = 30 * Time.deltaTime;
            saucer.transform.position = Vector3.MoveTowards(saucer.transform.position, saucerEndPoint.transform.position, step);

            if (distanceToTarget < 0.5)
            {
               // resume();
                SceneManager.LoadScene("video");
            }
        }
    }

    //newgame
    public void newGame()
    {
        
        newGameLogic = true;
        //play video
        //when video done
        //run scene shit
        //SceneManager.LoadScene("Level1");
    }
    //resume
    public void resume()
    {
        SceneManager.LoadScene("Level1");
    }

    //main menu
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    //options

    //credits 
    public void credits()
    {
        SceneManager.LoadScene("Credits");
    }

    //Testlevel 
    public void testLevel()
    {
        SceneManager.LoadScene("evilChrisWhitebox");
    }

    //quit
    public void quit()
    {
        Application.Quit();
    }
}
