using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TimeData
{
	public float time;
	public int level;

	public TimeData(float time, int level)
	{
		this.time = time;
		this.level = level;
	}

}
