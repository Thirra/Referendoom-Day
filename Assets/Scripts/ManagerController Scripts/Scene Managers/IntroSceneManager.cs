﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour {

    // A timer used to track when to automatically progress to the next scene
    //You will want to set this to be the same, if not a bit longer than the timer of your into movie
    public float introSceneTimer;

    // A bool that determines whether or not the timer will automatically trigger the gameplay scene or simply enable the button to continue
    public bool automaticSceneContinuation;

    //The object in the ui that prompts the player to press space to continue
    public GameObject continueDialogue;

    // Use this for initialization
    void Start ()
    {
        //Turns off the continue prompt until the timer has expired.
        continueDialogue.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        introSceneTimer -= Time.deltaTime;

        //When timer hits 0
        if (introSceneTimer <= 0)
        {
            continueDialogue.SetActive(true);

            //If it auto contniues do that
            if (automaticSceneContinuation)
            {
                GameManager.instance.ChangeScene(GameManager.instance.tag_GameplayScene);
                introSceneTimer = 0;
            }
            //Otherwise wait till space bar
            else
            {
                if ((Input.GetKeyDown(KeyCode.Space)))
                {
                    GameManager.instance.ChangeScene(GameManager.instance.tag_GameplayScene);
                }
            }
        }

    }
}
