using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipAnimation : MonoBehaviour {
	

	private Animator anim;

	public void Start()
	{
		anim = GetComponent<Animator> ();
	}
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.GetType() == typeof(BoxCollider2D))
		{
			Debug.Log ("left");
			anim.SetBool ("idle",false); //shootleft
		}
		if (collision.collider.GetType() == typeof(CircleCollider2D))
		{
			Debug.Log ("right");
			anim.SetBool ("shootright",true);
		}
	}

  
}
