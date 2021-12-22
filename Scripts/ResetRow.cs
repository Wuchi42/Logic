using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRow : MonoBehaviour
{
    GameField Lodzik;

    void Start()
    {
        Lodzik = GameObject.Find("GameObject").GetComponent<GameField>();
    }

    void OnMouseUp()
    {
        Lodzik.resetRowButton();
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    void OnMouseDown()
    {
        transform.localScale = new Vector3(1f, 0.25f, 1f);
    }
}
