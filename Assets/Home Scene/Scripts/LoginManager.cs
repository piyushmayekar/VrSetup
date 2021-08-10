using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
	[SerializeField]
	public TextMeshProUGUI ids;

	private string macID, modelID;

    private void Awake()
    {
		//Get device id
		macID = SystemInfo.deviceUniqueIdentifier;

		print("Mac ID:" + macID);

		print("\n Last Logged In Details: " + GetLastLoggedInData());

		StartCoroutine(POSTRequestData("https://ar-vr-authentication-api.azurewebsites.net/api/AuthorizationValidity"));
    }
	/// <summary>
	///Details for getting validity date for model access using API
	/// <param name="URL">Pass the URL for model access</param>
	/// </summary>
	public IEnumerator POSTRequestData(string url/*, string json, System.Action<string, bool, string> callbackOnFinish*/)
	{
		UnityWebRequest uwr = new UnityWebRequest(url, "POST");

		Debug.Log("Request : " + url);

		uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

		uwr.SetRequestHeader("macID", macID);
		uwr.SetRequestHeader("modelID", "");
		uwr.SetRequestHeader("Content-Type", "application/json");

		print(uwr.uri);
		
		yield return uwr.SendWebRequest();

		if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.DataProcessingError)
		{
			print("error: " + uwr.error);
		}
		else
		{
            ids.text = uwr.downloadHandler.text;
            print("Response: " + uwr.downloadHandler.text);
			SetLastLoggedInData(DateTime.Now.ToString());
		}
	}

	private void SetLastLoggedInData(string data)
    {
		PlayerPrefs.SetString("LastLoggedIn", data);
	}

	private string GetLastLoggedInData()
    {
		if (PlayerPrefs.HasKey("LastLoggedIn"))
			return PlayerPrefs.GetString("LastLoggedIn");
		else
			return "";
    }
}