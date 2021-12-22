using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColors : MonoBehaviour
{
    // Start is called before the first frame update
    GameField Lodzik;

    void Start()
    {
        Lodzik = GameObject.Find("GameObject").GetComponent<GameField>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnMouseUp()
    {
        Lodzik.gamePartEnd();
        transform.localScale = new Vector3(10f, 2f, 1f);
    }

    void OnMouseDown()
    {
        transform.localScale = new Vector3(10f, 0.25f, 1f);
    }
}
