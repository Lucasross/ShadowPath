using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController2D : MonoBehaviour
{
	private Animator anim;
	private MovementController2D move;
	private AttackController2D attack;
	private Rigidbody2D rb;
	private CharacterCollision2D coll;
	private Entity2D entity;
	[HideInInspector]
	public SpriteRenderer sr;

	void Start()
	{
		anim = GetComponent<Animator>();
		coll = GetComponent<CharacterCollision2D>();
		move = GetComponent<MovementController2D>();
		attack = GetComponent<AttackController2D>();
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		entity = GetComponent<Entity2D>();
	}

	void Update()
	{
		anim.SetBool("onGround", coll.onGround);
		anim.SetBool("onWall", coll.onWall);
		anim.SetBool("wallSlide", move.wallSlide && move.enabledWallSlide);
		anim.SetBool("isRunning", move.isRunning);
		anim.SetFloat("Horizontal", rb.velocity.x);
		anim.SetFloat("Vertical", rb.velocity.y);
		anim.SetBool("dashing", !move.dashDuration.done);
		anim.SetBool("isAttacking", attack.isAttacking);

		if (move.xRaw > 0)
			FlipRight();

		else if (move.xRaw < 0)
			FlipLeft();

		else if (rb.velocity.x > 1f)
			FlipRight();

		else if (rb.velocity.x < -1f)
			FlipLeft();
	}

	public void SetTrigger(string trigger) => anim.SetTrigger(trigger);
	public void ResetTrigger(string trigger) => anim.ResetTrigger(trigger);

	public void FlipRight() => Flip(false);
	public void FlipLeft() => Flip(true);

	public void Flip(bool flipX)
	{
		entity.side = flipX ? -1 : 1;
		sr.flipX = flipX;
	}
}