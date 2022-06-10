using UnityEngine.SceneManagement;

public class LevelSceneManager : IService
{
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public string GetLevel()
    {
        return SceneManager.GetActiveScene().name;
    }
}