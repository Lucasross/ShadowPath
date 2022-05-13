using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController2D : MonoBehaviour
{
	private Animator anim;
	private MovementController2D move;
	private Rigidbody2D rb;
	private CharacterCollision2D coll;
	[HideInInspector]
	public SpriteRenderer sr;

	void Start()
	{
		anim = GetComponent<Animator>();
		coll = GetComponentInParent<CharacterCollision2D>();
		move = GetComponentInParent<MovementController2D>();
		rb = GetComponentInParent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
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

		if (move.xRaw > 0)
			FlipRight();

		else if (move.xRaw < 0)
			FlipLeft();

		else if (rb.velocity.x > 1f)
			FlipRight();

		else if (rb.velocity.x < -1f)
			FlipLeft();
	}

	public void SetTrigger(string trigger)
	{
		anim.SetTrigger(trigger);
	}

	public void FlipRight() => Flip(false);
	public void FlipLeft() => Flip(true);

	public void Flip(bool flipX)
	{
		sr.flipX = flipX;
	}
}