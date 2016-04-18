using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StandBySceneController : MonoBehaviour {

	public Transform itemsContainerTransform;
	public GameObject emptyItemPrefab;
	public Sprite[] sprites;

	public float speed = 10.0f;

	private int nrItemsPerCol = 7;
	private float leftColPosition = -8;
	private float rightColPosition = 8;
	private float verticalLimit = 8;
	private List<GameObject> leftItems;
	private List<GameObject> rightItems;
	// Use this for initialization

	void createItemsInCol(bool isLeftCol)
	{
		for (int i = 0; i < nrItemsPerCol; i++) {
			
			int randomSpriteNr = Random.Range (0, sprites.Length);
			Sprite randomSprite = sprites [randomSpriteNr];
			
			// left col
			Vector2 position = new Vector2();
			if (isLeftCol) {
				position = new Vector2(leftColPosition, 2 * i - verticalLimit);
			} else {
				position = new Vector2(rightColPosition, 2 * i - verticalLimit);
			}

			GameObject newPrefab = Instantiate (emptyItemPrefab, position, Quaternion.identity) as GameObject;
			SpriteRenderer spriteRenderer = newPrefab.GetComponent<SpriteRenderer> ();
			spriteRenderer.sprite = randomSprite;

			newPrefab.transform.parent = itemsContainerTransform;
			if (isLeftCol) {
				leftItems.Add(newPrefab);
			} else {
				rightItems.Add(newPrefab);
			}
		}
	}
	void Start () {
		leftItems = new List<GameObject> ();
		rightItems = new List<GameObject> ();
		createItemsInCol (true);
		createItemsInCol (false);

	}

	// Update is called once per frame
	void Update () {
//		if (Input.anyKeyDown) {
		if (Input.GetKeyDown(KeyCode.A)) {

			Application.LoadLevel ("main");
		}


		foreach (var gameObject in leftItems) {
			gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, new Vector2(leftColPosition, verticalLimit), Time.deltaTime * speed);
			if (gameObject.transform.position.y > verticalLimit - 1) {
				Vector2 newPosition = new Vector2(gameObject.transform.position.x, -verticalLimit);
				gameObject.transform.position = newPosition;
			}
		}

		foreach (var gameObject in rightItems) {
			gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, new Vector2(rightColPosition, -verticalLimit), Time.deltaTime * speed);
			if (gameObject.transform.position.y < -verticalLimit + 2) {
				Vector2 newPosition = new Vector2(gameObject.transform.position.x, verticalLimit);
				gameObject.transform.position = newPosition;
			}
		}
	}
}
