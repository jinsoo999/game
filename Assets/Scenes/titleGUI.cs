using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class titleGUI : MonoBehaviour {
   
    public void OnClickStart()
    {
        SceneManager.LoadScene("Stage1");
    }

    public void OnClickHowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    public void OnClickRank()
    {
        SceneManager.LoadScene("Ranking");
    }
}
