using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ThreadUtils : MonoBehaviour
{
	static Thread mainThread;

	protected void Awake()
	{
		mainThread = Thread.CurrentThread;
	}

	public static bool IsMainThread()
	{
		return mainThread.Equals(Thread.CurrentThread);
	}
}
