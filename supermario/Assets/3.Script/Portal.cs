using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
  
  
    public string sceneToLoad;
    private void Start()
    {

   
    }
    private void Update()
    {
        if (GameManager.Instance.canPortal)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SceneManager.LoadScene(sceneToLoad);
                GameManager.Instance.canPortal = false;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.canPortal = true;
        }
   
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        GameManager.Instance.canPortal = false;
    }

}
