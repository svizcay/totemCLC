using UnityEngine;
using System.Collections;

public class HeartController : MonoBehaviour {

	public int nrOverlayedItems = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		nrOverlayedItems++;
		// print ("nr overlayed items: " + nrOverlayedItems);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		nrOverlayedItems--;
	}
}
