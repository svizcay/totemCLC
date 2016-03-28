using UnityEngine;
using System.Collections;

public class PieSlide : MonoBehaviour {

	public Vector2 direction;
	public float speed;
	public float directionDetectRate;
	public Vector2 velocity
	{
		get {
			StartCoroutine(Sleep());
			return _velocity;
		}
	}

	public bool sleeping
	{
		get {return _sleeping;}
	}

	private float _directionDetectRate;
	private Vector2 startPos;
	private Vector2 endPos;
	private Vector2 _velocity;
	private bool _sleeping;

	// Use this for initialization
	void Start () {
		_directionDetectRate = directionDetectRate;
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.A)) {
//			MyDebug.debugOn = !MyDebug.debugOn;
		}

//		GetComponent<SpriteRenderer> ().enabled = MyDebug.debugOn;

		DirectionDetectTimer ();
	}

	IEnumerator Sleep()
	{
		_sleeping = true;
		yield return new WaitForSeconds(0.2f);
		_sleeping = false;
	}

	void DirectionDetectTimer()
	{
		endPos = transform.position;
		_directionDetectRate -= Time.deltaTime;
		if (_directionDetectRate <= 0f)
		{
			SetDirection();
			startPos = transform.position;
			_directionDetectRate = directionDetectRate;
		}
	}

	void SetDirection()
	{
		direction = endPos - startPos; 
		speed = direction.magnitude;
		_velocity = speed * direction;
	}

}
