using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public Vector2 boomerangDirection;
    private Vector2 KirbyPosition;
    private KirbyMove Kirby;
    
    public float BoomerangSpeed;
    private float boomerangDirection_float;
    private bool isDoublejump;
    //public Transform returnOb;
     
    // Update is called once per frame
    private void Awake()
    {
        KirbyPosition = Vector2.zero;
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out Kirby);
        boomerangDirection_float = Kirby.GetboomerangDirection();
        boomerangDirection = new Vector2(boomerangDirection_float, 0);



    }
    private void Start()
    {
        Kirby.BoomerangSound();
    
    }
    void Update()
    {
        
        //�������� �Ұ� �ٹ޾ƿ���
        isDoublejump = Kirby.GetIsDoubleJump();
        //transform.position += (Vector3)boomerangDirection * BoomerangSpeed * Time.deltaTime;
        //�θ޶��� �����¹��� ,�ӵ�
                             // �����ִ� �������� ���ǵ尪��ŭ �̵�
        transform.Translate(boomerangDirection * BoomerangSpeed * Time.deltaTime);

        //������ ���� �ӵ��� �پ��

        BoomerangSpeed -= boomerangDirection_float * boomerangDirection_float*Time.deltaTime*60f;
        // �θ޶��� ���⶧�� Ŀ�� ��ġ ����
        if (BoomerangSpeed <1 && BoomerangSpeed >-1)
        {
            KirbyPosition = Kirby.transform.position;
        }
        // ���ƿö� ������ ��ġ�� �̵�
        if (BoomerangSpeed < 0)
        {

            transform.position = Vector2.MoveTowards(transform.position, KirbyPosition, boomerangDirection_float * 4 *Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isDoublejump)
        {
            Kirby.animator.Play("Catch");
            Destroy(gameObject);
        }
    }
    
}
