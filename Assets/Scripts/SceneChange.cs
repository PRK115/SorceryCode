using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour {

    public void ToStageSelection()
    {
        SceneManager.LoadScene("StageSelection");
    }

    public void ToIntro()
    {
        SceneManager.LoadScene("Intro");
    }

    public void ToStage(int stageNumber)
    {
        SceneManager.LoadScene("Stage" + stageNumber);
    }

    public void Restart()
    {
        GameStateManager.instance.Restart();
    }
}
