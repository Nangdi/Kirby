using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1BGM : MonoBehaviour
{
    // Start is called before the first frame update
         private BgmManager BGM;
    void Start()
    {
       
        GameManager.Instance.currentType = Type.Normal;
        BGM = FindObjectOfType<BgmManager>();
         BGM.BgmStop();
        BGM.BgmPlay(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
