using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour {

	public enum State{
		Moving,
		Waiting,
		Rotating,

		Count
	}
	public Transform [] path;
	public float speed = 4.0f;
	public float distToReachWaypoint = 0.5f;
	public float timeWaiting = 1.0f;
	public float angSpeed = 90.0f;

	private int currentWp = 0;
	private Animator anim;
	private Rigidbody rb;
	private State currentState = State.Moving;
	private float timer = 0.0f;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		float dt = Time.deltaTime;
		switch (currentState) {
		case State.Moving:
			updateMoving ();
			break;
		case State.Waiting:
			updateWaiting ();
			break;
		case State.Rotating:
			updateRotating (dt);
			break;
		default:
			Debug.LogError ("Unknown state " + currentState);
			break;
		}

	}
	public void updateMoving(){
		Vector3 dir = path [currentWp].position - transform.position;
		rb.velocity = dir.normalized * speed;
		transform.forward = dir.normalized;
		rb.angularVelocity = new Vector3 (0.0f, 0.0f, 0.0f);
		float magnitudeToNextUp = dir.magnitude;

		if (magnitudeToNextUp <= distToReachWaypoint) {
			currentWp = (currentWp + 1) % path.Length;
			rb.velocity = new Vector3 (0.0f, 0.0f, 0.0f);
			currentState = State.Waiting;
		}
		anim.SetFloat ("speed", speed);
	}
	public void updateWaiting(){
		timer += Time.deltaTime;
		if (timer >= timeWaiting) {
			timer = 0.0f;
			currentState = State.Rotating;
		}
		anim.SetFloat ("speed", 0.0f);
	}
	public void updateRotating(float dt){
		//calculate the directione we're looking at and the direction the target is
		Vector3 target = path [currentWp].transform.position;
		Vector3 pos = transform.position;
		Vector3 dir = (target - pos).normalized;
		Vector3 fwd = transform.forward;

		//remove the Y component and normalized
		dir.y = 0.0f;
		fwd.y = 0.0f;
		dir.Normalize ();
		fwd.Normalize ();

		//calculate the angle between these 2 vectors in degrees
		float iniAngle = Mathf.Rad2Deg * Mathf.Atan2(fwd.z, fwd.x);
		float endAngle = Mathf.Rad2Deg * Mathf.Atan2(dir.z, dir.x);
		float angle = NormalizeAngleDegs(endAngle-iniAngle);

		//modulate the angle we're going to move with the angular speed
		float maxAngleOffset = angSpeed *dt;
		float angleOffset = Mathf.Clamp (angle, -maxAngleOffset, maxAngleOffset);

		//finally rotate
		transform.RotateAround (transform.position, new Vector3(0.0f, 1.0f, 0.0f), -angleOffset);

		//change state
		if (Mathf.Abs (angle) <= maxAngleOffset) {
			currentState = State.Moving;
		}
	}
	private float NormalizeAngleDegs(float angle)
	{
		while (angle < -180.0f)
			angle += 360.0f;
		while (angle > 180.0f)
			angle -= 360.0f;
		return angle;
	}
}
