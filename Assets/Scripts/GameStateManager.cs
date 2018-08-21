using System.Collections;
using System.Collections.Generic;
using CodeUI;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance = null;

    public GameObject inStagePanel;
    public GameObject inStagePausePanel;
    public GameObject inStageFuneralPanel;
    public GameObject grimoire;
    public GameObject clearPanel;

    public GameObject tutorialPanel;
    public Text tutorialTitleText;
    public Text tutorialContentText;

    public CodeUIElement codeUIElement;

    private StmtListBlock Program;

    private void Awake()
    {
        instance = this;
        Program = codeUIElement.Program;
    }

    private void Start()
    {
        inStagePanel.SetActive(true);
        inStagePausePanel.SetActive(false);
        inStageFuneralPanel.SetActive(false);
        clearPanel.SetActive(false);
        grimoire.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseGrimoire();
            Resume();
        }
    }

    public void ExecuteCode()
    {
        var code = Compiler.Compile(Program);
        Interpreter.Inst.Execute(code);
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
