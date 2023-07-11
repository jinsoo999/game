using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Damage : MonoBehaviour {
    private const string enemyTag = "ENEMY";
    private float initHp = 100.0f;
    public float currHp;
    public int killGo = 0;
    public int killHo = 0;
    public int killTr = 0;
    public int Score = 0;
    public int baScore = 0;
    public int GoScore = 0;
    public int HoScore = 0;
    public int TrScore = 0;
    public Image healthBar;

    public Text scoreText;

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;
    //EnemyAI enemyAI;
    Animator animator;

    void Start () {
        currHp = initHp;
    }

    void PlayerDie()
    {  //사용자의 사망 처리
        OnPlayerDie();
        SceneManager.LoadScene("GameOver");
        Debug.Log("PlayerDie !");
    }

    void Update()
    {
        Score = baScore + GoScore + HoScore + TrScore + Score_Manager.score;
        scoreText.text = "SCORE " + Score.ToString();
        if (currHp <= 0.0f)
        {
            Invoke("PlayerDie", 1);
        }
        else if ((GoScore >= 1000) && (HoScore == 0))
        {
            Score_Manager.score = Score;
            SceneManager.LoadScene("Stage2");
        }
        else if ((GoScore >= 1000) && (HoScore >= 1000))
        {
            Score_Manager.score = Score;
            SceneManager.LoadScene("Stage3");
        }
        else if((HoScore >= 1600) && (TrScore >= 1000))
        {
            Score_Manager.score = Score;
            SceneManager.LoadScene("GameClear");
        }
        OnChangeHealth();
    }

    void OnChangeHealth()
    {
        healthBar.fillAmount = currHp / initHp;
    }
}
