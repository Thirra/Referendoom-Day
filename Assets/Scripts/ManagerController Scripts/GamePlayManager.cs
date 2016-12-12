using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePlayManager : MonoBehaviour {

    #region Serialized Fields
    [SerializeField]
    VideoController videoController;
    [SerializeField]
    UIController uiController;
    #endregion

    #region Private Variables
    private int inputCounter;
    private List<string> inputStringList = new List<string>();
    private VideoInputLogic videoInputLogic = new VideoInputLogic();
    private IncidentInputLogic incidentInputLogic = new IncidentInputLogic();
    private ResponseInputLogic responseInputLogic = new ResponseInputLogic();
    #endregion

    #region Gameplay Modifiers
    //True if the players are asked to input incidents before each round
    bool inputIncidents;
    #endregion

    public bool AutoRandomIncidents;

    public List<Incident> Incidents;
    public List<Response> Responses;
    public Incident currentIncident;
    public Response currentResponse;
    public List<Incident> UsableIncidents;
    public GameStates currentGameState;

    public int incidentsInGame;
    public int incidentCounter;

    public float IncidentPlayTimer;
    public float ResponsePlayTimer;

    public int[,] pointArray;

    public enum GameStates
    {
        ChooseIncident,
        PlayIncident,
        ChooseResponse,
        PlayResolution
    }

    #region Singlton Pattern
    public static GamePlayManager instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    // Use this for initialization
    void Start ()
    {
        //Create the usalbe incidents list
        for (int index = 0; index < Incidents.Count; index++)
        {
            Debug.Log("create usable incident: " + index);
            UsableIncidents.Add(Incidents[index]);
        }

        ChangeState(GameStates.ChooseIncident);
        incidentCounter = 0;
        InitializePointArray();
    }

    //Changes the current game state and adjusts input field visibility
    void ChangeState(GameStates _gameState)
    {
        currentGameState = _gameState;

        switch (_gameState)
        {
            case GameStates.ChooseIncident:
                if(AutoRandomIncidents)
                {
                    ActivateIncident((int)(Random.Range(0, UsableIncidents.Count )));
                }
                else
                {
                    uiController.ActivateIncidentInputField();
                }
                break;
            case GameStates.ChooseResponse:
                uiController.ActivateResponseInputField();
                break;
            case GameStates.PlayIncident:
                uiController.DeActivateInputFields();
                break;
            case GameStates.PlayResolution:
                uiController.DeActivateInputFields();
                break;
        }
    }

    //Called when the player enters input to choose incident
    public void ActivateIncident()
    {
        currentIncident = incidentInputLogic.DetermineIncident(uiController.inputField_Incident.text, Incidents);
        currentIncident.Activate();
        videoController.PlayVideo(videoInputLogic.DetermineIncidentVideo(videoController.videoClips));
        //Sets the timer to the duration of the video
        IncidentPlayTimer = videoInputLogic.DetermineIncidentVideo(videoController.videoClips).duration + 1;
        ChangeState(GameStates.PlayIncident);
        incidentCounter += 1;
    }

    public void ActivateIncident(int _incidentIndex)
    {
        currentIncident = UsableIncidents[_incidentIndex];
        currentIncident.Activate();
        videoController.PlayVideo(videoInputLogic.DetermineIncidentVideo(videoController.videoClips));
        //Sets the timer to the duration of the video
        IncidentPlayTimer = videoInputLogic.DetermineIncidentVideo(videoController.videoClips).duration + 1;
        ChangeState(GameStates.PlayIncident);
        incidentCounter += 1;
        UsableIncidents.RemoveAt(_incidentIndex);
    }

    //Called when the player enters input to choose response
    public void ActivateResponse()
    {
        currentResponse = responseInputLogic.DetermineResponse(uiController.inputField_Response.text, Responses);
        currentResponse.Activate();
        videoController.PlayVideo(videoInputLogic.DetermineResponseVideo(videoController.videoClips));
        //Sets the timer to the duration of the video
        ResponsePlayTimer = videoInputLogic.DetermineResponseVideo(videoController.videoClips).duration + 1;
        ChangeState(GameStates.PlayResolution);
    }

    // Update is called once per frame
    void Update ()
    {
        UpdateTimers();
	}

    //Controls the timers that control playing input between response and incident video playbacks
    void UpdateTimers()
    {
        if(IncidentPlayTimer > 0)
        {
            IncidentPlayTimer -= Time.deltaTime;
        }
        else
        {
            if(currentGameState == GameStates.PlayIncident)
            {
                ChangeState(GameStates.ChooseResponse);
            }
        }

        if(ResponsePlayTimer > 0)
        {
            ResponsePlayTimer -= Time.deltaTime;
        }
        else
        {
            if (currentGameState == GameStates.PlayResolution)
            {
                if(incidentCounter == incidentsInGame)
                {
                    EndGame();
                }
                else
                {
                    ChangeState(GameStates.ChooseIncident);
                }
            }
        }
    }

    void InitializePointArray()
    {

        pointArray = new int[,]
        {
            {-1,-1,1,1,-1,1,1}, /*Incident 1*/
            {1,-1,1,-1,1,-1,-1}, /*Incident 2*/
            {1,1,-1,-1,-1,-1,1}, /*Incident 3*/
            {1,-1,1,-1,1,1,-1}, /*Incident 4*/
            {-1,-1,1,1,-1,1,1}, /*Incident 5*/
            {-1,1,-1,1,1,1,-1}, /*Incident 6*/
            {1,1,-1,-1,1,1,-1}, /*Incident 7*/
            {1,-1,1,1,-1,-1,-1}, /*Incident 8*/
            {-1,1,1,-1,1,-1,1}, /*Incident 9*/
            {1,-1,-1,-1,1,-1,1}  /*Incident 10*/
        };
    }

    //Called when all the incidents have been responded to
    void EndGame()
    {
        GameManager.instance.ChangeScene(GameManager.instance.tag_OutroScene);
    }
}
