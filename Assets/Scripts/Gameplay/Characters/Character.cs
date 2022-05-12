using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : Entity2D, IDamageable
{
	public Entity2D Entity => this;

	[HideInInspector] public List<AttackData> attacksData;

	public void OnAnimationAttack(int attack)
	{

	}

	[Serializable]
	public class AttackData
	{
		public Hitbox2D hitbox;
		public float damageMultiplier;

		public bool showGizmosHitbox;

		public AttackData(Hitbox2D hitbox)
		{
			this.hitbox = hitbox;
		}
	}

	void OnDrawGizmosSelected()
	{
		foreach(AttackData data in attacksData)
		{
			if(data.showGizmosHitbox)
			{
				Gizmos.color = Color.magenta * 0.7f;
				Gizmos.DrawWireCube(data.hitbox.position, data.hitbox.size);
			}
		}	
	}
}
