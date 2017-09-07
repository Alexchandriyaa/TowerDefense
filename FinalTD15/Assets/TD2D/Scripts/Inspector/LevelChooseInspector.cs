﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[ExecuteInEditMode]


/// <summary>
/// Level choose inspector.
/// </summary>
public class LevelChooseInspector : MonoBehaviour
{
	// Level description
	public Transform levelFolder;

	// Level chooser component
	private LevelChoose levelChooser;

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		levelChooser = GetComponent<LevelChoose>();
		Debug.Assert(levelFolder && levelChooser, "Wrong initial settings");
		// Delete all missing prefabs from list
		levelChooser.levelsPrefabs.RemoveAll(GameObject => GameObject == null);
	}

	/// <summary>
	/// Sets the active level.
	/// </summary>
	/// <param name="levelPrefab">Level prefab.</param>
	public void SetActiveLevel(GameObject levelPrefab)
	{
		if (levelPrefab != null)
		{
			LevelDescriptionInspector oldLevel = levelFolder.GetComponentInChildren<LevelDescriptionInspector>();
			// Destroy old level description
			if (oldLevel != null)
			{
				DestroyImmediate(oldLevel.gameObject);
			}
			// Create new level description
			GameObject level = Instantiate(levelPrefab, levelFolder);
			level.name = levelPrefab.name;
			level.transform.SetAsFirstSibling();
			levelChooser.currentLevel = level;
			// Add to allowed levels list
			if (levelChooser.levelsPrefabs.Contains(levelPrefab) == false)
			{
				levelChooser.levelsPrefabs.Add(levelPrefab);
			}
		}
	}
}
#endif

