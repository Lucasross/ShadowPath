using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
	Entity2D Entity { get; }

	void Receive(float damage);
}
