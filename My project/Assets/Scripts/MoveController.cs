using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 100.0f;

    [SerializeField]
    float rotSpeed = 200.0f;
    float rx;
    float ry;

    bool isPickItem;                  // 현재 아이템을 들고 있는가?
    float distance;              // 상호작용 최대범위
    RaycastHit hit;
    Item curItem;              // 현재 들고있는 아이템

    bool isPause;

    Vector3 dir;

    Rigidbody rigid;
    [SerializeField]
    GameObject head;
    
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        distance = 3.5f;

        ResetPlayer();
    }

    void Update()
    {
        rigid.velocity = Vector3.zero;
        if (!isPause)
        {
            Move();
            Interaction();
        }
    }

    void FixedUpdate()
    {
        if (!isPause)
        {
            rigid.MovePosition(transform.position + dir * moveSpeed * Time.deltaTime * 5);
        }
    }

    void Interaction()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!isPickItem)
            {
                PickItem();
            }
            else
            {
                RelaseItem();
            }
        }
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        dir = transform.right * h + new Vector3(transform.forward.x, 0, transform.forward.z) * v;
        dir.Normalize();

        float mx = Input.GetAxis("Mouse X") * (rotSpeed + 10);
        float my = Input.GetAxis("Mouse Y") * (rotSpeed + 10);

        rx += my * Time.smoothDeltaTime;
        ry += mx * Time.smoothDeltaTime;

        rx = Mathf.Clamp(rx, -80, 80);

        transform.eulerAngles = new Vector3(0, ry, 0);
        head.transform.eulerAngles = new Vector3(-rx, ry, 0);
    }
    void PickItem()
    {
        if (Physics.Raycast(head.transform.position, head.transform.forward, out hit, distance))
        {
            if (hit.transform.gameObject.CompareTag("Table"))     // 아이템일 경우
            {
                Table table = hit.transform.GetComponent<Table>();
                curItem = table.GetItem();

                if (curItem)
                {
                    curItem.Pick(head.transform);
                    isPickItem = true;
                    ItemPosition();
                }
            }

            if (hit.transform.gameObject.CompareTag("Item"))
            {
                curItem = hit.transform.GetComponent<Item>();
                curItem.Pick(head.transform);
                isPickItem = true;
                ItemPosition();
            }
        }
    }

    void RelaseItem()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance))      // 특정 장소에 아이템을 놓을때
        {
            if (hit.transform.gameObject.CompareTag("Table"))        // 아이템 테이블에 올려놓기
            {
                curItem.Release();
                Table cur = hit.transform.GetComponent<Table>();
                cur.PickItem(curItem, this);
                curItem = null;
                isPickItem = false;
            }
        }
        else
        {
            if (curItem.CanRelease())
            {
                curItem.Release();
                curItem.transform.parent = null;
                isPickItem = false;
            }
        }
    }

    public void Pause()
    {
        isPause = true;
    }

    public void Resume()
    {
        isPause = false;
    }

    public bool GetIsPause()
    {
        return isPause;
    }

    void ItemPosition()
    {
        curItem.transform.localPosition = new Vector3(1.0f, 0.1f, 1.0f);
    }

    public void ResetPlayer()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isPickItem = false;
        curItem = null;
        isPause = false;
        head.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        rx = 0.0f;
        ry = 0.0f;
        transform.position = new Vector3(0.0f, 1.0f, 0.0f);
        dir = new Vector3(0, 0, 1);
    }

    public void SetMouseSensitivity(float value)
    {
        rotSpeed = value;
    }
}
