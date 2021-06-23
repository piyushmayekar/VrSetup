using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsSpawner : MonoBehaviour
{
    [SerializeField] ToolRepository toolRepository;
    [SerializeField] List<ToolType> toolsToSpawn;
    float x_Start, z_Start;
    public int ColumnLength = 2, RowLength = 2;
    public float x_Space = 0.5f, z_Space = 0.5f;
    public Transform toolsParent;
    [SerializeField] GameObject labelPrefab;
    [SerializeField] float labelZGap = 0.01f;
    void Start()
    {
        if (toolsParent == null) toolsParent = transform;
        x_Start = toolsParent.transform.position.x;
        z_Start = toolsParent.transform.position.z;
        for (int i = 0; i < toolsToSpawn.Count; i++)
        {
            //Spawning Tool Prefab
            Vector3 position = new Vector3(x_Start + (x_Space * (i % ColumnLength)), toolsParent.position.y, z_Start + (z_Space * (i / ColumnLength)));
            GameObject prefab = GetTheToolPrefab(toolsToSpawn[i]);
            Instantiate(prefab, position, prefab.transform.rotation, toolsParent);

            //Spawning Label Prefab
            Vector3 labelPos = position + Vector3.back * labelZGap;
            GameObject label = Instantiate(labelPrefab, labelPos, labelPrefab.transform.rotation);
            label.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = prefab.name;
        }
    }

    GameObject GetTheToolPrefab(ToolType toolType)
    {
        int index = toolRepository.ToolTypes.FindIndex(tool => tool == toolType);
        if (index >= 0) //In case if the prefab is not found
            return toolRepository.Prefabs[index];
        return null;
    }
}

public enum ToolType
{
    Bottle, BrushWire, BrushFlat, CenterPunch,
    ChippingHammer, DotPunch, ElectroRod35mm, ElectroRod4mm, FlatFile,
    Hacksaw, Hammer, Scriber, SteelRule,
    Tong
}