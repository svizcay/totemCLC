using UnityEngine;
using System.Collections;

public class HeartController : MonoBehaviour {

	public int nrOverlayedItems = 0;
	public bool wasDiscovered = false;

	public bool shouldBeConsideredDiscovered = false;

//	private bool isFirstTime = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D other)
	{
//		if (isFirstTime) {
//			print ("executing heart controller onTriggerEnter method");
//			isFirstTime = false;
//		}
		if (other.tag == "Item") {
			nrOverlayedItems++;
		}
		// print ("nr overlayed items: " + nrOverlayedItems);
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Item") {
			shouldBeConsideredDiscovered = false;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Item") {
			nrOverlayedItems--;
		}
	}
}
