using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public GameObject gameLabel;
    Text gameText;
    PlayerMove player;

    public GameObject hitEffect;

    private void Awake()
    {
        if(gm == null)
        {
            gm = this;
        }
    }

    public enum GameState
    {
        Ready,
        Run,
        Pause,
        GameOver
    }

    public GameState gState;

    public GameObject gameOption;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
        gState = GameState.Ready;
        gameText = gameLabel.GetComponent<Text>();
        gameText.text = "READY";
        //gameText.color = new Color32(255, 185, 0, 255);

        StartCoroutine(ReadyToStart());
    }

    IEnumerator ReadyToStart()
    {
        yield return new WaitForSeconds(2f);
        gameText.color = new Color32(255, 185, 0, 255);
        gameText.text = "GO";
        yield return new WaitForSeconds(0.5f);
        gameLabel.SetActive(false);
        gState = GameState.Run;
        //print(gState);
    }

    public void OpenOptionWindow() //�ɼ�â ���� �Լ� - Ȱ��ȭ, �ð�����, ���º���
    {
        gameOption.SetActive(true);
        Time.timeScale = 0f;
        gState = GameState.Pause;
    }

    public void CloseOptionWindow() //�ɼ�â �ݱ� �Լ�
    {
        gameOption.SetActive(false );
        Time.timeScale = 1f;
        gState = GameState.Run;
    }

    public void RestartGame() //����� �ɼ�
    {
        Time.timeScale = 1f;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    void Update()
    {
        if (player.hp <= 0)
        {
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0f);
            gameLabel.SetActive(true);
            gameText.text = "GAME OVER";
            gameText.color = new Color32(255, 0, 0, 255);

            Transform buttons = gameText.transform.GetChild(0);
            buttons.gameObject.SetActive(true);

            gState = GameState.GameOver;
            hitEffect.SetActive(true);
        }
    }
}
