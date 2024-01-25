using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClear : MonoBehaviour
{
    // Start is called before the first frame update
    private void Update()
    {
        if (GameManager.Instance.gameClear)
        {
            gameObject.SetActive(true);
        }
    }
}
