using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
    public enum Type
    {
        Normal,
        Cutter
    };


public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance {
        get
        {
            if(instance == null)
            {
                return null;
            }
            return instance;
        }
     }


    
    private Slider HPbar;
    private Text Type;
    private Text life;
    private BgmManager Bgm;
    public float CurHP =3;
    private float MaxHP= 3;
    public int lifecount = 2;
    public bool isHit;
    public bool isDie;
    public Type currentType = global::Type.Normal;
    public bool canPortal;
    public bool gameClear;
    public bool GameOver;
 
    private void Awake()
    {
        //instance = this;
        //DontDestroyOnLoad(this.gameObject);
       
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        HPbar = FindObjectOfType<Slider>();
        Bgm = FindObjectOfType<BgmManager>();
        life = GameObject.FindGameObjectWithTag("Life").GetComponent<Text>();
        Type = GameObject.FindGameObjectWithTag("TypeText").GetComponent<Text>();

        HPbar.value = CurHP / MaxHP;
        UITpye();

    }
        
    // Update is called once per frame
    void Update()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "Intro")
        {
            Destroy(gameObject);
        }
        UITpye();
        HandleHp();
        if (CurHP <=0)
        {
            isDie = true;

        }

        //if (isDie)
        //{
        //    isDie = false;
          

        //}
        
    }
    private void HandleHp()
    {
        HPbar.value =Mathf.Lerp(HPbar.value , CurHP / MaxHP , Time.deltaTime*10  );
    }
    public void UITpye()
    {
        Type.text = currentType.ToString();
        life.text = "X " + lifecount.ToString();

    }
   
}
