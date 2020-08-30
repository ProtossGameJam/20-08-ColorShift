using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAllToClear : MonoBehaviour
{

    private void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && GameObject.FindGameObjectsWithTag("Pink").Length == 0)
        {
            FindObjectOfType<ClearInputHandler>(true).gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
