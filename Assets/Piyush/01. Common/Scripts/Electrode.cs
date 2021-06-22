using UnityEngine;

public class Electrode : MonoBehaviour
{
    [SerializeField] ElectrodeType type;
    GameObject highlight;
    public ElectrodeType ElectrodeType { get => type; set => type = value; }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        highlight = transform.GetChild(0).gameObject;
    }

    public void OnSelectEnter()
    {
        highlight.SetActive(false);
    }
}