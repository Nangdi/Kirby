using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField]
    public int typeIndex;
    public Type type;
    public bool isDie;
    public bool inhaling;
    protected Animator animator;
    protected BoxCollider2D Kirby_col;
    
    protected bool DieEffectisOn;

    private void Start()
    {
   
        Kirby_col = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
    }
   
    //protected IEnumerator Die_co()
    //{
    //    yield return new WaitForSeconds(2f);
    //    Destroy(gameObject);
    //}
}
