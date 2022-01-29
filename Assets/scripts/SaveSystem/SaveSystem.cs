using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveSystem
{
	public static void SaveTime(float time, int level)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		TimeData timeData = new TimeData(time, level);

		string path = Application.persistentDataPath + "/highscores.save";
		FileStream stream = new FileStream(path, FileMode.Create);

		formatter.Serialize(stream, timeData);
		stream.Close();
	}
	public static TimeData LoadTime()
	{
		string path = Application.persistentDataPath + "/highscores.save";
		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			TimeData timeData = formatter.Deserialize(stream) as TimeData;
			stream.Close();

			return timeData;
		}
		else
		{
			Debug.LogError("Save file not found" + path);
			return null;
		}
	}
}
