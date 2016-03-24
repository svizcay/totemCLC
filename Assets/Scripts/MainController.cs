using UnityEngine;
using System.Collections;
// using UnityEngine.SceneManagement; // in Unity > 5

public class MainController : MonoBehaviour {

	// public static MainController mainController;

	public int gameDuration = 60;
	public int timeAlert = 15;

	public int nrItems = 1000;
	public float maxPositiveHorizontalSize = 9;
	public float maxPositiveVerticalSize = 4;
	public int nrHearts = 3;

	public Animator clockAnimator;
	public TextMesh timertext;

	public GameObject playAgainBtn;

	public GameObject heartPrefab;
	public GameObject emptyItemPrefab;

	public Transform heartsContainerTransform;
	public Transform itemsContainerTransform;

	public Sprite[] sprites;

	private int nrVisibleHearts = 0;
	private bool won = false;
	private bool gameOver = false;
	private bool isPlaying = true;

	private GameObject[] hearts;

	private float timeLeft;
	private bool wasClockAnimPlayed = false;

//	private bool isFirstTime = true;

	// kinect related variables
	public KinectWrapper.NuiSkeletonPositionIndex trackedJoint = KinectWrapper.NuiSkeletonPositionIndex.HandRight;
	private KinectWrapper.NuiSkeletonPositionIndex leftHandJoint = KinectWrapper.NuiSkeletonPositionIndex.HandLeft;
	public float smothFactor = 5.0f;
	public GameObject rightHandObject;
	public GameObject leftHandObject;
	
	private float distanceToCamera = 10.0f;

	// Use this for initialization
	void Start () {
//		print ("MainController: Start method");
//		print ("MainController: nr hearts visible: " + nrVisibleHearts);
//		print ("MainController: game over: " + gameOver);
//		print ("MainController: won: " + won);
		timertext.text = gameDuration.ToString ();
		timeLeft = gameDuration;

		// disable playAgainBtn
		playAgainBtn.SetActive (false);
		// instantiate hearts
		hearts = new GameObject[nrHearts];
		for (int i = 0; i < nrHearts; i++) {
			Vector2 randomPosition = new Vector2 (Random.Range (-maxPositiveHorizontalSize, maxPositiveHorizontalSize), Random.Range (-maxPositiveVerticalSize, maxPositiveVerticalSize));
			GameObject newHeart = Instantiate (heartPrefab, randomPosition, Quaternion.identity) as GameObject;
			newHeart.transform.parent = heartsContainerTransform;
			hearts [i] = newHeart;
		}

		// instantiate items
		for (int i = 0; i < nrItems; i++) {
			int randomSpriteNr = Random.Range (0, sprites.Length);
			Sprite randomSprite = sprites [randomSpriteNr];
			Bounds spriteBounds = randomSprite.bounds;
			Vector2 randomPosition = new Vector2 (Random.Range (-maxPositiveHorizontalSize, maxPositiveHorizontalSize), Random.Range (-maxPositiveVerticalSize, maxPositiveVerticalSize));
			GameObject newPrefab = Instantiate (emptyItemPrefab, randomPosition, Quaternion.identity) as GameObject;
			SpriteRenderer spriteRenderer = newPrefab.GetComponent<SpriteRenderer> ();
			spriteRenderer.sprite = randomSprite;
			BoxCollider2D collider = newPrefab.GetComponent<BoxCollider2D>();
			collider.size = spriteBounds.size;
			newPrefab.transform.parent = itemsContainerTransform;
		}

		// kinect related initialization
		distanceToCamera = (rightHandObject.transform.position - Camera.main.transform.position).magnitude;
	}

