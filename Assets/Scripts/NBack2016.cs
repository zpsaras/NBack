using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NBack2016 : MonoBehaviour {

	public int numberOfTargets;
	public int deckSize;
	public int numberOfLures;

	public Text zeroBackTargetText;
	public GameObject titlePanel;

	private Letter[] zeroBackLetters;
	private Letter[] oneBackLetters;
	private Letter[] twoBackLetters;
	private Letter[] threeBackLetters;

	private char zeroBackTarget;


	// Use this for initialization
	void Start () {

		if (deckSize < numberOfTargets) {
			Debug.Log("ERROR: WRONG SIZE DECK / TARGETS");
		}
		// Set up 0-back
		setUpZeroBack();

		// Set up 1-back
		setUpOneBack();

		twoBackLetters		= new Letter[deckSize + 2];
		threeBackLetters	= new Letter[deckSize + 3];

		StartCoroutine(startZeroBack());
	}
	
	// Update is called once per frame
	void Update () {

	}

	IEnumerator startZeroBack() {
		// Set hint
		zeroBackTargetText.text = zeroBackTarget.ToString();
		titlePanel.GetComponentInChildren<Text>().text = "0-Back";
		yield return null;
	}

	void setUpZeroBack() {
		int i;
		zeroBackLetters			= new Letter[deckSize];
		System.Random rng		= new System.Random();
		zeroBackTarget			= (char) rng.Next(65, 90);

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

	void setUpOneBack() {
		int i;
		oneBackLetters = new Letter[deckSize + 1];
		System.Random rng = new System.Random();

		// Fill in targets
		for (i = 0; i < numberOfTargets; i++) {

		}
	}

	// Debug function
	private void printLetterArray(Letter[] arr) {
		foreach (Letter l in arr) {
			Debug.Log(l.getChar().ToString() + " / IsLure: " + l.isLure() + " IsTarget: " + l.isTarget());
		}
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
	private bool lure, target;
	private char character;

	public Letter(char c, bool targ, bool lure) {
		this.character = c;
		this.target = targ;
		this.lure = lure;
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