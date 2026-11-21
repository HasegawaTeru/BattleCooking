
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void GoToSelect()
    {
        SceneManager.LoadScene("SelectScene");
    }

    public void GoToCooking()
    {
        SceneManager.LoadScene("CookingScene");
    }

    public void GoToResult()
    {
        SceneManager.LoadScene("ResultScene");
    }

    public void GameEnd()
    {
        Application.Quit();
    }
}
