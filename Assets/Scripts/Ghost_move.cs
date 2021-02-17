using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ghost_move : MonoBehaviour
{
    public GameObject[] wayPointGos;
    public float speed = 0.1f;
    private List<Vector3> wayPoints = new List<Vector3>();
    
    private int index1 = 0;
    private Vector3 startPos;

    private void Start()
    {
        Debug.Log(wayPointGos.Length);
        startPos = transform.position + new Vector3(0, 3, 0);
        LoadPath(wayPointGos[GameManager.Instance.usingIndex[GetComponent<SpriteRenderer>().sortingOrder - 2]]);
    }

    private void FixedUpdate()
    {
        if(transform.position!=wayPoints[index1])
        {
            //插值得到要移动到dest位置的下一次移动坐标
            Vector2 temp = Vector2.MoveTowards(transform.position, wayPoints[index1], speed);
            //通过刚体来设置物体的位置
            GetComponent<Rigidbody2D>().MovePosition(temp);
        }
        else
        {
            index1++;
            if(index1 >= wayPoints.Count)
            {
                index1 = 0;
                LoadPath(wayPointGos[Random.Range(0, wayPointGos.Length)]);
            }
            
        }
        //获取移动方向
        Vector2 dir = wayPoints[index1] - transform.position;
        //把获取到的移动方向设置给动画状态机
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);

    }

    private void LoadPath(GameObject go)
    {
        wayPoints.Clear();
        foreach (Transform t in go.transform)
        {
            wayPoints.Add(t.position);
        }
        wayPoints.Insert(0, startPos);
        wayPoints.Add(startPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Pacman")
        {
            if(GameManager.Instance.isSuperPacman)
            {
                //todo go back home
                transform.position = startPos - new Vector3(0, 3, 0);
                index1 = 0;
                GameManager.Instance.score += 500;
            }
            else
            {
                collision.gameObject.SetActive(false);
                GameManager.Instance.gamePanel.SetActive(false);
                GameManager.Instance.SetGameState(false);
                Instantiate(GameManager.Instance.gameoverPrefab);
                GameManager.Instance.gameoverPrefab.SetActive(true);
                Invoke("ReStart", 3f);
            }
        }
    }
    private void ReStart()
    {
        SceneManager.LoadScene(0);
    }
}
