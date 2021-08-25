using System;
using System.Globalization;

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Check .exe security
/// </summary>
public class SecurityManager : MonoBehaviour
{
	public static SecurityManager instance;
	public static bool isAccess;
	public  int remainingTime=5;//Change remaining time 
		//	Scene currentScene;

	public void Awake()
	{
		instance = this;
		DontDestroyOnLoad(this);
	}


	/// <summary>
	/// check first time exe start then we get api date time online
	/// </summary>
	public static int FirstTimeLogin
	{
		get
		{
			return PlayerPrefs.GetInt("FirstTimeLogin", 0);
		}
		set
		{
			PlayerPrefs.SetInt("FirstTimeLogin", value);
		}
	}
	/// <summary>
	/// Store api date time 
	/// </summary>
	public static string APIDateTimeLogin
	{
		get
		{
			return PlayerPrefs.GetString("APIDateTimeLogin");
		}
		set
		{
			PlayerPrefs.SetString("APIDateTimeLogin", value);
		}
	}

	TimeSpan finalTiming;
	DateTime dateAPI, dateTimeNow;



	/// <summary>
	/// check two date and time first is api date time and second system datetime
	/// </summary>
	public void CompareDateTime()
	{

		dateTimeNow = DateTime.Now;
		CultureInfo provider = CultureInfo.InvariantCulture;
		dateAPI = Convert.ToDateTime(APIDateTimeLogin, provider);  //convert date using - or /
		Debug.Log("dateTime of API  " + dateAPI);
		Debug.Log("system date  " + dateTimeNow);

		int result = DateTime.Compare(dateAPI, dateTimeNow);
		Debug.Log("Result find  " + result);
		finalTiming = dateAPI - dateTimeNow;



		if (result < 0)  // not access
		{
			isAccess = false;
		//	HomeManager.instance.DebugText("validity date for model not access");
		}
		else if (result == 0) //Accses
		{
			isAccess = true;
			Debug.Log("validity date for model access is the same time as");

		}
		else // access
		{
			isAccess = true;
			Debug.Log("validity date for model access is later than (You have more time)");
		}
		HomeManager.instance.loadingText.SetActive(false);
		Debug.Log("finalTiming  " + finalTiming);
		if (finalTiming.Hours <= 1)
		{
			if (finalTiming.Minutes <= 30)
			{
				if (finalTiming.Minutes <= remainingTime && finalTiming.Minutes >= 1)//Change remaining time 
				{
					isAccess = false;
				//	HomeManager.instance.DebugText("Subscribe Valid : 5 Minutes");
				}
				else
				{
					InvokeRepeating("CheckIf30_miniutesRemaining", 20, 60);

				}
			}
		}
		HomeManager.instance.CheckAccess();
	}
	public void CheckIf30_miniutesRemaining()
	{
		Debug.Log("call 30 minites time");
		dateTimeNow = DateTime.Now;
		CultureInfo provider = CultureInfo.InvariantCulture;
		dateAPI = Convert.ToDateTime(APIDateTimeLogin, provider);  //convert date using - or /
																   //Debug.Log("dateTime of API  " + dateAPI);
																   //	Debug.Log("system date  " + dateTimeNow);

		finalTiming = dateAPI - dateTimeNow;
		Debug.Log("finalTiming  ****  " + finalTiming);
		if (finalTiming.Hours <= 1)
		{
			if (finalTiming.Minutes <= 30)
			{
				if (finalTiming.Minutes <= remainingTime && finalTiming.Minutes >= 1)
				{
					Debug.Log("Minites : " + finalTiming.Minutes);
					//	 currentScene = SceneManager.GetActiveScene().name;

					// Retrieve the name of this scene.
					string sceneName = SceneManager.GetActiveScene().name;

					if (sceneName == "Home Scene")
					{
						Debug.Log("Call home scene");
						// Do something...
						//HomeManager.instance.DebugText("Subscribe Valid : 5 Minutes");
						HomeManager.instance.CheckAccess();
					}
					else
					{
						Debug.Log("Call other scene");
						// Do something...
						SceneManager.LoadScene("Home Scene");
					}
					isAccess = false;
					CancelInvoke("CheckIf30_miniutesRemaining");
				}
			}
		}
	}

}




/*	Debug.Log("///before current date time  " + dateTimeNow);
	dateTimeNow.ToString("MM/dd/yyyy HH:mm:ss t");
	Debug.Log("///after current date time  " + dateTimeNow);
//	DateTime dateTime10 = DateTime.ParseExact(dateTime, @"MM/dd/yyyy HH:mm:ss TT", provider);

		System.DateTime dateTime2 = System.DateTime.Parse(dateTime);
		Debug.Log("dajdajdh   " + dateTime2);*/
//resultAPI = Convert.ToDateTime(dateTime);//"22-08-2021 12:00:00 AM");