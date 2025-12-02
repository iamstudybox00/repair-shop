using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MinigameObject
{
    Table curTable;
    [SerializeField]
    float rotatingSpeed = 50.0f;
    float rotating;
    Vector3 originRotate;

    Collider coll;
    Rigidbody rigid;

    Material[] mat;
    Color[] originColor;

    bool isRepaired;      // 수리가 되었는지 확인
    bool canRelease;      // 아이템을 놓을 수 있는 곳인지 확인

    void Awake()
    {
        gameObject.tag = "Item";
        coll = GetComponent<Collider>();
        rigid = GetComponent<Rigidbody>();
        rigid.useGravity = true;
        mat = GetComponent<MeshRenderer>().materials;
        originColor = new Color[mat.Length];
        for (int i = 0; i < mat.Length; i++)
        {
            originColor[i] = mat[i].color;
        }
        originRotate = new Vector3(transform.eulerAngles.x, 90.0f, transform.eulerAngles.z);
        rotating = 90.0f;
        ResetRotate();
        isRepaired = false;
        canRelease = true;
    }

    public void Init()
    {
        OffTable();
        isRepaired = false;
        PickScale();
        BrokenColor();
        RandomMinigame();

        gameObject.SetActive(true);
    }

    public void Pick(Transform parent)      // 집어질 때 실행됨
    {
        OffTable();
        coll.isTrigger = true;
        transform.parent = parent;
        rigid.useGravity = false;
        rigid.isKinematic = true;
        ResetRotate();
    }

    public void Release()
    {
        coll.isTrigger = false;
        rigid.isKinematic = false;
        transform.parent = null;
        rigid.useGravity = true;
        if(transform.position.y < 0)     // 바닥 떨어짐 방지
        {
            transform.position = new Vector3(transform.position.x, 0.08f, transform.position.z);
        }
    }

    void RandomMinigame()
    {
        diffcult = Random.Range(0, 3);
        gameType = Random.Range(0, 5);
    }

    public void OnTable(Table table)           // 테이블에 올려졌을 때 효과
    {
        if(curTable)
        {
            return;
        }
        curTable = table;

        coll.isTrigger = false;
        rigid.isKinematic = true;
        ResetRotate();
        gameObject.SetActive(true);
    }

    public void OffTable()
    {
        if(!curTable)
        {
            return;
        }    

        curTable.ReleaseItem();
        curTable = null;
        rotating = originRotate.y;
        ResetScale();
    }

    public void Rotating()          // 테이블에 올려졌을 때 회전
    {
        rotating += rotatingSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(originRotate.x, rotating, originRotate.z);

    }

    public void ResetScale()         // 크기 위치 등의 변화를 초기화
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void PickScale()
    {
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }

    void ResetRotate()
    {
        transform.localEulerAngles = originRotate;
    }

    public override void Clear()
    {
        isRepaired = true;
        RepairColor();
    }

    public override void Fail()
    {
        OffTable();
        gameObject.SetActive(false);
    }

    public bool IsRepaired()
    {
        return isRepaired;
    }

    void BrokenColor()
    {
        for (int i = 0; i < mat.Length; i++)
        {
            mat[i].color = Color.black;
        }
    }

    void RepairColor()
    {
        for (int i = 0; i < mat.Length; i++)
        {
            mat[i].color = originColor[i];
        }
    }

    public bool CanRelease()
    {
        return canRelease;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Wall")
        {
            canRelease = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            canRelease = true;
        }
    }
}
