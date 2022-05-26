using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Character : Entity2D, IDamageable
{
	public Entity2D Entity => this;

	[HideInInspector] public List<AttackData> attacksData;

	protected override void Awake()
	{
		attacksData.ForEach(a => a.hitbox.owner = this);
	}

	public void OnAnimationAttack(int attack)
	{
		List<IDamageable> hitted = Hitbox2DUtility.Get<IDamageable>(attacksData[attack-1].hitbox).ToList();
		hitted.Remove(this);

		hitted.ForEach(h => h.Receive(UnityEngine.Random.Range(5, 20)));
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

	public void Receive(float damage)
	{
		throw new NotImplementedException();
	}
}
