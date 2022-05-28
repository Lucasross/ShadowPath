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
		transform.position = position;
		transform.localScale = Vector3.one * 1.5f;

		label.text = damage.ToString();

		DOTween.Sequence()
			.Append(transform.DOScale(Vector3.one, 0.2f))
			.AppendCallback(() => { 
				label.DOFade(0, 1.5f); 
				transform.DOMoveY(transform.position.y + 1.3f, 1.5f).OnComplete(() => Destroy(gameObject)); 
			});

		return this;
	}
}
