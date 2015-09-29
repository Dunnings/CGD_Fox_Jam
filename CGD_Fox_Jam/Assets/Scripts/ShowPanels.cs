using UnityEngine;
using System.Collections;

public class ShowPanels : MonoBehaviour {

	public GameObject levelSelectPanel;
	public GameObject menuPanel;
	public GameObject optionsPanel;

	public void ShowMenuPanel ()
	{
		Time.timeScale = 0;
		menuPanel.SetActive (true);
	}
	public void ShowLevelSelectPanel ()
	{
		levelSelectPanel.SetActive(true);
	}

	public void HideLevelSelectPanel()
	{
		levelSelectPanel.SetActive(false);
	}

	public void HideMenuPanel()
	{
		Time.timeScale=1;
		menuPanel.SetActive(false);
	}

	public void ShowOptionsPanel ()
	{
		optionsPanel.SetActive (true);
	}

	public void HideOptionsPanel ()
	{
		optionsPanel.SetActive (false);
	}
}
