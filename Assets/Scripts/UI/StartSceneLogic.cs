using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneLogic : MonoBehaviour
{
    public void Standalone() {
        SceneManager.LoadScene("StandaloneScene");
    }
    public void Net()
    {
        SceneManager.LoadScene("LoginScene");
    }
}
