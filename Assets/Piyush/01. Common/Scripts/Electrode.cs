using UnityEngine;

public class Electrode : MonoBehaviour
{
    [SerializeField] ElectrodeType type;
    public ElectrodeType ElectrodeType { get => type; set => type = value; }
}