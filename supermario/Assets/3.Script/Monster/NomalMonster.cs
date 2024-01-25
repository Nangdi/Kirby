using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalMonster : MonsterController
{
   
 
    private float moveDirection = 1; // -1: 왼쪽, 0: 정지, 1: 오른쪽

    private Vector2 currentPositon;
  
    // Start is called before the first frame update
    void Start()
    {
        switch (typeIndex)
        {
            case 0:
                type = Type.Normal;
                break;
            case 1:
                type = Type.Cutter;
                break;
        }
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Kirby_col = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        StartCoroutine(Turn());
    }

    // Update is called once per frame
    void Update()
    {

        if (moveDirection < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
        if(!isDie)
        transform.position += new Vector3(moveSpeed * moveDirection * Time.deltaTime, 0, 0);

        if(isDie && !DieEffectisOn)
        {
            StartCoroutine(DieEffect_co());
        }
        animator.SetBool("Inhaling", inhaling);
    }
    protected override IEnumerator DieEffect_co()
    {
        return base.DieEffect_co();
    }
    protected override IEnumerator Die_co()
    {
        return base.Die_co();
    }

    IEnumerator Turn()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.5f);

            moveDirection = -1;

            yield return new WaitForSeconds(2.5f);

            moveDirection = 1;
        }

        //}
        //IEnumerator Die_co()
        //{
        //    yield return new WaitForSeconds(2f);
        //    Destroy(gameObject);
        //}
        //IEnumerator DieEffect_co()
        //{
        //    Color monsterA = spriteRenderer.color;

        //    DieEffectisOn = true;
        //    spriteRenderer.color = new Color(monsterA.r, monsterA.g, monsterA.b, 0.7f);

        //    yield return new WaitForSeconds(0.2f);
        //    DieEffectisOn = false;
        //    spriteRenderer.color = monsterA;


        //}
      
        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    Collider2D thisCollider = GetComponent<Collider2D>();
        //    if (collision.CompareTag("PlayerSkil"))
        //    {
        //        isDie = true;
        //        animator.Play("Die");
        //        Physics2D.IgnoreCollision(thisCollider, Kirby_col);

        //        StartCoroutine(Die_co());

        //    }
    }
    private void DieRotation()
    {
        Vector3 DieRote = new Vector3(0, 0, -30f);
        transform.rotation = Quaternion.Euler(DieRote);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
