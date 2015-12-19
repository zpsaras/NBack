using UnityEngine;
using System.Collections;

public class NBack2016Animation : MonoBehaviour {

	public Animator wordPresent;

	const string k_enterTask	= "enter";
	const string k_newLetter	= "new";
	const string k_waiting		= "wait";

	private int m_EntryParameterId,
				m_PresentParameterId,
				m_WaitParameterId;

	public void OnEnable() {
		m_EntryParameterId 		= Animator.StringToHash (k_enterTask);
		m_PresentParameterId 	= Animator.StringToHash (k_newLetter);
		m_WaitParameterId 		= Animator.StringToHash (k_waiting);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void presentNewWord() {
		wordPresent.SetBool (m_PresentParameterId, true);
		StartCoroutine (waitUntilAnimFinished (m_PresentParameterId,false));
	}

	public void enterNewTask() {
		wordPresent.SetBool (m_EntryParameterId, true);
		StartCoroutine (waitUntilAnimFinished (m_EntryParameterId,false));
	}

	IEnumerator waitUntilAnimFinished(int m_param, bool val) {
		bool stateReached = false;
		while (!stateReached) {
			if (!wordPresent.IsInTransition (0)) {
				stateReached = true;

			}
			yield return new WaitForEndOfFrame ();
		}
		wordPresent.SetBool (m_param, val);
	}
}
