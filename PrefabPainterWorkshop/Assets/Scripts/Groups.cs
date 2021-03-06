﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Groups
{
    private List<GameObject> groups = new List<GameObject>();
	private Painter prefabPainter;
    private GameObject previousEditGroup;
    private bool reloadPainterSettings = true;
    private PrefabPainterLibrary prefabPainterLibrary;
    
    public Groups(Painter prefabPainter, PrefabPainterLibrary prefabPainterLibrary)

    {
        this.prefabPainter = prefabPainter;
        this.prefabPainterLibrary = prefabPainterLibrary;
        Group[] existingGroups = Object.FindObjectsOfType<Group>();
        foreach (Group group in existingGroups)
        {
            groups.Add(group.gameObject);
        }
    }

    public void renderGUI()
    {
        GUILayout.Label("Groups", EditorStyles.boldLabel);
        if (GUILayout.Button("Add Group"))
        {
            addGroup();
        }
        renderGroupsGUI();
    }

    private void renderGroupsGUI()
    {
        int groupIndex = 0;

        for (int i = groups.Count - 1; i >= 0; i--)
        {
            EditorGUILayout.BeginHorizontal();
            string myLabel = "" + (groupIndex + 1) + ": ";
            if (GUILayout.Button("Edit"))
            {
                editGroup(groups[i]);
            }
            if (GUILayout.Button("Delete"))
            {
                deleteGroup(groups[i]);
            }
            else
            {
                if (groups[i].GetComponent<Group>().isEditing())
                {
                    myLabel += " (EDIT)";
                    /*if (previousEditGroup == null)
                    {
                        previousEditGroup = groups[i];
                    }*/
                    if (reloadPainterSettings)
                    {
                        loadPrefabPainterSettings(groups[i]);
                        reloadPainterSettings = false;
                    }
                    savePrefabPainterSettings(groups[i]);
                }
                groups[i].name = EditorGUILayout.TextField(myLabel, text: groups[i].name);
            }
            groupIndex++;
            EditorGUILayout.EndHorizontal();
        }
    }

    private void toggleIsEditing(GameObject editGroup)
    {
        foreach (GameObject group in groups)
        {
            group.GetComponent<Group>().isEditing(false);
        }
        editGroup.GetComponent<Group>().isEditing(true);
    }

    private void addGroup()
    {
        GameObject group = new GameObject("Test group");
        group.AddComponent<Group>();
        groups.Add(group);
    }

    private void editGroup(GameObject group)
    {
        reloadPainterSettings = true;
        // savePrefabPainterSettings(previousEditGroup);
        toggleIsEditing(group);
        prefabPainter.setGroup(group);
        previousEditGroup = group;
    }

    private void savePrefabPainterSettings(GameObject group)
    {
        group.GetComponent<Group>().paintLayer = prefabPainter.paintLayer;
        group.GetComponent<Group>().paintMode = prefabPainter.paintMode;
        group.GetComponent<Group>().eraseRadius = prefabPainter.eraseRadius;
        group.GetComponent<Group>().placementRadius = prefabPainter.placementRadius;
        group.GetComponent<Group>().randomScaleMin = prefabPainter.randomScaleMin;
        group.GetComponent<Group>().randomScaleMax = prefabPainter.randomScaleMax;
        group.GetComponent<Group>().randomRotationMin = prefabPainter.randomRotationMin;
        group.GetComponent<Group>().randomRotationMax = prefabPainter.randomRotationMax;
        group.GetComponent<Group>().libraryItemList = prefabPainterLibrary.getLibraryItemList();
    }

    private void loadPrefabPainterSettings(GameObject group)
    {
        prefabPainter.paintLayer = group.GetComponent<Group>().paintLayer;
        prefabPainter.paintMode = group.GetComponent<Group>().paintMode;
        prefabPainter.eraseRadius = group.GetComponent<Group>().eraseRadius;
        prefabPainter.placementRadius = group.GetComponent<Group>().placementRadius;
        prefabPainter.randomScaleMin = group.GetComponent<Group>().randomScaleMin;
        prefabPainter.randomScaleMax = group.GetComponent<Group>().randomScaleMax;
        prefabPainter.randomRotationMin = group.GetComponent<Group>().randomRotationMin;
        prefabPainter.randomRotationMax = group.GetComponent<Group>().randomRotationMax;
        prefabPainterLibrary.setLibraryItemList(group.GetComponent<Group>().libraryItemList);
    }

    private void deleteGroup(GameObject group)
    {
        groups.Remove(group);
        UnityEngine.Object.DestroyImmediate(group);
    }
}
