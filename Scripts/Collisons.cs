using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisons : MonoBehaviour
{
    [SerializeField]
    private GameField lodzik;

    [SerializeField]
    private Global global;

    private int column;

    private int row;


    void Start()
    {
        lodzik = GameObject.Find("GameObject").GetComponent<GameField>();
        global = GameObject.Find("GameObject").GetComponent<Global>();
        if (gameObject.transform.name.Length == 4)
        {
            column = int.Parse(gameObject.transform.name.Substring(2, 2)); // get column of anti pin from a name
            row = int.Parse(gameObject.transform.name.Substring(0, 2)); // get row of anti pin from a name
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (
            lodzik.colorTable.GetElement(row, column) ==
            lodzik.colorTable.GetBlank() &&
            !global.mouseDown
        )
        {
            //if is spot empty you can put pin to place
            Debug.Log(lodzik.colorTable.GetElement(row, column));
            if (
                gameObject.transform.tag == "AntiPinBox" &&
                (
                collision.transform.tag == "Pin" ||
                collision.transform.tag == "PinToFreeze"
                )
            )
            {
                lodzik
                    .colorTable
                    .SetElement(row, column, collision.transform.name); // put pin into table
                collision.transform.tag = "PinToFreeze";
                collision.transform.position =
                    gameObject.transform.position + new Vector3(0f, 2.0f, 0f);
                collision.transform.rotation =
                    Quaternion.Euler(180.0f, 0.0f, 0.0f);
                Rigidbody gameObjectsRigidBody =
                    collision.transform.GetComponent<Rigidbody>();
                gameObjectsRigidBody.constraints =
                    RigidbodyConstraints.FreezeAll; // freeze pin in place
            }
            if (lodzik.aI != 1)
            {
                if (
                    gameObject.transform.tag == "MiniAntiPinBox" &&
                    collision.transform.tag == "MiniPin" ||
                    collision.transform.tag == "MiniPinToFreeze" &&
                    row <= lodzik.currentRow
                )
                {
                    Rigidbody gameObjectsRigidBody =
                        collision.transform.GetComponent<Rigidbody>();
                    collision.transform.position =
                        gameObject.transform.position +
                        new Vector3(0f, 2.0f, 0f);
                    collision.transform.rotation =
                        Quaternion.Euler(180.0f, 0.0f, 0.0f);
                    gameObjectsRigidBody.constraints =
                        RigidbodyConstraints.FreezeAll;
                }
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (global.mouseDown)
        {
            Rigidbody gameObjectsRigidBody =
                collision.transform.GetComponent<Rigidbody>();
                gameObjectsRigidBody.constraints = RigidbodyConstraints.None;
            if (
                gameObject.transform.tag == "AntiPinBox" &&
                collision.transform.tag == "PinToFreeze" &&
                lodzik.colorTable.GetElement(row, column) !=
                lodzik.colorTable.GetBlank()
            )
            {
                collision.transform.tag = "Pin";
                lodzik.colorTable.eraseElement (row, column); // erase element from a table when is held by player
            }
        }
    }
}
