using UnityEngine;

public class AttackController2D : MonoBehaviour
{
	private MovementController2D movement;
	private AnimationController2D anim;

	private bool attack;
	private Timer attackTimer;

	public bool isAttacking;

	void Awake()
	{
		movement = GetComponent<MovementController2D>();
		anim = GetComponent<AnimationController2D>();

		attackTimer = TimerUtility.Create(0.15f).OnEnd(() => Cancel());
		MovementController2D.OnDash += Stop;
	}

	void Update()
	{
		if (Input.GetButtonDown("Attack"))
			attack = true;

		if (attack && CanAttack())
			Attack();

		attackTimer.Update();
	}

	bool CanAttack()
	{
		return !movement.wallSlide && movement.dashDuration.done;
	}

	void Attack()
	{
		anim.SetTrigger("attack");
		attackTimer.Start();
		attack = false;
	}

	void Cancel()
	{
		anim.ResetTrigger("attack");
		attack = false;
	}

	void Stop()
	{
		isAttacking = false;
	}

	// Used by an animation
	void Anim_OnStart()
	{
		isAttacking = true;
	}

	void Anim_OnEnd() => Stop();
}
