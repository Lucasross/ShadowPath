using System;

public class Bar
{
	public float max { get; set; }
	public float current { get; private set; }
	public float normalized => current / max;

	public event Action OnReachMaximum;
	public event Action OnReachZero;
	public event Action<Bar> OnUpdate;

	public void Increase(float delta) => Mod(delta);
	public void Decrease(float delta) => Mod(-delta);
	public void Set(float value) => Mod(value - current);

	private void Mod(float delta)
	{
		current += delta;
		OnUpdate?.Invoke(this);

		if (current >= max)
		{
			current = max;
			OnReachMaximum?.Invoke();
		}
		else if (current <= 0)
		{
			current = 0;
			OnReachZero?.Invoke();
		}
	}
}