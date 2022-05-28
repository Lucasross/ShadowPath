using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIBar : MonoBehaviour
{
	private Image uiBar;

	void Awake()
	{
		uiBar = GetComponent<Image>();
	}

	public void Setup(Bar bar)
	{
		bar.OnUpdate += OnBarUpdate;	
	}

	private void OnBarUpdate(Bar bar)
	{
		uiBar.fillAmount = bar.normalized;
	}
}
