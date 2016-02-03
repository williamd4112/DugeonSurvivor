using UnityEngine;
using System.Collections;

public class MonsterAtt : MonoBehaviour {

	private Transform player;
	[SerializeField]
	private float PlayerDist;
	[SerializeField]
	private Vector3 m_pos;
	[SerializeField]
	private Animator m_ani;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		m_ani = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		m_pos = this.transform.position;
		PlayerDist = Vector3.Distance (player.position, m_pos);
		Debug.Log ("Distance = " + PlayerDist);
		if (PlayerDist <= 1.0f) {
			m_ani.SetTrigger ("Attack");
		}
			
	}
}
