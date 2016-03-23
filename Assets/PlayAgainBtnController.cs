using UnityEngine;
using System.Collections;

public class PlayAgainBtnController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
//		print ("play again");
		if (other.gameObject.tag == "Player") {
//			print ("play again restarting");
			// MainController.mainController.RestartGame();
			MainController.RestartGame();
		}
	}
}
