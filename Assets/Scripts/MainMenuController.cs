﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Level handoff (WEB)
	public void startTask() {
		if (Application.CanStreamedLevelBeLoaded("Task")){
			SceneManager.LoadScene("Task");
		}
	}
}
