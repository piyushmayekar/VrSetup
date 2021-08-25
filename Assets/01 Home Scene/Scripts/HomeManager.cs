using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Net.NetworkInformation;
using UnityEngine.Networking;

public class HomeManager : MonoBehaviour
{
    public static HomeManager instance;
    public static event Action<_Language> OnLanguageChange;

    [Header("ADD SCENE NAMES HERE")]
    [Tooltip("Add the scriptable object here of each experiment which will be loaded.\nDON'T FORGET TO ADD THOSE SCENES IN BUILD MENU")]
    public List<TextLangManager> textLangManagers = new List<TextLangManager>();

    public _Language currentLanguage = _Language.Gujrati;

    [Header("DON'T CHANGE THESE VARIABLES")]
    [SerializeField, Tooltip("The button that enables user to click on it & load an experiment scene")]
    GameObject expSelectorButtonPrefab;

    [SerializeField] Transform selectorPanel;

    List<ExpSelectorButton> expSelectorButtons = new List<ExpSelectorButton>();
    SceneLoadManager sceneLoadManager;

    [Header("Canvas objects")]
    public GameObject expSelectPanel, securityPanel, loadingText;
    public Text errorMsgText;
    public Text deviceidIF, guidIF;
    private string dateTimeAPI;
    private bool isNetworkReach;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        HideObjects();
    }
    public void ButtonInstantiate()
    {
        foreach (Transform item in selectorPanel)
        {
            Destroy(item.gameObject);
        }
        sceneLoadManager = GetComponent<SceneLoadManager>();
        currentLanguage = PiyushUtils.TaskManager.FetchCurrentLanguage();
        for (int i = 0; i < textLangManagers.Count; i++)
        {
            GameObject buttonGO = Instantiate(expSelectorButtonPrefab, selectorPanel);
            ExpSelectorButton expSelector = buttonGO.GetComponentInChildren<ExpSelectorButton>();
            expSelector.Initialize(textLangManagers[i], currentLanguage);
            expSelector.OnButtonClickEvent.AddListener(() => OnExpSelectorButtonClick(expSelector));
            expSelectorButtons.Add(expSelector);
        }
    }
    private void OnExpSelectorButtonClick(ExpSelectorButton expSelector)
    {
        sceneLoadManager.LoadSceneWithName(expSelector.sceneName);
    }

    public void OnLanguageChangeButtonClick()
    {
        int totalLanguagesCount = Enum.GetNames(typeof(_Language)).Length;
        int CurrentLangIndex = (int)currentLanguage;
        int nextLanguageIndex = (CurrentLangIndex + 1) % totalLanguagesCount;
        currentLanguage = (_Language)nextLanguageIndex;

        PiyushUtils.TaskManager.SaveCurrentLanguageToMemory(currentLanguage);

        OnLanguageChange?.Invoke(currentLanguage);
    }
    #region security manager
    void HideObjects()
    {
        DebugText(GetMacAddress());

        guidIF.text = "49716d55af374ccda4525ce1c9f99d0f";
        deviceidIF.text = "5405DBEA9181";

        expSelectPanel.SetActive(false);
        securityPanel.SetActive(true);
        loadingText.SetActive(true);
        OnChecksubscribe();
       // checkOffLine();
    }
    /// <summary>
	/// message for showing
	/// </summary>
	public void DebugText(string msg)
    {
        errorMsgText.text += "\n" + msg;
    }
    /// <summary>
	/// Get device unique id using mac address
	/// </summary>
	private static string GetMacAddress()
    {
        string physicalAddress = "";

        NetworkInterface[] nice = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface adaper in nice)
        {
            if (adaper.Description == "en0")
            {
                physicalAddress = adaper.GetPhysicalAddress().ToString();
                break;
            }
            else
            {
                physicalAddress = adaper.GetPhysicalAddress().ToString();

                if (physicalAddress != "")
                {
                    break;
                };
            }
        }

        return physicalAddress;
    }
    /// <summary>
	/// APi for get date time validate subscribe
	/// </summary>
	public IEnumerator POSTRequestData(string uri)
    {
        Debug.Log("Request : " + uri);
        UnityWebRequest uwr = new UnityWebRequest(uri, "POST");
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("macID", GetMacAddress());/// 
        //uwr.SetRequestHeader("macID", deviceidIF.text);
        Debug.Log("GetMacAddress  " + deviceidIF.text);
       
        uwr.SetRequestHeader("modelID", "49716d55af374ccda4525ce1c9f99d0f"); //Pass GUID
        Debug.Log("GUID   " + guidIF.text);

        uwr.SetRequestHeader("Content-Type", "application/json");
        yield return uwr.SendWebRequest();
        if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.Log("error: " + uwr.error);
            checkOffLine();
        }
        else
        {
            dateTimeAPI = uwr.downloadHandler.text;
            dateTimeAPI = dateTimeAPI.Trim('"');
              Debug.Log (" if access is denied ->Empty string" + dateTimeAPI);
            if (String.IsNullOrEmpty(dateTimeAPI))// if access is denied -> Empty string
            {
                DebugText("Your subscription expired" );
                yield return null;
               // checkOffLine();
            }
            else
            {
            SecurityManager.FirstTimeLogin = 1;
                //subscribe Valid : 8 / 22 / 2021 12:00:00 AM
                //20-08-2021 10:40:29
                DebugText("Valid till : " + dateTimeAPI);
                SecurityManager.APIDateTimeLogin = dateTimeAPI;
                //8/22/2021 12:00:00 AM
                Debug.Log("/// result for api  " + dateTimeAPI);
                SecurityManager.instance.CompareDateTime();//DateTime.Now);
            }
        }
    }
    /// <summary>
	/// Check subscriibe online/offline 
	/// online using api and offline we store api date
	/// </summary>
	public void OnChecksubscribe()
    {
        errorMsgText.text = "";
        isNetworkReach = false;
        if (Application.internetReachability == NetworkReachability.NotReachable) //Not reachable at all
        {
            DebugText("Not Connected to WiFi or Carrier network");
            isNetworkReach = false;
        }

        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) //Reachable via Carrier data network
        {
            Debug.Log("Connected to mobile carrier");
            isNetworkReach = true;
        }

        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) //Reachable via WiFi
        {
            Debug.Log("Connected to WiFi");
            isNetworkReach = true;
        }

        if (isNetworkReach)//online
        {
            StartCoroutine(POSTRequestData("https://ar-vr-authentication-api.azurewebsites.net/api/AuthorizationValidity"));
        }
        else //offline
        {
            checkOffLine();
        }
    }
    /// <summary>
    /// check stored date time
    /// </summary>
    public void checkOffLine()
    {
        Debug.Log("offline...check");
        if (String.IsNullOrEmpty(SecurityManager.APIDateTimeLogin))
        {
            DebugText("Please contact admin");
        }
        else
        {
            DebugText("Valid till : " + SecurityManager.APIDateTimeLogin);
            SecurityManager.instance.CompareDateTime();//DateTime.Now);
        }
    }

    public void CheckAccess()
    {
        if (SecurityManager.isAccess)
        {
            ButtonInstantiate();
            expSelectPanel.SetActive(true);
            securityPanel.SetActive(false);
        }
        else
        {
            expSelectPanel.SetActive(false);
            securityPanel.SetActive(true);
        }
    }
    #endregion
}
