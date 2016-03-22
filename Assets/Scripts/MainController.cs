using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour {

	public int nrItems = 1000;
	public int maxPositiveHorizontalSize = 9;
	public int maxPositiveVerticalSize = 4;
	public int nrHearts = 3;

	public GameObject heartPrefab;
	public GameObject emptyItemPrefab;

	public Transform heartsContainerTransform;
	public Transform itemsContainerTransform;

	public Sprite[] sprites;

	private int nrVisibleHearts = 0;
	private bool won = false;
	private bool isPlaying = true;

	private GameObject[] hearts;

	// Use this for initialization
	void Start () {
		// instantiate hearts
		hearts = new GameObject[nrHearts];
		for (int i = 0; i < nrHearts; i++) {
			Vector2 randomPosition = new Vector2 (Random.Range (-maxPositiveHorizontalSize, maxPositiveHorizontalSize+1), Random.Range (-maxPositiveVerticalSize, maxPositiveVerticalSize));
			GameObject newHeart = Instantiate (heartPrefab, randomPosition, Quaternion.identity) as GameObject;
			newHeart.transform.parent = heartsContainerTransform;
			hearts [i] = newHeart;
		}

		// instantiate items
		for (int i = 0; i < nrItems; i++) {
			int randomSpriteNr = Random.Range (0, sprites.Length);
			Sprite randomSprite = sprites [randomSpriteNr];
			Vector2 randomPosition = new Vector2 (Random.Range (-maxPositiveHorizontalSize, maxPositiveHorizontalSize+1), Random.Range (-maxPositiveVerticalSize, maxPositiveVerticalSize));
			GameObject newPrefab = Instantiate (emptyItemPrefab, randomPosition, Quaternion.identity) as GameObject;
			SpriteRenderer spriteRenderer = newPrefab.GetComponent<SpriteRenderer> ();
			spriteRenderer.sprite = randomSprite;
			newPrefab.transform.parent = itemsContainerTransform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isPlaying) {
			// check if a new heart is visible
			for (int i = 0; i < nrHearts; i++) {
				BoxCollider2D heartCollider = hearts [i].GetComponent<BoxCollider2D> ();
				if (heartCollider) {
					HeartController heartController = hearts [i].GetComponent<HeartController> (); 
					if (heartController.nrOverlayedItems == 0) {
						// heart is visible
						nrVisibleHearts++;
						print ("heart is now visible");
						heartCollider.isTrigger = false;
					}
				}


			}

			if (nrVisibleHearts == nrHearts) {
				won = true;
				isPlaying = false;
			}
		}
	}
}
