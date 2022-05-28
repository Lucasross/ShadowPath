using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity2D : MonoBehaviour
{
	[HideInInspector] public int side = 1;

	public Vector2 position => (Vector2)transform.position;

	[SerializeField] protected Vector2 damagePopupOffset;
	protected Vector2 damagePopupPosition => position + damagePopupOffset;

	protected virtual void Awake() { }

	protected virtual void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(damagePopupPosition, 0.25f);
	}
}
