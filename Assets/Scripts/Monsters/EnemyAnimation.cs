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
	public EnemyNavigation navigation;

	public Transform target => navigation.target.transform;

	void Update()
	{
		UpdateAnimation();

		if(rb.velocity.x == 0)
		{
			if (rb.position.x < target.position.x)
				FlipRight();
			else if (rb.position.x > target.position.x)
				FlipLeft();

			return;
		}


		if (rb.velocity.x >= 0.05f)
			FlipRight();

		else if (rb.velocity.x <= -0.05f)
			FlipLeft();
	}

	private void UpdateAnimation()
	{
		anim.SetBool("isRunning", navigation.IsRunning);
	}

	private void FlipLeft() => Flip(true);
	private void FlipRight() => Flip(false);

	private void Flip(bool isFlip)
	{
		sprite.flipX = isFlip;
	}
}
