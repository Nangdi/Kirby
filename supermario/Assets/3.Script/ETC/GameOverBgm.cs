using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverBgm : MonoBehaviour
{
    private BgmManager Bgm;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.isDie = false;
        GameManager.Instance.CurHP = 3;
        GameManager.Instance.currentType = Type.Normal;
        GameManager.Instance.lifecount = 2;
       
        Bgm = FindObjectOfType<BgmManager>();
        Bgm.BgmStop();
    }

}
