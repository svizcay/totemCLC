using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour {

	// Use this for initialization
	Rigidbody2D rigidbody;
	void Start () {
		rigidbody = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		Vector2 velocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
		print ("ItemController velocity: " + velocity);
		rigidbody.AddForce (velocity);
	}
}
