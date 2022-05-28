using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity2D, IDamageable
{
	public Entity2D Entity => this;
	public Animator anim;

	public void Receive(float damage)
	{
		anim.SetTrigger("hitted");
		Instantiate(EntityManager.Instance.damagePopup).Setup(Mathf.RoundToInt(damage), damagePopupPosition);
	}
}
