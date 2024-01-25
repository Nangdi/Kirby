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
        
        //더블점프 불값 겟받아오기
        isDoublejump = Kirby.GetIsDoubleJump();
        //transform.position += (Vector3)boomerangDirection * BoomerangSpeed * Time.deltaTime;
        //부메랑의 나가는방향 ,속도
                             // 보고있는 방향으로 스피드값만큼 이동
        transform.Translate(boomerangDirection * BoomerangSpeed * Time.deltaTime);

        //나가고 점점 속도가 줄어듬

        BoomerangSpeed -= boomerangDirection_float * boomerangDirection_float*Time.deltaTime*60f;
        // 부메랑이 멈출때쯤 커비 위치 저장
        if (BoomerangSpeed <1 && BoomerangSpeed >-1)
        {
            KirbyPosition = Kirby.transform.position;
        }
        // 돌아올땐 저장한 위치로 이동
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
