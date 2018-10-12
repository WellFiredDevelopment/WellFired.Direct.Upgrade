using System.IO;
using UnityEditor;
using UnityEngine;

namespace WellFired.Direct.Upgrade.Editor
{
	public class Upgrade
	{
		private const string FileLocation = "Assets/existing.json";
		
		[MenuItem("Window/WellFired/.Direct/Write Existing Data")]
		public static void DumpCurrentData()
		{
			var data = TypeRunner.GetTypeData();
			var json = JsonUtility.ToJson(data);

			var canProceed = true;
			var alreadyExists = File.Exists(FileLocation);
			
			if(alreadyExists)
				canProceed = EditorUtility.DisplayDialog(
					"Please Confirm",
					"Seems like you already exported some data, if you continue this data will be overwritten", 
					"Ok, overwrite it!", 
					"Cancel, leave it as is.");

			if (!canProceed)
			{
				Debug.LogWarning("User aborted the process, they don't have a backup.");
				return;
			}
			
			if (alreadyExists)
				File.Delete(FileLocation);
			
			File.WriteAllText(FileLocation, json);
			AssetDatabase.Refresh();
		}

		[MenuItem("Window/WellFired/.Direct/Upgrade From Existing")]
		public static void UpgradeToNewData()
		{
			if (!File.Exists(FileLocation))
			{
				Debug.LogError("You must have previously Wrote Data to upgrade. Please run " +
				               "Window/WellFired/.Direct/Write Existing Data in your old project before trying to upgrade " +
				               "WellFired.Direct.");
				return;
			}

			var canProceed = EditorUtility.DisplayDialog(
				"Please Confirm",
				"This process might take a long time and will upgrade all of " +
				"the assets in your project, please make sure you have a backup " +
				"before continuing", 
				"Ok, I have a backup", 
				"Cancel, I didn't make a backup");

			if (!canProceed)
			{
				Debug.LogWarning("User aborted the process, they don't have a backup.");
				return;
			}
			
			var toData = TypeRunner.GetTypeData();
			var json = File.ReadAllText(FileLocation);
			var fromData = JsonUtility.FromJson<TypeData>(json);
			TypeRunner.Upgrade(fromData, toData);
			AssetDatabase.Refresh();
		}
	}
}