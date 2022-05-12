using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class MovementController2D : MonoBehaviour
{
	private CharacterCollision2D coll;
	private Rigidbody2D rb;

	[Space]
	[Header("Stats")]
	public float speed = 10;
	public float jumpForce = 500;
	public float wallJumpForce = 500;
	public float wallSlideSpeed = 5;
	public float dashSpeed = 20;

	public float x, y, xRaw, yRaw;
	public bool jump, wallJump, wallSlide, xButton;

	public Timer jumpInputTimer;
	public Timer wallJumpIncapacityTimer;

	public bool isRunning => xButton;

	private void Awake()
	{
		CharacterCollision2D.OnTouchGround += OnTouchGround;
	}

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		coll = GetComponent<CharacterCollision2D>();
		
		jumpInputTimer = TimerUtility.Create(0.2f);
		jumpInputTimer.OnTimerEnd += () => jump = false;

		wallJumpIncapacityTimer = TimerUtility.Create(0.5f);
	}

	private void Update()
	{
		x = Input.GetAxis("Horizontal");
		y = Input.GetAxis("Vertical");
		xRaw = Input.GetAxisRaw("Horizontal");
		yRaw = Input.GetAxisRaw("Vertical");
		xButton = Input.GetButton("Horizontal");

		wallSlide = !coll.onGround && (coll.onLeftWall && xRaw < 0) || (coll.onRightWall && xRaw > 0);

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;

			jumpInputTimer.Start();

			if (coll.onWall)
				wallJumpIncapacityTimer.Start();
		}

		jumpInputTimer.Update();
		wallJumpIncapacityTimer.Update();
	}

	void FixedUpdate()
	{
		Run();

		if (jump && coll.onGround)
			Jump();

		if (wallSlide)
			WallSlide();

		if (jump && coll.onWall)
			WallJump();
	}

	private void Run()
	{
		if (wallJumpIncapacityTimer.done)
			rb.velocity = new Vector2(x * speed, rb.velocity.y);

		else
			rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, x * speed, 0.1f), rb.velocity.y);
	}

	private void Jump()
	{
		rb.velocity = new Vector2(rb.velocity.x, jumpForce);
		jump = false;
	}

	private void WallSlide()
	{
		if (wallJumpIncapacityTimer.done)
			rb.velocity = new Vector2(0, -wallSlideSpeed);
	}

	private void WallJump()
	{
		float xDir = coll.onRightWall ? -1f : 1f;

		rb.velocity = new Vector2(xDir, 1f) * wallJumpForce;

		jump = false;
	}

	// EVENTS

	private void OnTouchGround()
	{
	}
}