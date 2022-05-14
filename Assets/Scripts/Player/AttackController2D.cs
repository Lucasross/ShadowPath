using UnityEngine;

public class AttackController2D : MonoBehaviour
{
	private MovementController2D movement;
	private AnimationController2D anim;

	void Awake()
	{
		movement = GetComponent<MovementController2D>();
		anim = GetComponent<AnimationController2D>();
	}

	void Update()
	{
		bool attack = Input.GetButton("Attack");

		if (attack && CanAttack())
			Attack();
	}

	void Attack()
	{
		anim.SetTrigger("Attack");
	}

	bool CanAttack()
	{
		return !movement.wallSlide;
	}
}
