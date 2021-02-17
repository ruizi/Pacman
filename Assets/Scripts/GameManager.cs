using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }
    public GameObject pacman;
    public GameObject blinky;
    public GameObject clyde;
    public GameObject inky;
    public GameObject pinky;
    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject startCountDownPrefab;
    public GameObject gameoverPrefab;
    public GameObject winPrefab;
    public AudioClip startClip;
    public Text remainText;
    public Text nowText;
    public Text scoreText;

    public bool isSuperPacman = false;
    public List<int> usingIndex = new List<int>();
    public List<int> rawIndex = new List<int> { 0, 1, 2, 3 };
    private List<GameObject> pacdotGos = new List<GameObject>();

    private int pacdotNum = 0;
    private int nowEat = 0;
    public int score = 0;

    private void Awake()
    {
        _instance = this;
        Screen.SetResolution(1024, 768, false);
        int tempCount = rawIndex.Count;
        for (int i = 0; i < tempCount; i++)
        {
            int tempIndex = Random.Range(0, rawIndex.Count);
            usingIndex.Add(rawIndex[tempIndex]);
            rawIndex.RemoveAt(tempIndex);
            
        }
        foreach(Transform t in GameObject.Find("Maze").transform)
        {
            pacdotGos.Add(t.gameObject);
        }
        pacdotNum = GameObject.Find("Maze").transform.childCount;
    }

    private void Start()
    {
        SetGameState(false);
    }

    private void Update()
    {
        if (nowEat == pacdotNum && pacman.GetComponent<Pacman_move>().enabled != false)
        {
            gamePanel.SetActive(false);
            Instantiate(winPrefab);
            StopAllCoroutines();
            SetGameState(false);
        }
        if (nowEat == pacdotNum)
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(0);
            }
        }
        if (gamePanel.activeInHierarchy)
        {
            remainText.text = "Remain:\n" + (pacdotNum - nowEat);
            nowText.text = "Eaten:\n" + nowEat;
            scoreText.text = "Score:\n" + score;
        }
    }

    public void OnStartButton()
    {
        Debug.Log("touch");
        startCountDownPrefab.SetActive(true);
        StartCoroutine(PlayStartCountDown());
        AudioSource.PlayClipAtPoint(startClip, new Vector3(0, 0, -5));
        startCountDownPrefab.SetActive(false);
        startPanel.SetActive(false);
    }
    public void OnExitButton()
    {
        Application.Quit();
    }

    IEnumerator PlayStartCountDown()
    {
        GameObject go = Instantiate(startCountDownPrefab);
        yield return new WaitForSeconds(4f);
        Destroy(go);
        SetGameState(true);
        Invoke("CreateSuperPacdot", 10f);
        gamePanel.SetActive(true);
        GetComponent<AudioSource>().Play();
    }

    public void OnEatPacdot(GameObject go)
    {
        nowEat++;
        score += 100;
        pacdotGos.Remove(go);
    }
    public void OnEatSuperPacdot()
    {
        score += 200;
        Invoke("CreateSuperPacdot", 5f);
        isSuperPacman = true;
        FreezeEnemy();
        StartCoroutine(RecoveryEnemy());
    }
    private void CreateSuperPacdot()
    {
        if(pacdotGos.Count<6)
        {
            return;
        }
        int TempIndex = Random.Range(0, pacdotGos.Count);
        pacdotGos[TempIndex].transform.localScale = new Vector3(3, 3, 3);
        pacdotGos[TempIndex].GetComponent<Pacdot>().issuperPacdot = true;
    }
    IEnumerator RecoveryEnemy()
    {
        yield return new WaitForSeconds(3f);
        DisFreezeEnemy();
        isSuperPacman = false;
    }

    private void FreezeEnemy()
    {
        blinky.GetComponent<Ghost_move>().enabled = false;
        clyde.GetComponent<Ghost_move>().enabled = false;
        inky.GetComponent<Ghost_move>().enabled = false;
        pinky.GetComponent<Ghost_move>().enabled = false;
        blinky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        clyde.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        inky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        pinky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
    }
    private void DisFreezeEnemy()
    {
        blinky.GetComponent<Ghost_move>().enabled = true;
        clyde.GetComponent<Ghost_move>().enabled = true;
        inky.GetComponent<Ghost_move>().enabled = true;
        pinky.GetComponent<Ghost_move>().enabled = true;
        blinky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        clyde.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        inky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        pinky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }
    public void SetGameState(bool state)
    {
        pacman.GetComponent<Pacman_move>().enabled = state;
        blinky.GetComponent<Ghost_move>().enabled = state;
        clyde.GetComponent<Ghost_move>().enabled = state;
        inky.GetComponent<Ghost_move>().enabled = state;
        pinky.GetComponent<Ghost_move>().enabled = state;
    }
}
