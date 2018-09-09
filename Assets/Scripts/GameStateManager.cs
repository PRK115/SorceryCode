using System;
using System.Collections;
using System.Collections.Generic;
using CodeUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    private SceneChange sceneChange;

    public GameObject inStagePanel;
    public GameObject inStagePausePanel;
    public GameObject inStageFuneralPanel;
    public GameObject grimoire;
    public GameObject clearPanel;

    public UnityEngine.UI.Button codeButton;
    public UnityEngine.UI.Button closeCodeButton;
    public UnityEngine.UI.Button aimButton;
    public UnityEngine.UI.Button pauseButton;
    public UnityEngine.UI.Button resumeButton;
    public UnityEngine.UI.Button restartButton;
    public UnityEngine.UI.Button exitButton;

    //public GameObject tutorialPanel;
    public Text tutorialTitleText;
    public Text tutorialContentText;

    public CodeUIElement codeUIElement;

    private StmtListBlock Program;

    PlayerCtrl player;

    public enum UIState
    {
        Game, Code, Pause, GameOver, StageClear
    }

    private UIState state = UIState.Game;
        public static GameStateManager instance = null;

    public void SetState(UIState nextState)
    {
        if (state == nextState) return;
        switch (state)
        {
            case UIState.Game:
                if (nextState == UIState.Code)
                {
                    inStagePanel.SetActive(false);
                    grimoire.SetActive(true);
                }
                else if (nextState == UIState.Pause)
                {
                    inStagePanel.SetActive(false);
                    inStagePausePanel.SetActive(true);
                    Time.timeScale = 0;
                }
                else if (nextState == UIState.GameOver)
                {
                    inStagePanel.SetActive(false);
                    inStagePausePanel.SetActive(false);
                    inStageFuneralPanel.SetActive(true);
                }
                else if (nextState == UIState.StageClear)
                {
                    inStagePanel.SetActive(false);
                    inStagePausePanel.SetActive(false);
                    clearPanel.SetActive(true);
                    StartCoroutine(ReturnAfterClear());
                }
                else throw new Exception($"Invalid UI State {state} -> {nextState}");
                break;
            case UIState.Code:
                if (nextState == UIState.Game)
                {
                    inStagePanel.SetActive(true);
                    grimoire.SetActive(false);
                }
                else if (nextState == UIState.GameOver)
                {
                    grimoire.SetActive(false);
                    inStagePanel.SetActive(false);
                    inStagePausePanel.SetActive(false);
                    inStageFuneralPanel.SetActive(true);

                    // When game over, stop all running programs
                    Interpreter.Inst.CancelAll();
                }
                else if (nextState == UIState.StageClear)
                {
                    grimoire.SetActive(false);
                    inStagePanel.SetActive(false);
                    inStagePausePanel.SetActive(false);
                    clearPanel.SetActive(true);

                    // When stage is clear, stop all running programs
                    Interpreter.Inst.CancelAll();
                }
                else throw new Exception($"Invalid UI State {state} -> {nextState}");
                break;
            case UIState.Pause:
                if (nextState == UIState.Game)
                {
                    inStagePausePanel.SetActive(false);
                    inStagePanel.SetActive(true);
                    Time.timeScale = 1;
                }
                else throw new Exception($"Invalid UI State {state} -> {nextState}");
                break;
            default:
                return;
        }
        state = nextState;
    }

    private void Awake()
    {
        sceneChange = FindObjectOfType<SceneChange>();

        instance = this;
        Program = codeUIElement.Program;
        codeButton.onClick.AddListener(() => SetState(UIState.Code));
        aimButton.onClick.AddListener(() => SetState(UIState.Game));
        pauseButton.onClick.AddListener(() => SetState(UIState.Pause));
        resumeButton.onClick.AddListener(() => SetState(UIState.Game));
        closeCodeButton.onClick.AddListener(() => SetState(UIState.Game));
        restartButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            sceneChange.Restart();
        });
        exitButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            sceneChange.ToStageSelection();
        });

        player = FindObjectOfType<PlayerCtrl>();
        aimButton.onClick.AddListener(() => player.SetState(PlayerCtrl.State.Aiming));
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
            SetState(UIState.Game);
        }
    }

    public void OpenTutorialPanel(GameObject tutorialPanel)
    {
        tutorialPanel.SetActive(true);
        //tutorialTitleText.text = name;
        //tutorialContentText.text = content;
    }

    public void CloseTutorialPanel(GameObject tutorialPanel)
    {
        tutorialPanel.SetActive(false);
    }

    IEnumerator ReturnAfterClear()
    {
        yield return new WaitForSeconds(1.2f);
        sceneChange.ToStageSelection();
    }
}
