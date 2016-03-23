using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	Rigidbody2D rigidbody;
	void Start () {
		rigidbody = GetComponent<Rigidbody2D> ();
		// rigidbody.AddForce (new Vector2(-100, 50));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		Vector2 velocity = collision.relativeVelocity;
//		print ("PlayerController velocity: " + velocity);
		// rigidbody.AddForce (velocity);
	}
}
