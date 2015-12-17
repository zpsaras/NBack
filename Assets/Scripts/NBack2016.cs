using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class NBack2016 : MonoBehaviour {

	public int numberOfTargets;
	public int deckSize;
	public int numberOfLures;

	public GameObject zeroBackTargetPanel;
	public GameObject titlePanel;
	public Text currentLetterText;
	public Text fKeyText;
	public Text jKeyText;

	public Animator animator;

	private Letter[] zeroBackLetters;
	private Letter[] oneBackLetters;
	private Letter[] twoBackLetters;
	private Letter[] threeBackLetters;

	private char zeroBackTarget;
	private bool zeroBackFinished, 
		oneBackFinished,
		twoBackFinished, 
		threeBackFinished;

	public bool version; // false for A, true for B

	// Use this for initialization
	void Start () {

		if (deckSize < numberOfTargets) {
			throw new Exception("ERROR: TARGET # EXCEEDS DECK SIZE");
		}

		// Set Intructional Hint
		if (!version) {
			// Version A
			fKeyText.text = "Match - F";
			jKeyText.text = "No Match - J";
		} else {
			// Version B
			fKeyText.text = "No Match - f";
			jKeyText.text = "Match - J";
		}

		// Set up 0-back
		setUpZeroBack();

		// Set up 1-back
		setUpOneBack();
		printLetterArray(oneBackLetters);
		twoBackLetters		= new Letter[deckSize + 2];
		threeBackLetters	= new Letter[deckSize + 3];

		StartCoroutine(startZeroBack());
	}

	// Update is called once per frame
	void Update () {

	}
	
	// NOTE: This should only be called *IF ONE OF THE KEYS HAS BEEN PRESSED*
	// This function's function (;]) is to normalize Match to `true` across versions
	bool keyHandler(bool f, bool j) {
		if (!version) {
			// Version A
			return f ? true : false;
		} else {
			// Version B
			return j ? true : false;
		}
	}

	IEnumerator startZeroBack() {
		// Set hint
		zeroBackTargetPanel.GetComponentInChildren<Text>().text = zeroBackTarget.ToString();
		titlePanel.GetComponentInChildren<Text>().text = "0-Back";
		int curr = 0;
		bool refresh = true;

		while (!zeroBackFinished) {
			// Task logic here
			if (refresh) {
				// UI Update Logic
				currentLetterText.text = zeroBackLetters[curr].getChar().ToString();
				refresh = false;
			}
			// Key logic here
			bool f = Input.GetKeyDown(KeyCode.F);
			bool j = Input.GetKeyDown(KeyCode.J);
			if (f || j) {
				if (curr < zeroBackLetters.Length - 1) {
					if (keyHandler(f, j)) {
						zeroBackLetters[curr].setResponse(Letter.resp.match);
					} else {
						zeroBackLetters[curr].setResponse(Letter.resp.noMatch);
					}
					curr++; refresh = true;
				} else {
					zeroBackFinished = true;
				}
			}
			yield return null;
		}
		StartCoroutine (startOneBack ());
	}

	IEnumerator startOneBack() {
		zeroBackTargetPanel.SetActive (false);
		titlePanel.GetComponentInChildren<Text> ().text = "1-Back";
		int curr = 0;
		bool refresh = true;

		while (!oneBackFinished) {
			if (refresh) {
				currentLetterText.text = oneBackLetters [curr].getChar ().ToString ();
				refresh = false;
			}
			bool f = Input.GetKeyDown (KeyCode.F);
			bool j = Input.GetKeyDown (KeyCode.J);
			if (f || j) {
				if (curr < oneBackLetters.Length - 1) {
					if (keyHandler (f, j)) {
						oneBackLetters [curr].setResponse (Letter.resp.match);
					} else {
						zeroBackLetters [curr].setResponse (Letter.resp.noMatch);
					}
					curr++;
					refresh = true;
				} else {
					oneBackFinished = true;
				}
			}
			yield return null;
		}
	}

	void setUpZeroBack() {
		int i;
		zeroBackLetters			= new Letter[deckSize];
		System.Random rng		= new System.Random();
		zeroBackTarget			= (char) rng.Next(65, 90);
		zeroBackFinished		= false;

		// Fill in target number of...targets ;]
		for (i = 0; i < numberOfTargets; i++) {
			zeroBackLetters[i] = new Letter(zeroBackTarget, true, false);
		}

		// Resuming from where we left off, fill in junk of varying case.
		// No lures.
		// Before you ask, I did this so that you could read it.
		for (; i < zeroBackLetters.Length; i++) {
			char temp = (char)rng.Next(65, 90);
			while (temp == zeroBackTarget) {
				// Pick again
				temp = (char)rng.Next(65, 90);
			}
			zeroBackLetters[i] = new Letter(temp, false, false);
		}

		// Shuffle the deck
		FisherYates<Letter>(new System.Random(), zeroBackLetters);
		
	}

	char[] createTargetArray(System.Random rng) {
		int i;
		char hold;
		char[] ret = new char[numberOfTargets];
		for (i = 0; i < numberOfTargets; i++) {
			hold = (char)rng.Next(65, 90);
			// Check for existence
			if (existsInArray<char>(hold,ret)) {
				// Shucks.
				i--;
				continue;
			}

			ret[i] = hold;
		}
		return ret;
	}

	void setUpOneBack() {
		int i, j;
		char hold;
		bool flag=false;
		oneBackLetters		= new Letter[deckSize + 1];
		System.Random rng	= new System.Random();
		oneBackFinished		= false;

		// Create array of letters to be targets
		char[] tempTargetArray = createTargetArray(rng);

		// Fill in targets
		i = 0;
		while (i < numberOfTargets) {
			/* Flow ---------------
			 * Pick random index int
			 *	Check current space & Check n spaces ahead
			 *	If something exists, break
			 *	else add first as non-target using target letter
			 *		and n-ahead as target using target letter
			 */
			j = rng.Next(0, oneBackLetters.Length - 1);
			if ((oneBackLetters[j] == null) && (oneBackLetters[j + 1] == null)) {
				oneBackLetters[j] = new Letter(tempTargetArray[i], false, false);
				oneBackLetters[j + 1] = new Letter(tempTargetArray[i], true, false);
				i++;
			} else {
				continue;
			}
		}
		// Initialize the rest
		for (i = 0; i < oneBackLetters.Length; i++) {
			if (oneBackLetters[i] == null) {
				oneBackLetters[i] = new Letter('?', false, false);
			}
		}
			// Fill in the rest
		for (i = 0; i < oneBackLetters.Length; i++) {
			flag = false;
			if (oneBackLetters[i].getChar() == '?') {
				// Empty slot for insertion
				while (!flag) {
					hold = (char)rng.Next(65, 90);
					// Ensure that we aren't accidentally inserting what would be a target
					// I know it's sloppy and I'm moderately sorry
					if (i > 0 && i < (oneBackLetters.Length - 1)) {
						Debug.Log(i);
						if ((oneBackLetters[i - 1].getChar() == hold) ||
							(oneBackLetters[i + 1].getChar() == hold)) {
							// This is NO-K
							continue;
						} else {
							oneBackLetters[i] = new Letter(hold, false, false);
							flag = true;
						}
					} else if (i == 0) {
						if (oneBackLetters[i + 1].getChar() == hold) {
							// NO-K
							continue;
						} else {
							oneBackLetters[i] = new Letter(hold, false, false);
							flag = true;
						}
					} else if (i == (oneBackLetters.Length - 1)) {
						if (oneBackLetters[i - 1].getChar() == hold) {
							//NO-K
							continue;
						} else {
							oneBackLetters[i] = new Letter(hold, false, false);
							flag = true;
						}
					}
				}
				Debug.Log("Added new letter in pos: [" + i + "] with value: " + oneBackLetters[i].getChar());
			}
		}
	}

	// Debug function
	private void printLetterArray(Letter[] arr) {
		foreach (Letter l in arr) {
			Debug.Log(l.getChar().ToString() + " / IsLure: " + l.isLure() + " IsTarget: " + l.isTarget() + " Reponse: " + l.getResponse());
		}
	}

	public static bool existsInArray<T>(T c, T[] arr) {
		int i;
		for (i = 0; i < arr.Length - 1; i++) {
			if (arr[i].Equals(c)) {
				return true;
			}
		}
		return false;
	}

	public static void FisherYates<T>(System.Random rng, T[] arr) {
		Debug.Log("Shuffling an array...");
		int n = arr.Length;
		while (n > 1) {
			int k = rng.Next(n--);
			T temp = arr[n];
			arr[n] = arr[k];
			arr[k] = temp;
		}
	}
}

class Letter {
	public enum resp {
		match,
		noMatch,
		noResp
	}
	private bool lure, target;
	private char character;
	private resp response;

	public Letter(char c, bool targ, bool lure) {
		this.character = c;
		this.target = targ;
		this.lure = lure;
		this.response = resp.noResp;
	}

	public override bool Equals (object obj)
	{
		//return base.Equals (obj);
		if (obj == null) {
			return false;
		}

		Letter l = obj as Letter;
		if ((object)l == null) {
			return false;
		}

		return ((l.lure == this.lure) && (l.target == this.target) && (l.character == this.character) && (l.response == this.response));
	}

	public resp getResponse() {
		return this.response;
	}

	public void setResponse(resp r) {
		this.response = r;
	}

	public char getChar() {
		return this.character;
	}

	public void setChar(char c) {
		this.character = c;
	}

	public bool isLure() {
		return this.lure;
	}

	public bool isTarget() {
		return this.target;
	}
}