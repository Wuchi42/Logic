using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    private Vector3 mOffset;

    private Global global;

    void Start()
    {
        global = GameObject.Find("GameObject").GetComponent<Global>();
    }

    private float mZCoord;

    void OnMouseDown()
    {
        const int zOffset = 2;
        mZCoord =
            Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        // Store offset = gameobject world pos - mouse world pos
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        mOffset.z = Mathf.Round(mOffset.z) + zOffset;
        mOffset.x = Mathf.Round(mOffset.x);
        mOffset.y = 2; // some offset added to let player see pin behind cursor/finger

        transform.position = GetMouseAsWorldPoint() + mOffset;
        global.mouseDown = true;
    }

    void OnMouseUp()
    {
        mZCoord =
            Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        // Store offset = gameobject world pos - mouse world pos
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint(); //smooth transition
        mOffset.z = Mathf.Round(mOffset.z);
        mOffset.x = Mathf.Round(mOffset.x);
        mOffset.y = 2f;
        transform.position = GetMouseAsWorldPoint() + mOffset;
        global.mouseDown = false;
        // If your mouse hovers over the GameObject with the script attached, output this message
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 returnValue = new Vector3(0f, 0f, 0f);
        Touch touch;
        Vector3 mousePoint;
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            mousePoint = touch.position;
        }
        else
        {
            mousePoint = Input.mousePosition;
        }

        // z coordinate of game object on screen
        mousePoint.z = mZCoord;
        returnValue = Camera.main.ScreenToWorldPoint(mousePoint);
        returnValue.y = 0; // we are setting this like a constant to avoid flying above the game table

        // Convert it to world points
        return returnValue;
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseAsWorldPoint() + mOffset;
        transform.rotation = Quaternion.Euler(180.0f, 0.0f, 0.0f);
    }
}
