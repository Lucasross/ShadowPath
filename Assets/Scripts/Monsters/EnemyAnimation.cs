using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
	public Rigidbody2D rb;
	public SpriteRenderer sprite;
	public Animator anim;

	void Update()
	{
		UpdateAnimation();

		if (rb.velocity.x >= 0.01f)
			FlipRight();

		else if(rb.velocity.x <= 0.01f)
			FlipLeft();
	}

	private void UpdateAnimation()
	{
		anim.SetFloat("velocity", Mathf.Abs(rb.velocity.x));
	}

	private void FlipLeft() => Flip(true);
	private void FlipRight() => Flip(false);

	private void Flip(bool isFlip)
	{
		sprite.flipX = isFlip;
	}

}
