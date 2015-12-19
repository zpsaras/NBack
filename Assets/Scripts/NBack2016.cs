using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class NBack2016 : MonoBehaviour {

	public int numberOfTargets;
	public int deckSize;
	public int numberOfLures;
	public int countdownLength;

	public AudioSource clickSource;
	public AudioSource countdownToneSource;

	public GameObject zeroBackTargetPanel;
	public GameObject titlePanel;
	public Text currentLetterText;
	public Text fKeyText;
	public Text jKeyText;

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
			fKeyText.text = "No Match - F";
			jKeyText.text = "Match - J";
		}

		setUpZeroBack();

		setUpOneBack();

		setUpTwoBack();

		setUpThreeBack();

		StartCoroutine(taskIntermission(startZeroBack()));
	}

	IEnumerator taskIntermission(IEnumerator nextPhase) {
		float count = countdownLength + 1;
		float timeHold = Time.time;
		titlePanel.GetComponentInChildren<Text> ().text = "Twilight Phase";
		currentLetterText.text = count.ToString ();
		countdownToneSource.Play ();
		zeroBackTargetPanel.SetActive (false);
		while (true) {
			if (count > 1) {
				if (Time.time - timeHold > 1) {
					countdownToneSource.Play ();
					timeHold = Time.time;
				}
				currentLetterText.text = ((int)Math.Floor (count)).ToString ();
				count -= Time.deltaTime;
			} else {
				break;
			}
			yield return new WaitForEndOfFrame ();
		}
		StartCoroutine (nextPhase);
	}


	// NOTE: This should only be called *IF ONE OF THE KEYS HAS BEEN PRESSED*
	// This function's function (;]) is to normalize Match to `true` across versions
	bool keyHandler(bool f, bool j) {
		clickSource.Play ();
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
		zeroBackTargetPanel.SetActive(true);
		zeroBackTargetPanel.GetComponentInChildren<Text>().text = zeroBackTarget.ToString();
		titlePanel.GetComponentInChildren<Text>().text = "0-Back";
		int curr = 0;
		bool refresh = true;

		while (!zeroBackFinished) {
			// Task logic here
			if (curr == zeroBackLetters.Length) {
				zeroBackFinished = true;
				break;
			}
			if (refresh) {
				// UI Update Logic
				currentLetterText.text = zeroBackLetters[curr].getChar().ToString();
				refresh = false;
			}
			// Key logic here
			bool f = Input.GetKeyDown(KeyCode.F);
			bool j = Input.GetKeyDown(KeyCode.J);
			if (f || j) {
				if (curr < zeroBackLetters.Length) {
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
		printLetterArray(zeroBackLetters);
		StartCoroutine (taskIntermission(startOneBack()));
	}

	IEnumerator startOneBack() {
		zeroBackTargetPanel.SetActive (false);
		titlePanel.GetComponentInChildren<Text> ().text = "1-Back";
		int curr = 0;
		bool refresh = true;

		while (!oneBackFinished) {

			if (curr == oneBackLetters.Length) {
				oneBackFinished = true;
				break;
			}
			if (refresh) {
				currentLetterText.text = oneBackLetters [curr].getChar ().ToString ();
				refresh = false;
			}
			bool f = Input.GetKeyDown (KeyCode.F);
			bool j = Input.GetKeyDown (KeyCode.J);
			if (f || j) {
				if (curr < oneBackLetters.Length) {
					if (keyHandler (f, j)) {
						oneBackLetters [curr].setResponse (Letter.resp.match);
					} else {
						oneBackLetters [curr].setResponse (Letter.resp.noMatch);
					}
					curr++;
					refresh = true;
				} else {
					oneBackFinished = true;
					break;
				}
			}
			yield return null;
		}
		printLetterArray(oneBackLetters);
		StartCoroutine(taskIntermission(startTwoBack()));
	}

	IEnumerator startTwoBack() {
		zeroBackTargetPanel.SetActive(false);
		titlePanel.GetComponentInChildren<Text>().text = "2-Back";
		int curr = 0;
		bool refresh = true;

		while (!twoBackFinished) {
			if (curr == twoBackLetters.Length) {
				twoBackFinished = true;
				break;
			}
			if (refresh) {
				currentLetterText.text = twoBackLetters[curr].getChar().ToString();
				refresh = false;
			}
			bool f = Input.GetKeyDown(KeyCode.F);
			bool j = Input.GetKeyDown(KeyCode.J);
			if (f || j) {
				if (curr < twoBackLetters.Length) {
					if (keyHandler(f, j)) {
						twoBackLetters[curr].setResponse(Letter.resp.match);
					} else {
						twoBackLetters[curr].setResponse(Letter.resp.noMatch);
					}
					curr++;
					refresh = true;
				} else {
					twoBackFinished = true;
					break;
				}
			}
			yield return null;
		}
		printLetterArray(twoBackLetters);
		StartCoroutine (taskIntermission(startThreeBack()));
	}

	IEnumerator startThreeBack() {
		zeroBackTargetPanel.SetActive(false);
		titlePanel.GetComponentInChildren<Text>().text = "3-Back";
		int curr = 0;
		bool refresh = true;

		while (!threeBackFinished) {
			if (curr == threeBackLetters.Length) {
				threeBackFinished = true;
				break;
			}
			if (refresh) {
				currentLetterText.text = threeBackLetters[curr].getChar().ToString();
				refresh = false;
			}
			bool f = Input.GetKeyDown(KeyCode.F);
			bool j = Input.GetKeyDown(KeyCode.J);
			if (f || j) {
				if (curr < threeBackLetters.Length) {
					if (keyHandler(f, j)) {
						threeBackLetters[curr].setResponse(Letter.resp.match);
					} else {
						threeBackLetters[curr].setResponse(Letter.resp.noMatch);
					}
					curr++;
					refresh = true;
				} else {
					threeBackFinished = true;
					break;
				}
			}
			yield return null;
		}
		printLetterArray(threeBackLetters);
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
		// Before you ask, I did this for readability.
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
				//Debug.Log("Added new letter in pos: [" + i + "] with value: " + oneBackLetters[i].getChar());
			}
		}
	}

	// Lure bug?
	void setUpTwoBack() {
		int i, j, rand, luresLeft = numberOfLures;
		char hold;
		bool flag = false;
		twoBackLetters		= new Letter[deckSize + 2];
		System.Random rng	= new System.Random();
		twoBackFinished		= false;

		char[] tempTargetArray = createTargetArray(rng);
		int[] targetLocations = new int[numberOfTargets];

		// Fill targets
		i = 0;
		while (i < numberOfTargets) {
			j = rng.Next(0, twoBackLetters.Length - 2);
			if ((twoBackLetters[j] == null) && (twoBackLetters[j + 2] == null)) {
				twoBackLetters[j] = new Letter(tempTargetArray[i], false, false);
				twoBackLetters[j + 2] = new Letter(tempTargetArray[i], true, false);
				targetLocations[i] = j + 2;

				// Perhaps add a lures idk
				if (luresLeft > 0) {

					// j+2 is target location
					rand = rng.Next(0, 2);
					if (rand == 0) {
						// Insert @ targetLocation - 3
						if (j - 1 >= 0) {
							if (twoBackLetters[j - 1] == null) {
								twoBackLetters[j - 1] = new Letter(tempTargetArray[i], false, true);
								luresLeft--;
							} else {
								rand = 1;
							}
						} else {
							rand = 1;
						}
					} 
					if (rand == 1) {
						// Insert @ targetLocation - 1
						if (twoBackLetters[j + 1] == null) {
							twoBackLetters[j + 1] = new Letter(tempTargetArray[i], false, true);
							luresLeft--;
						} // Never cycles around?
					}

					luresLeft--;
				}

				i++;
			} else {
				continue;
			}
		}

		// Initialize rest
		for (i = 0; i < twoBackLetters.Length; i++) {
			if (twoBackLetters[i] == null) {
				twoBackLetters[i] = new Letter('?', false, false);
			}
		}

		// Fill random while protecting against accidental targets & lures
		for (i = 0; i < twoBackLetters.Length; i++) {
			flag = false;
			if (twoBackLetters[i].getChar() == '?') {
				// Empty slot for insertion
				while (!flag) {
					hold = (char)rng.Next(65, 90);
					// Ensure that we aren't accidentally inserting what would be a target
					// I know it's sloppy and I'm moderately sorry
					if (i == 0 || i == 1) {
						if ((twoBackLetters[i + 2].getChar() == hold)) {
							// NO-K
							continue;
						} else {
							twoBackLetters[i] = new Letter(hold, false, false);
							flag = true;
							continue;
						}
					}
					if (i > 0 && i < (twoBackLetters.Length - 2)) {
						Debug.Log(i);
						if ((twoBackLetters[i - 2].getChar() == hold) ||
							(twoBackLetters[i + 2].getChar() == hold)) {
							// This is NO-K
							continue;
						} else {
							twoBackLetters[i] = new Letter(hold, false, false);
							flag = true;
						}
					} else if (i == 0) { // Redundant
						if (twoBackLetters[i + 2].getChar() == hold) {
							// NO-K
							continue;
						} else {
							twoBackLetters[i] = new Letter(hold, false, false);
							flag = true;
						}
					} else if (i >= (twoBackLetters.Length - 2)) {
						if (twoBackLetters[i - 2].getChar() == hold) {
							//NO-K
							continue;
						} else {
							twoBackLetters[i] = new Letter(hold, false, false);
							flag = true;
						}
					}
				}
			}
		}
	}

	// Lure bug?
	void setUpThreeBack() {
		int i, j, rand, luresLeft = numberOfLures;
		char hold;
		bool flag = false;
		threeBackLetters	= new Letter[deckSize + 3];
		System.Random rng	= new System.Random();
		threeBackFinished	= false;

		char[] tempTargetArray = createTargetArray(rng);
		int[] targetLocations = new int[numberOfTargets];

		// Fill targets
		i = 0;
		while (i < numberOfTargets) {
			j = rng.Next(0, threeBackLetters.Length - 3);
			if ((threeBackLetters[j] == null) && (threeBackLetters[j + 3] == null)) {
				threeBackLetters[j] = new Letter(tempTargetArray[i], false, false);
				threeBackLetters[j + 3] = new Letter(tempTargetArray[i], true, false);
				targetLocations[i] = j + 3;

				// Perhaps add a lures idk
				if (luresLeft > 0) {

					// j+3 is target location
					rand = rng.Next(0, 2);
					if (rand == 0) {
						// Insert @ targetLocation - 4
						if (j - 1 >= 0) {
							if (threeBackLetters[j - 1] == null) {
								threeBackLetters[j - 1] = new Letter(tempTargetArray[i], false, true);
								luresLeft--;
							} else {
								rand = 1;
							}
						} else {
							rand = 1;
						}
					} 
					if (rand == 1) {
						// Insert @ targetLocation - 1
						if (threeBackLetters[j + 1] == null) {
							threeBackLetters[j + 1] = new Letter(tempTargetArray[i], false, true);
							luresLeft--;
						} // Never cycles around?
					}

					luresLeft--;
				}

				i++;
			} else {
				continue;
			}
		}

		// Initialize rest
		for (i = 0; i < threeBackLetters.Length; i++) {
			if (threeBackLetters[i] == null) {
				threeBackLetters[i] = new Letter('?', false, false);
			}
		}

		// Fill random while protecting against accidental targets & lures
		for (i = 0; i < threeBackLetters.Length; i++) {
			flag = false;
			if (threeBackLetters[i].getChar() == '?') {
				// Empty slot for insertion
				while (!flag) {
					hold = (char)rng.Next(65, 90);
					// Ensure that we aren't accidentally inserting what would be a target
					// I know it's sloppy and I'm moderately sorry
					if (i == 0 || i == 1 || i == 2) {
						if ((threeBackLetters[i + 3].getChar() == hold)) {
							// NO-K
							continue;
						} else {
							threeBackLetters[i] = new Letter(hold, false, false);
							flag = true;
							continue;
						}
					}
					if (i > 0 && i < (threeBackLetters.Length - 3)) {
						Debug.Log(i);
						if ((threeBackLetters[i - 3].getChar() == hold) ||
							(threeBackLetters[i + 3].getChar() == hold)) {
							// This is NO-K
							continue;
						} else {
							threeBackLetters[i] = new Letter(hold, false, false);
							flag = true;
						}
					} else if (i == 0) { // Redundant
						if (threeBackLetters[i + 3].getChar() == hold) {
							// NO-K
							continue;
						} else {
							threeBackLetters[i] = new Letter(hold, false, false);
							flag = true;
						}
					} else if (i >= (threeBackLetters.Length - 3)) {
						if (threeBackLetters[i - 3].getChar() == hold) {
							//NO-K
							continue;
						} else {
							threeBackLetters[i] = new Letter(hold, false, false);
							flag = true;
						}
					}
				}
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