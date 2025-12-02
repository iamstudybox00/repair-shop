using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumer : MonoBehaviour
{
    enum State
    {
        GO,
        WAITREPAIR,
        WAITLINE,
        HOME1,
        HOME2
    }

    [SerializeField]
    float speed = 0.05f;        // 이동속도

    Vector3[] path;
    
    Item item;
    Table target;
    Consumer front;             // 나의 앞 손님 오브젝트

    Animator anim;

    State state;

    // 시작위치, 이동경로, 아이템, 아이템을 놓을 테이블
    public void Init(Vector3 position, Vector3[] path, Table target, Consumer prevConsumer = null)     
    {
        transform.position = position;
        this.path = path;
        this.target = target;
        front = prevConsumer;
        item.Pick(transform);
        item.Init();

        item.transform.localPosition = new Vector3(0.0f, 1.0f, -0.5f);
        if (prevConsumer && prevConsumer.gameObject == gameObject)
        {
            front = null;
        }
        anim = GetComponentInChildren<Animator>();
        state = State.GO;
        transform.eulerAngles = new Vector3(0.0f, -180.0f, 0.0f);
        gameObject.SetActive(true);
        anim.SetBool("isMove", true);
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        switch (state)
        {
            case State.GO:
                if (front)
                {
                    Vector3 targetPosition = new Vector3(front.transform.position.x, front.transform.position.y, front.transform.position.z + 2.0f);
                    if (!transform.position.Equals(targetPosition))
                    {
                        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                        if (front.state == State.HOME1 || !front.gameObject.activeSelf)
                        {
                            front = null;
                            anim.SetBool("isMove", true);
                        }
                    }
                    else
                    {
                        anim.SetBool("isMove", false);
                    }

                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, path[0], speed * Time.deltaTime);
                }
                if (transform.position.Equals(path[0]))
                {
                    target.PickItem(item);
                    state = State.WAITREPAIR;
                    anim.SetBool("isMove", false);
                }
                
                break;
            case State.WAITREPAIR:
                if (!item.isActiveAndEnabled)
                {
                    Score.Get().Decrease(item.GetGameType(), item.GetDif());
                    state = State.HOME1;
                    anim.SetBool("isMove", true);
                    transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y - 90.0f, 0.0f);
                }
                else if(target.GetItem() && item.IsRepaired())  
                {
                    Score.Get().Increase(item.GetGameType(), item.GetDif());
                    state = State.HOME1;
                    item.Pick(transform);
                    anim.SetBool("isMove", true);
                    item.transform.localPosition = new Vector3(0.0f, 1.0f, -0.5f);
                    transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y - 90.0f, 0.0f);
                }
                break;
            case State.HOME1:
                transform.position = Vector3.MoveTowards(transform.position, path[1], speed * Time.deltaTime);
                if(transform.position.Equals(path[1]))
                {
                    state = State.HOME2;
                    transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y - 90.0f, 0.0f);
                }
                break;
            case State.HOME2:
                transform.position = Vector3.MoveTowards(transform.position, path[2], speed * Time.deltaTime);
                if (transform.position.Equals(path[2]))
                {
                    gameObject.SetActive(false);
                }
                break;
        }
    }

    public void SetItem(Item item)
    {
        item.transform.parent = transform;
        this.item = item;
    }

    public void DestroyConsumer()
    {
        if (item)
        {
            item.Pick(transform);
            Destroy(item.gameObject);
        }
        Destroy(gameObject);
    }
}
