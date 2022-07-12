using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationFly : MonoBehaviour
{
    public AIPath aiPath;
    public SpriteRenderer sprite;
	public Entity2D entity;

	private void Awake()
	{
		entity = GetComponent<Entity2D>();
	}

	public void Update()
	{
		if (aiPath.desiredVelocity.x > 0.1f)
			FlipRight();

		else if (aiPath.desiredVelocity.x < -0.1f)
			FlipLeft();
	}

	private void FlipLeft() => Flip(true);
	private void FlipRight() => Flip(false);

	private void Flip(bool flipX)
	{
		entity.side = flipX ? -1 : 1;
		sprite.flipX = flipX;
	}
}
