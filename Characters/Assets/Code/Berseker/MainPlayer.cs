using UnityEngine;
using System.Collections;

public class MainPlayer : MonoBehaviour {

	public float speedMov = 5.0f;

	private CharacterController charController;
	private Animator anim;
	// Use this for initialization
	void Start () {
		charController = GetComponent<CharacterController> ();
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		float horInput = Input.GetAxis ("Horizontal");
		Vector3 movement = new Vector3 (0.0f, 0.0f, horInput * speedMov*Time.deltaTime);
		charController.Move(movement);
		anim.SetFloat ("speed", horInput);
	}
}
