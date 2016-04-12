using UnityEngine;
using System.Collections;
// using UnityEngine.SceneManagement; // in Unity > 5
using System.Diagnostics;
using UnityEngine.UI;

public class MainController : MonoBehaviour {

	// public static MainController mainController;

	public bool usingKinect = false;
	public bool takePicture = true;
	public int gameDuration = 60;
	public int timeAlert = 15;

	public int nrItems = 1000;
	public float maxPositiveHorizontalSize = 9;
	public float maxPositiveVerticalSize = 4;
	public int nrHearts = 3;

	public Animator clockAnimator;
	public Text timertext;

	public GameObject playAgainBtn;

	public GameObject heartPrefab;
	public GameObject emptyItemPrefab;

	public Transform heartsContainerTransform;
	public Transform itemsContainerTransform;

	public Sprite[] sprites;

	private int nrVisibleHearts = 0;
	private bool won = false;
	private bool gameOver = false;
	private bool isPlaying = false;		// allow items to be moved by user's interactions
	private bool standby = true;		// waiting for users

	private GameObject[] hearts;

	private float timeLeft;
	private bool wasClockAnimPlayed = false;

	private int playerLayer;
	private int itemsLayer;

//	private bool isFirstTime = true;

	// kinect related variables
	private KinectWrapper.NuiSkeletonPositionIndex trackedJoint = KinectWrapper.NuiSkeletonPositionIndex.HandRight;
	private KinectWrapper.NuiSkeletonPositionIndex leftHandJoint = KinectWrapper.NuiSkeletonPositionIndex.HandLeft;
	private int iTrackedJoint;
	private int iLeftHandJoint;
	public float smothFactor = 5.0f;
	public GameObject rightHandObject;
	public GameObject leftHandObject;
	
	private float distanceToCamera = 10.0f;

	private ProcessStartInfo processInfo;

	void ResetTimer()
	{
		timertext.text = gameDuration.ToString ();
		timeLeft = gameDuration;
	}

	void UpdateTimer()
	{
		if (!wasClockAnimPlayed && timeLeft < timeAlert) {
			clockAnimator.SetTrigger("alertTime");
			wasClockAnimPlayed = false;
		}
		timeLeft -= Time.deltaTime;
		timertext.text = timeLeft.ToString("0");
	}

	// Use this for initialization
	void Start () {
		
		ResetTimer ();
		
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

		if (usingKinect) {
			iTrackedJoint = (int)trackedJoint;
			iLeftHandJoint = (int)leftHandJoint;
		}

		playerLayer = LayerMask.NameToLayer ("Player");
		itemsLayer = LayerMask.NameToLayer ("Items");

		Physics2D.IgnoreLayerCollision(playerLayer, itemsLayer, false);

//		processInfo = new ProcessStartInfo ("sh", System.IO.Path.Combine(Application.streamingAssetsPath, "run.sh"))
//		{
//			CreateNoWindow = true,
//			WindowStyle = ProcessWindowStyle.Hidden,
//			UseShellExecute = false,
//			RedirectStandardOutput = true
//		};
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

	void Update() {
//		if (Input.anyKeyDown) {
//			
//		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (standby) {
			// waiting for input (allow user input)
			ResetTimer();
//			print ("in standby");
			if (!usingKinect && GameObject.Find ("TUIOHand") != null) {
				// start game
				standby = false;
				isPlaying = true;	// user is allowed to move items
				print ("game started");
			} else if (usingKinect) {
				KinectManager kinectManager = KinectManager.Instance;
				if (kinectManager && kinectManager.IsInitialized()) {
//					print("kinnect manager is initialized");
					if (kinectManager.IsUserDetected()) {
//						print ("user was detected");
						uint userId = kinectManager.GetPlayer1ID();
						trackJoint(kinectManager, userId, iTrackedJoint, rightHandObject);
						trackJoint(kinectManager, userId, iLeftHandJoint, leftHandObject);
						standby = false;
						isPlaying = true;	// user is allowed to move items
						print ("game started");
					}

				}
			} else {
//				print ("waiting for user");
			}
		} else {
			// user is interacting with kinect or tuio
			if (isPlaying) {
				if (Input.GetKeyDown(KeyCode.A)){
					UnityEngine.Debug.Log("nr items above each heart:");
					for (int i = 0; i < nrHearts; i++) {
						HeartController heartController = hearts [i].GetComponent<HeartController> (); 
						UnityEngine.Debug.Log(heartController.nrOverlayedItems);
					}
					print ("nr corazones descubiertos" + nrVisibleHearts);

				}
				// check game over
				if (timeLeft <= 0) {
					gameOver = true;
				} else {
					// check if player won
//					print("mainController: antes de contar corazones");
					CountNrVisibleHearts();
					MarkHeartsForCheck();
//					print("mainController: despues de setear corazones a true");
					if (nrVisibleHearts == nrHearts) {
						won = true;
					}
					
					UpdateTimer();

				}

				isPlaying = !gameOver && !won;

//				print ("is playing: " + isPlaying);
				if (!isPlaying) {
					// take picture with gopro
					if (takePicture) {
//						Process process = Process.Start (processInfo);
					}
					UnityEngine.Debug.Log ("retorno de la toma de foto");

					// do not allow collision between hands and items anymore
					Physics2D.IgnoreLayerCollision(playerLayer, itemsLayer);
					playAgainBtn.SetActive(true);
				}
			} else {
				// user is not allowed to move items anymore but can still click on playAgainBtn
//				print ("waiting for user to press play again button");
			}

			// update player's hands
			if (usingKinect) {
				KinectManager kinectManager = KinectManager.Instance;
				if (kinectManager && kinectManager.IsInitialized()) {
					//					print("kinnect manager is initialized");
					if (kinectManager.IsUserDetected()) {
						//						print ("user was detected");
						uint userId = kinectManager.GetPlayer1ID();
						trackJoint(kinectManager, userId, iTrackedJoint, rightHandObject);
						trackJoint(kinectManager, userId, iLeftHandJoint, leftHandObject);
					}
				}
			}
		}
	}

	void LateUpdate()
	{
//		MarkHeartsForCheck();
	}

	void CountNrVisibleHearts()
	{
		for (int i = 0; i < nrHearts; i++) {
			CircleCollider2D heartCollider = hearts [i].GetComponent<CircleCollider2D> ();
			if (heartCollider) {
				HeartController heartController = hearts [i].GetComponent<HeartController> (); 
				if (!heartController.wasDiscovered && heartController.nrOverlayedItems == 0 || !heartController.wasDiscovered && heartController.shouldBeConsideredDiscovered) {
					// heart is visible
					nrVisibleHearts++;
					heartCollider.isTrigger = false;
					heartController.wasDiscovered = true;
					hearts[i].GetComponent<Animator>().SetTrigger("wasDiscovered");
					heartController.shouldBeConsideredDiscovered = false;
					print ("se descubrio el corazon " + i);
				}
			}
		}
	}

	void MarkHeartsForCheck()
	{
		for (int i = 0; i < nrHearts; i++) {
			HeartController heartController = hearts [i].GetComponent<HeartController> (); 
			heartController.shouldBeConsideredDiscovered = true;
		}
	}

	public static void RestartGame()
	{
		// SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // in Unity > 5
		Application.LoadLevel (Application.loadedLevel);
	}
}
