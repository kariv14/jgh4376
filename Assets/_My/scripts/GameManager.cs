using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("���")]
    [Space(10)]
    public GameObject[] Stairs;
    public bool[] isTurn;

    private enum State {Start,Left,Right };
    private State state;
    private Vector3 oldPosition;

    [Header("UI")]
    [Space(10)]
    public GameObject UI_GameOver;
    public TextMeshProUGUI textMaxScore;
    public TextMeshProUGUI textNowScore;
    public TextMeshProUGUI textShowScore;
    private int maxScore = 0;
    private int nowScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        Init();
        InitStairs();
    }

    public void Init()
    {
        state = State.Start;
        oldPosition = Vector3.zero;

        isTurn = new bool[Stairs.Length];

        for (int i = 0; i < Stairs.Length; i++)
        {
            Stairs[i].transform.position = Vector3.zero;
            isTurn[i] = false;
        }

        nowScore = 0;

        textShowScore.text = nowScore.ToString();

        UI_GameOver.SetActive(false);
    }

    public void InitStairs()
    {
        for (int i = 0; i < Stairs.Length; i++)
        {
            switch (state)
            {
                case State.Start:
                    Stairs[i].transform.position = new Vector3(0.75f, -0.1f, 0);
                    state = State.Right;
                    break;
                case State.Left:
                    Stairs[i].transform.position = oldPosition + new Vector3(-0.75f, 0.5f, 0);
                    isTurn[i] = true;
                    break;
                case State.Right:
                    Stairs[i].transform.position = oldPosition + new Vector3(0.75f, 0.5f, 0);
                    isTurn[i] = false;
                    break;
            }
            oldPosition = Stairs[i].transform.position;

            if(i != 0)
            {
                int ran = Random.Range(0, 5);

                if(ran < 2 && i < Stairs.Length - 1)
                {
                    state = state == State.Left ? State.Right : State.Left;
                }
            }
        }
    }

    public void SpawnStair(int cnt)
    {
        int ran = Random.Range(0, 5);

        if (ran < 2)
        {
            state = state == State.Left ? State.Right : State.Left;
        }

        switch (state)
        {
            case State.Left:
                Stairs[cnt].transform.position = oldPosition + new Vector3(-0.75f, 0.5f, 0);
                isTurn[cnt] = true;
                break;
            
            case State.Right:
                Stairs[cnt].transform.position = oldPosition + new Vector3(0.75f, 0.5f, 0);
                isTurn[cnt] = false;
                break;
        }
        oldPosition = Stairs[cnt].transform.position;
    }

    public void GameOver()
    {
        StartCoroutine(ShowGameOver());
    }

    IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(1f);

        UI_GameOver.SetActive(true);

        if(nowScore > maxScore)
        {
            maxScore = nowScore;
        }

        textMaxScore.text = maxScore.ToString();
        textNowScore.text = nowScore.ToString();
    }

    public void AddScore()
    {
        nowScore++;
        textShowScore.text = nowScore.ToString();
    }

}


