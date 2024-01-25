using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBGM : MonoBehaviour
{
    // Start is called before the first frame update
         private BgmManager BGM;
    void Start()
    {
        BGM = FindObjectOfType<BgmManager>();
        BGM.BgmStop();
        BGM.BgmPlay(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
