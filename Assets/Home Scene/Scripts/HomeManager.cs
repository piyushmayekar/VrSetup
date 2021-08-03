using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeManager : MonoBehaviour
{
   
    public GameObject[] allExpGroupButtons;
	int countButton;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void onClickNextButton()
    {
		if (countButton + 1 < allExpGroupButtons.Length)
		{
			countButton++;
		}
		for (int i = 0; i < allExpGroupButtons.Length; i++)
		{
			allExpGroupButtons[i].SetActive(false);
		}
		allExpGroupButtons[countButton].SetActive(true);
	}
    public void OnClickPreviousButton()
    {
		if (countButton - 1 >= 0)
		{
			countButton--;
		}
		for (int i = 0; i < allExpGroupButtons.Length; i++)
		{
			allExpGroupButtons[i].SetActive(false);
		}
		allExpGroupButtons[countButton].SetActive(true);
	}
	/*
	 public void BAckButton()
  {
	  if(countButton - 1 >= 0)
	  {
		  countButton --;

	  }

	  for (int i = 0; i < allExpButtons.Length; i++) {
		  allExpButtons [i].SetActive (false);
	  }
	  allExpButtons [countButton].SetActive (true);

  }
  public void Nextbutton()
  {
	  if(countButton + 1 < allExpButtons.Length)
	  {
		  countButton++;

	  }
	  for (int i = 0; i < allExpButtons.Length; i++) {
		  allExpButtons [i].SetActive (false);
	  }
	  allExpButtons [countButton].SetActive (true);

  }
	 */
}
