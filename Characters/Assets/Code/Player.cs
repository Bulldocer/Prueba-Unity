using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float movVel = 5.0f;

	private Animator anim;
	private Rigidbody rb;
	private int idleHash = Animator.StringToHash ("Idle");
	private int forwardHash = Animator.StringToHash("WalkForward");
	private int backHash = Animator.StringToHash("WalkBackward");
	private int punchHash = Animator.StringToHash("Punch");
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		float horInput = Input.GetAxis ("Horizontal");
		bool punch = Input.GetKeyDown (KeyCode.Space);
		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo (0);

		if (horInput < 0) {
			anim.SetBool("Walk Backward", true);
			Vector3 movement = new Vector3 (0.0f, 0.0f, horInput * movVel);
			rb.velocity = movement;
		}




		if (horInput < 0 && (stateInfo.nameHash == idleHash ||stateInfo.nameHash == forwardHash)) {
			anim.SetTrigger (backHash);
			Vector3 movement = new Vector3 (0.0f, 0.0f, horInput * movVel);
			rb.velocity = movement;
		} else {
			if (horInput > 0 && (stateInfo.nameHash == idleHash || stateInfo.nameHash == backHash)) {
				anim.SetTrigger (forwardHash);
				Vector3 movement = new Vector3 (0.0f, 0.0f, horInput * movVel);
				rb.velocity = movement;
			} else {
				if (horInput == 0 && (stateInfo.nameHash == forwardHash || stateInfo.nameHash == backHash)) {
					anim.SetTrigger (idleHash);
					Vector3 movement = new Vector3 (0.0f, 0.0f, 0.0f);
					rb.velocity = movement;
				}
			}
		}
		if (punch && stateInfo.nameHash == idleHash) {
			anim.SetTrigger (punchHash);
		}



	}
}
