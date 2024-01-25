using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inhale : MonoBehaviour
{
    //private MonsterController monster;
    [SerializeField]
    private KirbyMove kirby;
    [SerializeField]
    private Wall wall;
    private GameManager gameManager;
    Vector3 targetKirbyPosition;
    MonsterController monsterScript;
    private bool isTriggered;
    private float elapsedTime;
    private void Start()
    {
        GameObject.FindGameObjectWithTag("Wall").TryGetComponent<Wall>(out wall);
        GameObject.FindGameObjectWithTag("Player").TryGetComponent<KirbyMove>(out kirby);
        monsterScript = GameObject.FindGameObjectWithTag("Monster").GetComponent<MonsterController>();
    }
    private void Update()
    {
        if (isTriggered)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= 2.0f)
            {
                // 2초 이상 트리거 스테이 이벤트가 지속되면 원하는 동작을 실행합니다.

               
                kirby.animator.Play("SuccessInhale");
                wall.isInhaile = true;
                kirby.big = true;
            }
        }
        else
        {
            // 트리거 스테이가 끊어지면 초기화합니다.
            elapsedTime = 0.0f;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            GameObject.FindGameObjectWithTag("Wall").TryGetComponent<Wall>(out wall);
            isTriggered = true;
        }
        if (collision.CompareTag("Monster"))
        {
        targetKirbyPosition = kirby.gameObject.transform.position;
            monsterScript = collision.GetComponent<MonsterController>();
        }
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Monster"))
        {
            //monsterScript = collision.GetComponent<MonsterController>();
            monsterScript.inhaling = true;
            Vector3 currentMonsterPosition = collision.transform.position;
            

            // 한 프레임당 이동할 거리를 계산 (속도 * 시간)
            float moveSpeed = 2.0f; // 적절한 속도를 설정
            float step = moveSpeed * Time.deltaTime;

            // 몬스터 위치를 커비 위치로 이동
            collision.transform.position = Vector3.MoveTowards(currentMonsterPosition, targetKirbyPosition, step);
            
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            isTriggered = false;
        }
        if (collision.CompareTag("Monster"))
        {
            Monster monsterScript = collision.gameObject.GetComponent<Monster>();
            monsterScript.inhaling = false;
            
        }
    }
    //public LayerMask objectLayer; // 검출하려는 오브젝트 레이어
    //public Vector2 center; // 상자의 중심 위치
    //public Vector2 size; // 상자의 크기

    //void Update()
    //{
    //    // 부분적으로 사용하고 싶은 영역을 정의합니다.
    //    float halfWidth = 1.0f; // 예를 들어 X 축 방향으로 -1부터 1까지를 부분적으로 사용하려고 합니다.
    //    float halfHeight = 2.0f; // 예를 들어 Y 축 방향으로 -2부터 2까지를 부분적으로 사용하려고 합니다.

    //    // OverlapBox를 호출하여 부분적인 영역 내의 오브젝트를 검출합니다.
    //    Collider2D[] colliders = Physics2D.OverlapBoxAll(center, size, 0, objectLayer);

    //    foreach (Collider2D collider in colliders)
    //    {
    //        Debug.Log("찾음");
    //        // 이 영역에 포함된 오브젝트에 대한 처리를 수행합니다.
    //    }
    //}
}
