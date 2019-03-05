using System;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using CodeUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public GameObject inStagePanel;
    public GameObject inStagePausePanel;
    public GameObject inStageFuneralPanel;
    public GameObject grimoire;
    public Image grimoireImage;
    public GameObject clearPanel;

    public UnityEngine.UI.Button codeButton;
    public UnityEngine.UI.Button closeCodeButton;
    public UnityEngine.UI.Button aimButton;
    public UnityEngine.UI.Slider transparencySlider;
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

    private Task interpreterCancelTask = null;

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

                    interpreterCancelTask = Interpreter.Inst.CancelAll();
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
                    interpreterCancelTask = Interpreter.Inst.CancelAll();
                }
                else if (nextState == UIState.StageClear)
                {
                    grimoire.SetActive(false);
                    inStagePanel.SetActive(false);
                    inStagePausePanel.SetActive(false);
                    clearPanel.SetActive(true);

                    // When stage is clear, stop all running programs
                    interpreterCancelTask = Interpreter.Inst.CancelAll();
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
        instance = this;
        Program = codeUIElement.Program;
        codeButton.onClick.AddListener(() => SetState(UIState.Code));
        pauseButton.onClick.AddListener(() => SetState(UIState.Pause));
        resumeButton.onClick.AddListener(() => SetState(UIState.Game));
        closeCodeButton.onClick.AddListener(() => SetState(UIState.Game));
        restartButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            Restart();
        });
        exitButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            ToStageSelection();
        });

        player = FindObjectOfType<PlayerCtrl>();
        aimButton.onClick.AddListener(() => AimingButton.inst.CheckExprSlots());
        grimoireImage = grimoire.GetComponent<Image>();
        transparencySlider = FindObjectOfType<Slider>();
        if (transparencySlider != null)
            transparencySlider.onValueChanged.AddListener(ChangeCodeWindowTransparncy);
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

        if(Input.GetKeyDown(KeyCode.M))
        {
            SetState(UIState.Code);
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
        ToStageSelection();
    }

    public void ToStageSelection()
    {
        if (interpreterCancelTask != null)
        {
            interpreterCancelTask.Wait();
        }
        SceneManager.LoadScene("StageSelection");
    }

    public void ToIntro()
    {
        if (interpreterCancelTask != null)
        {
            interpreterCancelTask.Wait();
        }
        SceneManager.LoadScene("Intro");
    }

    public void ToStage(int stageNumber)
    {
        if (interpreterCancelTask != null)
        {
            interpreterCancelTask.Wait();
        }
        SceneManager.LoadScene("Stage" + stageNumber);
    }

    public void Restart()
    {
        if (interpreterCancelTask != null)
        {
            interpreterCancelTask.Wait();
        }
        //Destroy(Interpreter.Inst.gameObject);
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        Time.timeScale = 1;
    }

    public void ChangeCodeWindowTransparncy(float value)
    {
        grimoireImage.color = new Color(1,1,1,value);
    }
}
