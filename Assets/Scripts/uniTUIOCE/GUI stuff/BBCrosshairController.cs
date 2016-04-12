using UnityEngine;
using System.Collections;

// this object just handles the crosshairs that show up under each touch event

public class BBCrosshairController : MonoBehaviour {

	public GameObject crosshairPrefab;
	// public BBInputDelegate eventManager;
	// 
	private ArrayList crosshairs = new ArrayList();
	private Camera renderingCamera;
	
	void Start()
	{
		renderingCamera = Camera.main;
		if (renderingCamera == null) {
			// someone didnt tag their cameras properly!!
			// just grab the first one
			if (Camera.allCameras.Length == 0) return;
			renderingCamera = Camera.allCameras[0];
		}
	}
	
	// we go through each touch input and place a crosshair at it's position.
	// we save a list of crosshairs and deactivate them when they are not
	// being used.
	void FixedUpdate () {
	 	int crosshairIndex = 0;
		int i;
//		print ("nr touch events: " + iPhoneInput.touchCount);
		for (i = 0; i < iPhoneInput.touchCount; i++) {
			if (crosshairs.Count <= crosshairIndex) {
				// make a new crosshair and cache it
				GameObject newCrosshair = (GameObject)Instantiate (crosshairPrefab, Vector3.zero, Quaternion.identity);
				newCrosshair.name = crosshairPrefab.name;
				crosshairs.Add(newCrosshair);
//				print ("instanciando manito");
			}
			iPhoneTouch touch = iPhoneInput.GetTouch(i);
			Vector3 screenPosition = new Vector3(touch.position.x,touch.position.y,0.0f);
			GameObject thisCrosshair = (GameObject)crosshairs[crosshairIndex];
			//thisCrosshair.SetActiveRecursively(true);
			thisCrosshair.GetComponent<CircleCollider2D>().enabled = true;
			thisCrosshair.SetActive(true);
			//thisCrosshair.transform.position = renderingCamera.ScreenToViewportPoint(screenPosition * 4f);
			Vector2 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 10f));
//			thisCrosshair.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 10f));
//			thisCrosshair.transform.position = new Vector3(thisCrosshair.transform.position .x, thisCrosshair.transform.position .y, -1f);
//			thisCrosshair.GetComponent<Rigidbody2D>().position = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 10f));
			thisCrosshair.GetComponent<Rigidbody2D>().MovePosition(newPosition);
			crosshairIndex++;
		}
		
		// if there are any extra ones, then shut them off
		for (i = crosshairIndex; i < crosshairs.Count; i++) {
			GameObject thisCrosshair = (GameObject)crosshairs[i];
			//thisCrosshair.SetActiveRecursively(false);
			thisCrosshair.GetComponent<CircleCollider2D>().enabled = false;
			thisCrosshair.SetActive(false);
		}
	}
}
