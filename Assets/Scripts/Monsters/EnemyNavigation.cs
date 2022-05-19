using Pathfinding;
using UnityEngine;

public class EnemyNavigation : MonoBehaviour
{
	public LayerMask groundLayer;

	public Transform target;
	public float speed = 5f;
	public float nextWaypointDistance = 3f;
	public float reachedTargetDistance = 3f;
	public float jumpDistance = 1.5f;

	private Path path;
	private int currentWaypoint;
	private bool reachedEndOfWaypoint = false;
	private bool reachedTarget = false;

	private Seeker seeker;
	private Rigidbody2D rb;

	private Vector2 direction;
	private Vector2 toward => new Vector2(direction.x + jumpDistance * direction.x, 0);

	void Start()
	{
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();

		InvokeRepeating("UpdatePath", 0f, 2f);
	}

	void UpdatePath()
	{
		Debug.Log("UpdatePath");

		if(!reachedTarget)
			seeker.StartPath(rb.position, target.position, OnPathComplete);
	}

	void FixedUpdate()
	{
		if (path == null)
			return;

		UpdateReachedEndOfWaypoint();

		CheckReachedTarget();

		if (reachedTarget)
			return;

		GetDirection();

		Run();

		if (NeedToJump())
			Jump();

		UpdateWaypoint();
	}

	private void UpdateWaypoint()
	{
		float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

		if (distance < nextWaypointDistance && currentWaypoint + 1 < path.vectorPath.Count)
			currentWaypoint++;
	}

	private void UpdateReachedEndOfWaypoint()
	{
		if (currentWaypoint >= path.vectorPath.Count)
			reachedEndOfWaypoint = true;

		else
			reachedEndOfWaypoint = false;
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
		reachedTarget = true;
	}

	private void Run()
	{
		rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
	}

	private void GetDirection()
	{
		direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position);

		if (direction.x > 0.01f)
			direction.x = 1;

		else if (direction.x < 0.01f)
			direction.x = -1;
	}

	private void OnPathComplete(Path p)
	{
		if (p == null || p.error)
			return;

		path = p;
		currentWaypoint = 0;
	}

	private void ReachedEndOfPath()
	{
		reachedEndOfWaypoint = true;
	}

	private bool NeedToJump()
	{
		RaycastHit2D hit = Physics2D.Raycast(rb.position, toward, jumpDistance, groundLayer);

		return hit.collider != null;
	}

	private void Jump()
	{
		rb.velocity = new Vector2(rb.velocity.x, 5);
	}

	private void OnDrawGizmos()
	{
		if (direction == null || direction == Vector2.zero)
			return;

		Gizmos.DrawLine(rb.position, rb.position + direction);

		Gizmos.color = Color.red;

		//Gizmos.DrawWireSphere(path.vectorPath[currentWaypoint], 0.2f);

		Gizmos.color = Color.magenta * 0.8f;

		Gizmos.DrawLine(rb.position, rb.position + toward);
	}

	private void OnDrawGizmosSelected()
	{
		if (rb == null)
			rb = GetComponent<Rigidbody2D>();

		Gizmos.color = Color.red;

		Gizmos.DrawWireSphere(rb.position, nextWaypointDistance);

		Gizmos.color = Color.green;

		Gizmos.DrawWireSphere(rb.position, reachedTargetDistance);
	}
}
