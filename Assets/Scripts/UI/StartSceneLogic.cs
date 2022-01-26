using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneLogic : MonoBehaviour
{
    public GameObject PlayBackPanel;
    public GameObject content;
    bool playbackPanelIsInit = false;
    private string recordContent;
    private void Awake()
    {
        recordContent = Path.Combine(Application.streamingAssetsPath, "GameRecord");
    }
    public void Standalone() {
        GlobalConfig.Instance.gameType = GameType.Standalone;
        GlobalConfig.Instance.recording = true;
        SceneManager.LoadScene("StandaloneScene");
    }
    public void Net()
    {
        GlobalConfig.Instance.gameType = GameType.Net;
        GlobalConfig.Instance.recording = false;
        SceneManager.LoadScene("LoginScene");
    }
    public void PlayBack() {
        PlayBackPanel.SetActive(true);
        if (!playbackPanelIsInit) {
            playbackPanelIsInit = true;
            GameObject gameObject = Resources.Load<GameObject>("UI/RecordBtn");
            string [] dics = Directory.GetFiles(recordContent,"*.rcd");
            foreach (string dd in dics) {
                GameObject btn = Instantiate(gameObject,content.transform);
                btn.GetComponent<Button>().onClick.AddListener(()=> {
                    GlobalConfig.Instance.gameType = GameType.Playback;
                    GlobalConfig.Instance.recording = false;
                    GlobalConfig.Instance.currentRecordName = dd.Substring(dd.LastIndexOf(@"\") + 1);
                    SceneManager.LoadScene("StandaloneScene");
                });
                btn.transform.Find("Text").GetComponent<Text>().text = dd.Substring(dd.LastIndexOf(@"\")+1);
            }
        }
    }

    public void ExitPlayBackPanel()
    {
        PlayBackPanel.SetActive(false);
    }
}
