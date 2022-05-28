using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity2D, IDamageable
{
	public MonsterData data;
	public Animator anim;

	public UIBar uiHealthBar;

	public Entity2D Entity => this;
	public Bar healthBar = new Bar();

	protected override void Awake()
	{
		healthBar.max = data.health;
		healthBar.Set(healthBar.max);

		healthBar.OnReachZero += OnDie;

		uiHealthBar.Setup(healthBar);
	}

	private void OnDie()
	{
		Destroy(gameObject.transform.parent.gameObject);
	}

	public void Receive(float damage)
	{
		anim.SetTrigger("hitted");
		Instantiate(EntityManager.Instance.damagePopup).Setup(Mathf.RoundToInt(damage), damagePopupPosition);
		healthBar.Decrease(damage);
	}
}
