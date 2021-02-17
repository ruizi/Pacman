using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman_move : MonoBehaviour
{
    //吃豆人的移动速度
    public float speed = 0.35f;
    //吃豆人下一次移动将要去的目的地
    private Vector2 dest = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        //保证吃豆人在游戏刚开始的时候不会动
        dest = this.transform.position;
    }

   
   
    private void FixedUpdate()
    {
        //插值得到要移动到dest位置的下一次移动坐标
        Vector2 temp = Vector2.MoveTowards(transform.position, dest, speed);
        //通过刚体来设置物体的位置
        GetComponent<Rigidbody2D>().MovePosition(temp);
        //必须先到达上一个dest的位置才可以发出新的目的地设置指令
        if ((Vector2)transform.position == dest)
        {
            if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && Valid(Vector2.up))
            {
                dest = (Vector2)transform.position + Vector2.up;
            }
            if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && Valid(Vector2.down))
            {
                dest = (Vector2)transform.position + Vector2.down;
            }
            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && Valid(Vector2.left))
            {
                dest = (Vector2)transform.position + Vector2.left;
            }
            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && Valid(Vector2.right))
            {
                dest = (Vector2)transform.position + Vector2.right;
            }
            //获取移动方向
            Vector2 dir = dest - (Vector2)transform.position;
            //把获取到的移动方向设置给动画状态机
            GetComponent<Animator>().SetFloat("DirX", dir.x);
            GetComponent<Animator>().SetFloat("DirY", dir.y); 

        }

    }
    //检测将要去的位置是否可以到达
    private bool Valid(Vector2 dir)
    {
        Vector2 pos = transform.position;
        //从下一步位置打一条射线回自身位置
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        //做边缘检查，如果射线打中的刚体不是自身（墙或者其他障碍物）时返回false，表示改步移动非法，否则合法，可执行
        return (hit.collider == GetComponent<Collider2D>());
    }
}
