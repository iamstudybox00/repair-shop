using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumerManager : MonoBehaviour
{
    Vector3[] movePosition;        // 손님 이동경로
    Vector3 startPosition;         // 생성 좌표

    [SerializeField]
    GameObject[] item;

    [SerializeField]
    GameObject[] ConsumerOrigin;
    Consumer[] consumerData;
    Consumer curConsumer;           // 지금 사용한 손님 오브젝트

    [SerializeField]
    Table target;               // 목표로 이동할 테이블

    int poolSize;                   // 미리 생성할 오브젝트 크기

    Coroutine cor;

    void Start()
    {
        MinigameManager.Get();
        Score.Get();

        movePosition = new Vector3[3];
        movePosition[0] = new Vector3(0.0f, 0.0f, 4.0f);
        movePosition[1] = new Vector3(2.0f, 0.0f, 4.0f);
        movePosition[2] = new Vector3(2.0f, 0.0f, 15.0f);

        startPosition = new Vector3(0.0f, 0.0f, 25.0f);
        poolSize = 10;

        consumerData = new Consumer[poolSize];
        for (int i = 0; i < poolSize; i++)        // 손님 오브젝트 미리 생성
        {
            consumerData[i] = Instantiate(ConsumerOrigin[Random.Range(0, ConsumerOrigin.Length)]).GetComponent<Consumer>();
            consumerData[i].gameObject.SetActive(false);
            int temp = Random.Range(0, item.Length);
            consumerData[i].SetItem(Instantiate(item[temp]).GetComponent<Item>());
        }
    }

    void InitConsumer()     // 생성해논 오브젝트를 사용할 때 호출
    {
        for (int i = 0; i < poolSize; i++)
        {
            if(!consumerData[i].gameObject.activeSelf)        // 비활성화 되어있는 오브젝트를 사용
            {
                consumerData[i].Init(startPosition, movePosition, target, curConsumer);
                curConsumer = consumerData[i];
                break;
            }
        }
    }

    public void ReloadConsumer()
    {
        for (int i = 0; i < poolSize; i++)        // 손님 오브젝트 미리 생성
        {
            consumerData[i].DestroyConsumer();
            consumerData[i] = Instantiate(ConsumerOrigin[Random.Range(0, ConsumerOrigin.Length)]).GetComponent<Consumer>();
            consumerData[i].gameObject.SetActive(false);
            int temp = Random.Range(0, item.Length);
            consumerData[i].SetItem(Instantiate(item[temp]).GetComponent<Item>());
        }
        SommonStop();
        SommonStart();
    }

    public void SommonStart()
    {
        cor = StartCoroutine(Sommon());
    }

    public void SommonStop()
    {
        if(cor != null)
        {
            StopCoroutine(cor);
        }
    }


    IEnumerator Sommon()
    {
        InitConsumer();
        yield return new WaitForSeconds(3.0f);
        cor = StartCoroutine(Sommon());
    }
}
