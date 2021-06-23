using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tools Repository", menuName = "Scriptable Objects/Tool Repository")]
public class ToolRepository : ScriptableObject
{
    [SerializeField] List<ToolType> toolTypes;
    [SerializeField] List<GameObject> prefabs;

    public List<ToolType> ToolTypes { get => toolTypes; }
    public List<GameObject> Prefabs { get => prefabs; }

    [ContextMenu("Fill all tool types")]
    public void FillToolTypes()
    {
        ToolTypes.Clear();
        int toolCount = Enum.GetValues(typeof(ToolType)).Length;
        for (int i = 0; i < toolCount; i++)
            ToolTypes.Add((ToolType)i);
    }
}