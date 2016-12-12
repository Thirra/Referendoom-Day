using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurrentScoreDisplay : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        GetComponent<Slider>().value = GameManager.instance.PlayScore;
    }
}
