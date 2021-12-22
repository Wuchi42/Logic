using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableCollision : MonoBehaviour
{
    private GameField lodzik;

    void Start()
    {
        lodzik = GameObject.Find("GameObject").GetComponent<GameField>();
    }

    void OnCollisionEnter(Collision collision)
    {
        collision.transform.position =
            new Vector3(Random
                    .Range(3 + ((lodzik.maxColumn + 0) * 2),
                    -2 + ((Mathf.RoundToInt(lodzik.maxColumn * 1.5f) + 1) * 3)),
                Random.Range(1, 40),
                Random.Range(1, (lodzik.maxRow * 2) - 3));
    }
}
