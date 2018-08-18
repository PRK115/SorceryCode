using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour {

    public GameObject inStagePanel;
    public GameObject inStagePausePanel;
    public GameObject inStageFuneralPanel;
    public GameObject grimoire;
    public GameObject clearPanel;

    RectTransform grimoireTransform;

    public GameObject tutorialPanel;
    public Text tutorialTitleText;
    public Text tutorialContentText;

    private void Start()
    {
        //inStagePanel = GameObject.Find("InStagePanel");
        //inStagePausePanel = GameObject.Find("PausePanel");
        //inStageFuneralPanel = GameObject.Find("FuneralPanel");
        inStagePanel.SetActive(true);
        inStagePausePanel.SetActive(false);
        inStageFuneralPanel.SetActive(false);
        clearPanel.SetActive(false);
        grimoireTransform = grimoire.GetComponent<RectTransform>();
        grimoireTransform.position = new Vector3(-2000, 0, 0);
    }

    //일시 정지 메뉴
    public void Pause()
    {
        inStagePanel.SetActive(false);
        inStagePausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        inStagePausePanel.SetActive(false);
        inStagePanel.SetActive(true);
        Time.timeScale = 1;
    }

    public void OpenGrimoire()
    {
        inStagePanel.SetActive(false);
        grimoireTransform.localPosition = Vector3.zero;
    }

    public void CloseGrimoire()
    {
        inStagePanel.SetActive(true);
        grimoireTransform.position = new Vector3(-2000, 0, 0);
    }

    public void Funeral()
    {
        inStagePanel.SetActive(false);
        inStagePausePanel.SetActive(false);
        inStageFuneralPanel.SetActive(true);
    }

    public void StageClear()
    {
        inStagePanel.SetActive(false);
        inStagePausePanel.SetActive(false);
        clearPanel.SetActive(true);
    }

    public void OpenTutorialPanel(string name, string content)
    {
        tutorialPanel.SetActive(true);
        tutorialTitleText.text = name;
        tutorialContentText.text = content;
    }

    public void CloseTutorialPanel()
    {
        tutorialPanel.SetActive(false);
    }
}
