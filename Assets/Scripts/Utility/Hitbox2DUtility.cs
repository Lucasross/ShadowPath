using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Hitbox2DUtility
{
	public static IEnumerable<T> Get<T>(Hitbox2D hitbox, float angle = 0)
	{
		return Physics2D.OverlapBoxAll(hitbox.position, hitbox.size, angle)
			.Where(c => c.gameObject.GetComponent<T>() != null)
			.Select(c => c.gameObject.GetComponent<T>());
	}
}

[Serializable]
public class Hitbox2D
{
	public Vector2 offset;
	public Vector2 size;
	public Vector2 position => (owner == null ? Vector2.zero : (Vector2)owner.transform.position) + sidedOffset;

	public Entity2D owner { private get; set; }
	private Vector2 sidedOffset => new Vector2(offset.x * (owner == null ? 1 : owner.side), offset.y);

	public Hitbox2D(Entity2D owner, Vector2 offset, Vector2 size)
	{
		this.owner = owner;
		this.offset = offset;
		this.size = size;
	}
}
