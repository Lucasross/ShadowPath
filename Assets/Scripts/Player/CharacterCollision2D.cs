using System;
using UnityEngine;

public class CharacterCollision2D : MonoBehaviour
{
	public static event Action OnTouchGround;

	public LayerMask groundLayer;

	[HideInInspector] public bool onGround;
	[HideInInspector] public bool onWall => onRightWall || onLeftWall;
	[HideInInspector] public bool onRightWall;
	[HideInInspector] public bool onLeftWall;

	[Header("Collision"), Range(0, 2)]
	public float collisionRadius = 0.25f;
	public Vector2 bottomOffset, rightOffset, leftOffset;

	public Vector2 position => (Vector2)transform.position;

	private bool wasOnGround;

	void FixedUpdate()
	{
		wasOnGround = onGround;

		onGround = Physics2D.OverlapCircle(position + bottomOffset, collisionRadius, groundLayer);
		onRightWall = Physics2D.OverlapCircle(position + rightOffset, collisionRadius, groundLayer);
		onLeftWall = Physics2D.OverlapCircle(position + leftOffset, collisionRadius, groundLayer);

		if (!wasOnGround && onGround)
			OnTouchGround?.Invoke();
	}

	void OnDrawGizmos()
	{
		Gizmos.color = onGround ? Color.green : Color.red;
		Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);

		Gizmos.color = onRightWall ? Color.green : Color.red;
		Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);

		Gizmos.color = onLeftWall ? Color.green : Color.red;
		Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
	}
}
