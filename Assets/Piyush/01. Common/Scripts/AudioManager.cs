using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    #region SINGLETON
    static AudioManager instance = null;

    public static AudioManager Instance { get => instance; set => instance = value; }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (instance == null)
            instance = this;
        if (instance != this)
            Destroy(this);
    }
    #endregion
    [SerializeField] AudioMixer audioMixer;
    void Start()
    {
    }
}