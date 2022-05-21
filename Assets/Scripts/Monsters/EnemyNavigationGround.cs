using Pathfinding;
using System;
using UnityEngine;

public class EnemyNavigationGround : MonoBehaviour
{
	public LayerMask groundLayer;

	public bool isHunting;

	public Transform target;
	public float speed = 5f;
	public float jumpForce = 10f;
	public float reachedTargetDistance = 3f;
	public bool reachedTarget = false;
	public float jumpDistance = 1.5f;

	public bool IsRunning => Mathf.Abs(rb.velocity.x) > 0 || (!reachedTarget && !targetOnYAxis);

	private Rigidbody2D rb;
	private EnemyCollision coll;

	private Vector2 direction;
	private Vector2 toward => new Vector2(direction.x + jumpDistance * direction.x, 0);

	private bool targetOnYAxis;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		coll = GetComponent<EnemyCollision>();

		isHunting = true;
	}

	void FixedUpdate()
	{
		CheckReachedTarget();

		if (reachedTarget)
			return;

		CheckYAxis();

		if (targetOnYAxis)
			return;

		GetDirection();

		Run();

		if (NeedToJump() && CanJump())
			Jump();

	}

	private void CheckYAxis()
	{
		if (BarelyEqual(rb.position.x, target.position.x))
		{
			rb.velocity = new Vector2(0, rb.velocity.y);
			targetOnYAxis = true;
			return;
		}

		targetOnYAxis = false;
	}

	private void CheckReachedTarget()
	{
		Collider2D[] allColliders = Physics2D.OverlapCircleAll(rb.position, reachedTargetDistance);

		foreach (Collider2D collider in allColliders)
		{
			if (collider.transform == target)
			{
				ReachedTarget();
				return;
			}
		}

		reachedTarget = false;
	}

	private void ReachedTarget()
	{
		if (!reachedTarget)
			rb.velocity = Vector2.up * rb.velocity;

		reachedTarget = true;
	}

	private void Run()
	{
		rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
	}

	private void GetDirection()
	{
		direction = (Vector2)target.position - rb.position;

		if (direction.x > 0.01f)
			direction.x = 1;

		else if (direction.x < 0.01f)
			direction.x = -1;
	}

	private bool NeedToJump()
	{
		RaycastHit2D hit = Physics2D.Raycast(rb.position, toward, jumpDistance, groundLayer);

		return hit.collider != null;
	}

	private bool CanJump()
	{
		return coll.onGround;
	}

	private void Jump()
	{
		rb.velocity = new Vector2(rb.velocity.x, jumpForce);
	}

	private void OnDrawGizmos()
	{
		if (direction == null || direction == Vector2.zero)
			return;

		Gizmos.DrawLine(rb.position, rb.position + direction);

		Gizmos.color = Color.magenta * 0.8f;

		Gizmos.DrawLine(rb.position, rb.position + toward);
	}

	private void OnDrawGizmosSelected()
	{
		if (rb == null)
			rb = GetComponent<Rigidbody2D>();

		Gizmos.color = Color.green;

		Gizmos.DrawWireSphere(rb.position, reachedTargetDistance);
	}

	public static bool BarelyEqual(float a, float b, float threshold = 0.5f)
	{
		a = Mathf.Abs(a);
		b = Mathf.Abs(b);
		float diff = Mathf.Abs(a - b);
		return diff < threshold;
	}
}
