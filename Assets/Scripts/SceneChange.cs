using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour {

    public void ToStageSelection()
    {
        if (Interpreter.Inst != null)
            Interpreter.Inst.CancelAll();
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
        Interpreter.Inst.CancelAll();
        //Destroy(Interpreter.Inst.gameObject);
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        Time.timeScale = 1;
    }

}
