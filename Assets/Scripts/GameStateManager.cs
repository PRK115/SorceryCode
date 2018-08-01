using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

    public GameObject inStagePanel;
    public GameObject inStagePausePanel;
    public GameObject inStageFuneralPanel;
    public GameObject grimoire;
    public GameObject clearPanel;

    private void Start()
    {
        //inStagePanel = GameObject.Find("InStagePanel");
        //inStagePausePanel = GameObject.Find("PausePanel");
        //inStageFuneralPanel = GameObject.Find("FuneralPanel");
        inStagePanel.SetActive(true);
        inStagePausePanel.SetActive(false);
        inStageFuneralPanel.SetActive(false);
        grimoire.SetActive(false);
            clearPanel.SetActive(false);
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
        grimoire.SetActive(true);
    }

    public void CloseGrimoire()
    {
        inStagePanel.SetActive(true);
        grimoire.SetActive(false);
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
}
