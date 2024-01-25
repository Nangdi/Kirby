using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear : MonoBehaviour
{
    private Animator animator;
    public GameObject White;
    public GameObject GameClear;
    private BgmManager BGM;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            BGM = FindObjectOfType<BgmManager>();
            BGM.BgmStop();
         BGM.BgmPlay(3);
            animator.Play("BoxOpen");
            StartCoroutine(time());
            
        }
    }
    IEnumerator time()
    {
        yield return new WaitForSeconds(1.2f);
        White.SetActive(true);
        GameManager.Instance.gameClear = true;
        yield return new WaitForSeconds(2f);
        GameClear.SetActive(true);
        Destroy(gameObject);
        

    }
   
}
