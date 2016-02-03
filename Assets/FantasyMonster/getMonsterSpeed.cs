using UnityEngine;
using System.Collections;

public class getMonsterSpeed : MonoBehaviour {
	[SerializeField]
	private Vector3 monsterSpeed;
	[SerializeField]
	private float trueSpeed;
	[SerializeField]
	private NavMeshAgent m_Nav;
	[SerializeField]
	private Animator m_ani;
	// Use this for initialization
	void Start () {
		m_Nav = this.GetComponent<NavMeshAgent> ();
		m_ani = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		monsterSpeed = m_Nav.velocity;
		trueSpeed = monsterSpeed.magnitude;
		m_ani.SetFloat ("Speed", trueSpeed);

	}
}
