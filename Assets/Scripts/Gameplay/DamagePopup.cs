using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class DamagePopup : MonoBehaviour
{
	public TextMeshPro label;

	public DamagePopup Setup(int damage, Vector3 position)
	{
		position.y += 0.5f;
		transform.position = position;

		label.text = damage.ToString();

		DOTween.Sequence()
			.AppendInterval(0.5f)
			.AppendCallback(() => { 
				label.DOFade(0, 1.5f); 
				transform.DOMoveY(transform.position.y + 1.3f, 1.5f).OnComplete(() => Destroy(gameObject)); 
			});

		return this;
	}
}