	void trackJoint(KinectManager kinectManager, uint userId, int iTrackedJoint, GameObject hand)
	{
		if (kinectManager.IsJointTracked(userId, iTrackedJoint)) {
			Vector3 posJoint = kinectManager.GetRawSkeletonJointPos(userId, iTrackedJoint);
			if (posJoint != Vector3.zero) {
				// get 3d position to depth
				Vector2 posDepth = kinectManager.GetDepthMapPosForJointPos(posJoint);

				// depth pos to color pos
				Vector2 posColor = kinectManager.GetColorMapPosForDepthPos(posDepth);
				
				float scaleX = (float)posColor.x / KinectWrapper.Constants.ColorImageWidth;
				float scaleY = 1.0f - (float)posColor.y / KinectWrapper.Constants.ColorImageHeight;
				
				Vector3 newPosition = Camera.main.ViewportToWorldPoint(new Vector3(scaleX, scaleY, distanceToCamera));
				hand.transform.position = Vector3.Lerp(hand.transform.position, newPosition, smothFactor * Time.deltaTime);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
//		if (isFirstTime) {
//			print ("executing main controller update method");
//			isFirstTime = false;
//		}
		KinectManager kinectManager = KinectManager.Instance;
		if (!gameOver && !won) {
			if (timeLeft <= 0) {
				isPlaying = false;
				gameOver = true;
			} else {
				if (!wasClockAnimPlayed && timeLeft < timeAlert) {
					clockAnimator.SetTrigger("alertTime");
					wasClockAnimPlayed = false;
				}
				timeLeft -= Time.deltaTime;
				timertext.text = timeLeft.ToString("0");
			}

			if (isPlaying) {
				// update hands
				if (kinectManager && kinectManager.IsInitialized()) {
//					print("kinnect manager is initialized");
					int iTrackedJoint = (int)trackedJoint;
					int iLeftHandJoint = (int)leftHandJoint;
					if (kinectManager.IsUserDetected()) {
//						print ("user was detected");
						uint userId = kinectManager.GetPlayer1ID();
						trackJoint(kinectManager, userId, iTrackedJoint, rightHandObject);
						trackJoint(kinectManager, userId, iLeftHandJoint, leftHandObject);
//						if (kinectManager.IsJointTracked(userId, iTrackedJoint)) {
////							print ("right hand is being tracked");
//							Vector3 posJoint = kinectManager.GetRawSkeletonJointPos(userId, iTrackedJoint);
//							print ("posJoint: " + posJoint);
//							if (posJoint != Vector3.zero) {
////								print ("there was a movement");
//								// get 3d position to depth
//								Vector2 posDepth = kinectManager.GetDepthMapPosForJointPos(posJoint);
//
//								print("before depth pos to color pos " + posDepth);
//								print("user depth map: " + kinectManager.GetRawDepthMap());
//								// depth pos to color pos
//								Vector2 posColor = kinectManager.GetColorMapPosForDepthPos(posDepth);
//								print("before kinect wrapper constants");
//
//								float scaleX = (float)posColor.x / KinectWrapper.Constants.ColorImageWidth;
//								float scaleY = 1.0f - (float)posColor.y / KinectWrapper.Constants.ColorImageHeight;
//
//								Vector3 newPosition = Camera.main.ViewportToWorldPoint(new Vector3(scaleX, scaleY, distanceToCamera));
//								print ("new position: " + newPosition);
//								rightHandObject.transform.position = Vector3.Lerp(rightHandObject.transform.position, newPosition, smothFactor * Time.deltaTime);
//
//							}
//
//						}
					}

				}
				// check if a new heart is visible
				for (int i = 0; i < nrHearts; i++) {
					CircleCollider2D heartCollider = hearts [i].GetComponent<CircleCollider2D> ();
					if (heartCollider) {
						HeartController heartController = hearts [i].GetComponent<HeartController> (); 
						if (!heartController.wasDiscovered && heartController.nrOverlayedItems == 0) {
							// heart is visible
							nrVisibleHearts++;
//							print ("heart is now visible");
							heartCollider.isTrigger = false;
							heartController.wasDiscovered = true;
							hearts[i].GetComponent<Animator>().SetTrigger("wasDiscovered");
						}
					}
					
					
				}
				
				if (nrVisibleHearts == nrHearts) {
					won = true;
					isPlaying = false;
				}
			}
		}

		if (gameOver || won) {
			playAgainBtn.SetActive(true);
		}

	}

	public static void RestartGame()
	{
		// SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // in Unity > 5
//		print ("MainController: restarting game");
		Application.LoadLevel (Application.loadedLevel);
//		print ("MainController: after loading the scene again");
	}
}
