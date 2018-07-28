using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour {

    public void ToStageSelection()
    {
        SceneManager.LoadScene("StageSelection");
    }

    public void ToStage(int stageNumber)
    {
        if (stageNumber == 2)
            SceneManager.LoadScene("MapGenerator");
        else
            SceneManager.LoadScene("Stage" + stageNumber);
    }

    public void Restart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        Time.timeScale = 1;
    }

}
