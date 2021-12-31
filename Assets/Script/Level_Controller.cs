using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Controller : MonoBehaviour
{
    private Spring[] springs;
    private Robot robot;
    private Camera_Behavior camera;
    private int behavior;

    public static bool isPaused = false;
    [SerializeField] private LayerMask layerMask;
    private GameObject[] pauseObjects;

    private void OnEnable()
    {
        springs = FindObjectsOfType<Spring>();
        robot = FindObjectOfType<Robot>();
        camera = FindObjectOfType<Camera_Behavior>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        behavior = 0;
        isPaused = false;
        HidePaused();
        Physics2D.IgnoreLayerCollision(3, 6, true);
    }

    // Update is called once per frame
    void Update()
    {

        Pause_Resume();
        camera.CheckIfPlayerJumps(Robot.isJumping);
    }
    private void FixedUpdate()
    {

        IsPlayerClimbing();
        CompareCharges();
    }

    private void CompareCharges()
    {
        foreach (var spring in springs)
        {
            if (Physics2D.OverlapBox(spring.transform.position, transform.localScale / 2, 0, layerMask))
            {

                behavior = robot.GetCharge() * spring.GetCharge();
                robot.SetBehavior(behavior);

            }
        }
        


    }

    private void Pause_Resume()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPaused)
            {
                Time.timeScale = 0;
                isPaused = true;
                ShowPaused();
            }
            else
            {
                Time.timeScale = 1;
                isPaused = false;
                HidePaused();
            }
        }
    }

    public void Resume()
    {
       
            Time.timeScale = 1;
            isPaused = false;
            HidePaused();
        
    }

    public void Reload()
    {
        SceneManager.LoadScene("Demo_Sandbox");
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void HidePaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

    private void ShowPaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }
    private void IsPlayerClimbing()
    {
       if (robot.onLadder)
        {
            if(robot.isGrounded)
            {
                Physics2D.IgnoreLayerCollision(3, 6, false);
                
            }
            if(Robot.isClimbing)
            {
                Physics2D.IgnoreLayerCollision(3, 6, true);
            }
           
        }
       if(!robot.onLadder)
        {
            Physics2D.IgnoreLayerCollision(3, 6, false);
        }
        
    }

    
}
