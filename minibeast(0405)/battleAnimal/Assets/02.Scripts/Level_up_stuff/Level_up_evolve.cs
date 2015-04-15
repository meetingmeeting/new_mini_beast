﻿using UnityEngine;
using System.Collections;

public class Level_up_evolve : MonoBehaviour {

	public int currentlevel;
	
	public int killint;
	
	public GameObject firstEvolvPlayer;
	
	public bool switchToEvol;
	
	public bool evol_already;


	// Use this for initialization
	void Start () {
		
		switchToEvol = false;
		
		evol_already = false;
	
	}
	
	// Update is called once per frame
	void Update () {
	
		
		killint= PlayerPrefs.GetInt ("minions_killed");
		
		if (1 < killint && killint <= 3) {
			
			currentlevel = 1;
			
		} else if (3 < killint && killint <= 6) {
			
			currentlevel = 2;
			
			//StartEvolution();
			
			//switchToEvol = true;
		}//ifelse
		
		
		if(switchToEvol==true && PlayerPrefs.GetString("evolved") =="false"){
			
			StartEvolution();
			PlayerPrefs.SetString ("evolved", "true");
			switchToEvol=false;
			evol_already = true;
		}


	} //end update

	
	void OnGUI(){
		
		if (GUI.Button (new Rect (400, 0, 70, 30), "kill: "+ PlayerPrefs.GetInt("minions_killed"))) {
			
			
		}
		
		
		if (GUI.Button (new Rect (470, 0, 70, 30), "Level: "+ currentlevel)) {
			
			
		}
	} //end gui
	void StartEvolution(){
		
		switchToEvol = false;
		
		GameObject prevob = GameObject.Find (PlayerPrefs.GetString("email"));
		
		Vector3 pastpos = prevob.transform.position;
		
		if(evol_already == false && PlayerPrefs.GetString("evolved")=="false"){
			
			Destroy (prevob); 
			
			
			GameObject aa;
			
			aa = (GameObject)Instantiate(firstEvolvPlayer, pastpos, Quaternion.identity);
			
			aa.name= PlayerPrefs.GetString("email");
			
		}
		
		PlayerPrefs.SetString ("evolved", "true");
		evol_already = true;
		
	}//end evolution

}