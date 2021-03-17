using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    private void Start()
    {
        OnStartGame.IsTablet();
    }

    public void LoadGameScreen()
    {
        SceneManager.LoadScene("MainGameScene");
    }
}
