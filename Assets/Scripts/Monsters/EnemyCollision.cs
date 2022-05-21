using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
	public LayerMask groundLayer;

	[HideInInspector] public bool onGround;

	[Header("Collision"), Range(0, 2)]
	public float collisionRadius = 0.25f;
	public Vector2 bottomOffset;

	public Vector2 position => (Vector2)transform.position;

	void FixedUpdate()
	{
		onGround = Physics2D.OverlapCircle(position + bottomOffset, collisionRadius, groundLayer);
	}

	void OnDrawGizmos()
	{
		Gizmos.color = onGround ? Color.green : Color.red;
		Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
	}
}
