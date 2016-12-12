using UnityEngine;
using System.Collections;

public class Response : MonoBehaviour {

    public int multiplier;
    //Used to match the playing input to the response card
    public string ID;
    //Used to match the response card to the video clip
    //public string VideoID;
    //Use to calculate the points gvien compared to the incident
    public int ResponseIndex;

    //If your response cards have effects, put them here
    public void Activate()
    {
        int IncidentIndex = GamePlayManager.instance.currentIncident.incidentIndex;
        Debug.Log("Inc_Index: " + IncidentIndex);
        int ResponseIndex = GamePlayManager.instance.currentResponse.ResponseIndex;
        Debug.Log("Res_Index: " + ResponseIndex);
        GameManager.instance.PlayScore += (GamePlayManager.instance.pointArray[IncidentIndex, ResponseIndex]) * multiplier;
        Debug.Log("Points: " + (GamePlayManager.instance.pointArray[IncidentIndex, ResponseIndex]) * multiplier);
    }
}
