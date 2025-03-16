using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

static class MenuConfig
{
    public static string taskToDo;
}

public class SwitchScene : MonoBehaviour
{
    public void GoToMainScene(string taskToDo)
      {
            MenuConfig.taskToDo = taskToDo;
            SceneManager.LoadScene("NewMain FINAL PRESENTATIE");
            ObjectSpawnManager.instance.SpawnAllObjects();
      }
}
