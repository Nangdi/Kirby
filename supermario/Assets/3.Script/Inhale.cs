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
                // 2�� �̻� Ʈ���� ������ �̺�Ʈ�� ���ӵǸ� ���ϴ� ������ �����մϴ�.

               
                kirby.animator.Play("SuccessInhale");
                wall.isInhaile = true;
                kirby.big = true;
            }
        }
        else
        {
            // Ʈ���� �����̰� �������� �ʱ�ȭ�մϴ�.
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
            

            // �� �����Ӵ� �̵��� �Ÿ��� ��� (�ӵ� * �ð�)
            float moveSpeed = 2.0f; // ������ �ӵ��� ����
            float step = moveSpeed * Time.deltaTime;

            // ���� ��ġ�� Ŀ�� ��ġ�� �̵�
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
    //public LayerMask objectLayer; // �����Ϸ��� ������Ʈ ���̾�
    //public Vector2 center; // ������ �߽� ��ġ
    //public Vector2 size; // ������ ũ��

    //void Update()
    //{
    //    // �κ������� ����ϰ� ���� ������ �����մϴ�.
    //    float halfWidth = 1.0f; // ���� ��� X �� �������� -1���� 1������ �κ������� ����Ϸ��� �մϴ�.
    //    float halfHeight = 2.0f; // ���� ��� Y �� �������� -2���� 2������ �κ������� ����Ϸ��� �մϴ�.

    //    // OverlapBox�� ȣ���Ͽ� �κ����� ���� ���� ������Ʈ�� �����մϴ�.
    //    Collider2D[] colliders = Physics2D.OverlapBoxAll(center, size, 0, objectLayer);

    //    foreach (Collider2D collider in colliders)
    //    {
    //        Debug.Log("ã��");
    //        // �� ������ ���Ե� ������Ʈ�� ���� ó���� �����մϴ�.
    //    }
    //}
}
