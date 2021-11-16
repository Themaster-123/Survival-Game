using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Settings : MonoBehaviour
{
	public static Settings Instance { get; protected set; }

	public uint ChunkLoadDistance;

	protected Mutex settingsMutex;

	public uint GetChunkLoadDistance()
	{
		settingsMutex.WaitOne();
		uint returnValue = ChunkLoadDistance;
		settingsMutex.ReleaseMutex();
		return returnValue;
	}

	protected void Awake() 
	{
		if (Instance == null)
		{
			Instance = this;
			settingsMutex = new Mutex();
			DontDestroyOnLoad(gameObject);
		} else
		{
			Destroy(gameObject);
		}
	}
}
