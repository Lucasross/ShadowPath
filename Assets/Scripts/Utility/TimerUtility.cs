using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimerUtility
{
    public static Timer Create(float duration)
	{
		Timer t = new Timer() { duration = duration };
		return t;
	}
}

public class Timer
{
	public event Action OnTimerEnd;

	public bool done => progress <= 0;

	public float duration;
	public float progress;

	public void Start()
	{
		progress = duration;
	}

	public void Update()
	{
		if (progress <= 0)
			return;

		progress -= Time.deltaTime;

		if (progress <= 0)
			OnTimerEnd?.Invoke();
	}
}
