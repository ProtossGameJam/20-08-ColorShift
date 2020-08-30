using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerToClear : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<ClearInputHandler>(true).gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
