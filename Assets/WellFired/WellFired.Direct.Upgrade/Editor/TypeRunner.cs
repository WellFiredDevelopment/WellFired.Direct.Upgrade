using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace WellFired.Direct.Upgrade.Editor
{
	public static class TypeRunner 
	{
		public static TypeData GetTypeData()
		{
			var typeData = new TypeData();

			var assets = new List<string>();
			
			// Concat all potential assets
			assets.AddRange(AssetDatabase.FindAssets("uSequencerRuntime"));
			assets.AddRange(AssetDatabase.FindAssets("uSequencerEditor"));
			assets.AddRange(AssetDatabase.FindAssets("sharedRuntime"));
			assets.AddRange(AssetDatabase.FindAssets("sharedEditor"));
			assets.AddRange(AssetDatabase.FindAssets("WellFired.Direct.Runtime"));
			assets.AddRange(AssetDatabase.FindAssets("WellFired.Direct.Editor"));
			assets.AddRange(AssetDatabase.FindAssets("WellFired.Shared.Runtime"));
			assets.AddRange(AssetDatabase.FindAssets("WellFired.Shared.Editor"));

			foreach (var asset in assets)
			{
				var assetPath = AssetDatabase.GUIDToAssetPath(asset);
				var loadedAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
				foreach (var unityObject in loadedAssets)
				{
					string guid;
					long fileId;

					if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(unityObject, out guid, out fileId))
						typeData.AddEntry(TypeDataEntry.Create(unityObject.name, guid, fileId));
				}
			}

			return typeData;
		}

		public static void Upgrade(TypeData fromData, TypeData toData)
		{
			try
			{
				AssetDatabase.StartAssetEditing();
				EditorUtility.DisplayProgressBar("Asset Conversion Progress", "Conversion in progress, please wait.", 0.0f);
				PerformUpgrade(fromData, toData);
			}
			catch (Exception e)
			{
				Debug.LogError("An exception occured when processing the upgrade. upgrade aborted.");
				Debug.LogError(e);
			}
			finally
			{
				EditorUtility.ClearProgressBar();
				AssetDatabase.StopAssetEditing();
			}
		}

		private static void PerformUpgrade(TypeData fromData, TypeData toData)
		{
			var assets = new List<string>();

			assets.AddRange(AssetDatabase.FindAssets("t:Scene"));
			assets.AddRange(AssetDatabase.FindAssets("t:prefab"));
			assets.AddRange(AssetDatabase.FindAssets("t:ScriptableObject"));

			for (var index = 0; index < assets.Count; index++)
			{
				EditorUtility.DisplayProgressBar("Asset Conversion Progress", "Conversion in progress, please wait.", index / (float)assets.Count);
				
				var asset = assets[index];
				var assetPath = AssetDatabase.GUIDToAssetPath(asset);
				var text = File.ReadAllText(assetPath);
				var modified = false;
				
				foreach (var fromEntry in fromData.Data)
				{
					var toEntry = toData.Data.FirstOrDefault(o => o.Type == fromEntry.Type);

					if (fromEntry == null || toEntry == null)
						continue;

					var fromString = BuildString(fromEntry);
					var toString = BuildString(toEntry);

					if (text.Contains(fromString))
					{
						modified = true;
						text = text.Replace(fromString, toString);
					}

					if (text.Contains(fromEntry.GUID))
					{
						modified = true;
						text = text.Replace(fromEntry.GUID, toEntry.GUID);
					}
				}
				
				if(modified)
					File.WriteAllText(assetPath, text);
			}
		}

		private static string BuildString(TypeDataEntry data)
		{
			// m_Script: {fileID: 11500000, guid: 3dd072ebad4384a4c96e1049872b47a4, type: 1}
			return string.Format("m_Script: {{fileID: {0}, guid: {1}", data.FileId, data.GUID);
		}
	}
}