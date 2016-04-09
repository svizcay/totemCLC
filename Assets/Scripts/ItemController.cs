using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour {

	public float force = 80.0f;
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
//		Vector2 velocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
//		print ("ItemController velocity: " + velocity);
//		rigidbody.AddForce (velocity);
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player") {
			Vector3 delta = transform.position - other.transform.position;
			delta.z = 0f;
			if (delta.magnitude > 0.0f) {
				if(other.gameObject.GetComponent<PieSlide>().sleeping) {
					//					print ("tuio hand is sleeping");
					return;
				}
				Vector3.Normalize (delta);
				
				//rigidbody2D.velocity = delta * force;
				Vector2 vel = other.gameObject.GetComponent<PieSlide>().velocity * force;
				
				if(vel.magnitude > 100)
				{
					//	print("maxi!!!");
					vel.Normalize();
					vel *= 100;
				}

				print ("on trigger stay!! vel: " + vel + " en " + GetComponent<SpriteRenderer>().sprite.name);
				
				GetComponent<Rigidbody2D>().velocity = vel;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
//		print ("trigger item");
		if (other.tag == "Player") {
//			print ("OnTriggerEnter item + player");
//			if (!GameController.instance.inMatch && GetComponent<Rigidbody2D>().angularVelocity == 0f) {
//				GameController.instance.FirstKick ();
//				return;
//			}
			
			Vector3 delta = transform.position - other.transform.position;
			delta.z = 0f;
			
			//si la magnitud de la direccion es suficiente, golpeamos la pelota,
			//si no, activamos el grab
			if (delta.magnitude > 0.0f) {
				if(other.gameObject.GetComponent<PieSlide>().sleeping) {
//					print ("tuio hand is sleeping");
					return;
				}
//				print ("tuio hand is awake");
					
//				grabbingBall = false;
				
				Vector3.Normalize (delta);
				
				//rigidbody2D.velocity = delta * force;
				Vector2 vel = other.gameObject.GetComponent<PieSlide>().velocity * force;
				
				if(vel.magnitude > 100)
				{
					//	print("maxi!!!");
					vel.Normalize();
					vel *= 100;
				}

				GetComponent<Rigidbody2D>().velocity = vel;
				
//				//para manejar el rebote
//				if (activarRebote)
//				{
//					if (vel == Vector2.zero)
//					{
//						vel = delta * GetComponent<Rigidbody2D>().velocity.magnitude * 0.5f;
//						activarRebote = false;
//					}
//					
//					if (vel.magnitude < 40f)
//					{
//						GetComponent<Rigidbody2D>().velocity = vel;
//					} else {
//						vel.Normalize();
//						GetComponent<Rigidbody2D>().velocity = vel * 40;
//					}
//				}
				
				/*if (vel.magnitude < 40f)
				{
					rigidbody2D.velocity = vel;
					print(vel);
				} else {
					vel.Normalize();
					rigidbody2D.velocity = vel * 40;
				}*/
			}

		}

	}
}
