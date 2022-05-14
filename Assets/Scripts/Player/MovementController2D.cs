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

	[Space, Header("Ability")]
	public bool enabledWallJump = true;
	public bool enabledWallSlide = true;
	public bool enabledDash = true;
	public int airJumpQty = 1;
	public int postDashJumpQty = 1;
	private int postDashJumpCurrent;
	private int airJumpCurrent;

	[Space, Header("Resources")]
	public GameObject airJumpPrefab;

	[HideInInspector] public float x, y, xRaw, yRaw;
	[HideInInspector] public bool jump, wallJump, wallSlide, xButton, special;

	[HideInInspector] public Timer jumpInputTimer;
	[HideInInspector] public Timer wallJumpIncapacityTimer;
	[HideInInspector] public Timer dashCooldown;
	[HideInInspector] public Timer dashDuration;

	public bool isRunning => xButton;

	private Vector2 beforeDashVelocity;

	private void Awake()
	{
		CharacterCollision2D.OnTouchGround += OnTouchGround;
	}

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		coll = GetComponent<CharacterCollision2D>();

		jumpInputTimer = TimerUtility.Create(0.2f).OnEnd(() => jump = false);
		wallJumpIncapacityTimer = TimerUtility.Create(0.5f);

		dashCooldown = TimerUtility.Create(1f);
		dashDuration = TimerUtility.Create(0.3f).OnEnd(() =>
		{
			rb.velocity = beforeDashVelocity;
			
			if(!coll.onGround)
			{
				airJumpCurrent += postDashJumpCurrent;
				postDashJumpCurrent -= postDashJumpQty;
			}
		});
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

		if (Input.GetButtonDown("Special") && dashCooldown.done && xButton)
		{
			special = true;
		}

		jumpInputTimer.Update();
		wallJumpIncapacityTimer.Update();
		dashCooldown.Update();
		dashDuration.Update();
	}

	void FixedUpdate()
	{
		if (!dashDuration.done)
			return;

		Run();

		if (jump && coll.onWall && enabledWallJump)
			WallJump();

		if (jump)
		{
			if (coll.onGround)
				Jump();

			else if (!wallSlide && airJumpCurrent > 0)
				AirJump();
		}

		if (wallSlide && enabledWallSlide)
			WallSlide();

		if (special && dashCooldown.done && enabledDash)
			Dash();
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

	private void AirJump()
	{
		airJumpCurrent--;
		Jump();
		GameObject inst = Instantiate(airJumpPrefab, coll.position, Quaternion.identity);
		inst.transform.DOScale(Vector3.one * 2, 0.25f).OnComplete(() => Destroy(inst));
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

	private void Dash()
	{
		beforeDashVelocity = new Vector2(rb.velocity.x, 0);

		float dir = GetDir();

		if (dir == 0)
		{
			Debug.Log("Dash cancelled due to velocity equal 0");
			return;
		}

		rb.velocity = new Vector2(dashSpeed * dir, 0);

		dashCooldown.Start();
		dashDuration.Start();

		special = false;

		float GetDir()
		{
			if (xRaw != 0)
				return xRaw;
			else if (rb.velocity.x > 0)
				return 1;
			else if (rb.velocity.x < 0)
				return -1;
			else
				return 0;
		}
	}

	private void OnTouchGround()
	{
		airJumpCurrent = airJumpQty;
		postDashJumpCurrent = postDashJumpQty;
	}
}