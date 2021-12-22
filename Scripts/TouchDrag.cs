using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDrag : MonoBehaviour
{
    private Vector3 mOffset;

    private Global global;

    void Start()
    {
        global = GameObject.Find("GameObject").GetComponent<Global>();
    }

    private float mZCoord;

    void OnTouchDown()
    {
        mZCoord =
            Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        mOffset = gameObject.transform.position - GetTouchAsWorldPoint();
        mOffset.z = Mathf.Round(mOffset.z);
        mOffset.x = Mathf.Round(mOffset.x);
        mOffset.y = 2; // some offset added to let player see pin behind cursor/finger
        transform.position = GetTouchAsWorldPoint() + mOffset;
        global.mouseDown = true;
    }

    void OnTouchUp()
    {
        mZCoord =
            Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetTouchAsWorldPoint(); //smooth transition
        mOffset.z = Mathf.Round(mOffset.z);
        mOffset.x = Mathf.Round(mOffset.x);
        mOffset.y = 2f;
        transform.position = GetTouchAsWorldPoint() + mOffset;
        global.mouseDown = false;
    }

    private Vector3 GetTouchAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 returnValue = new Vector3(0f, 0f, 0f);
        Touch touch;
        Vector3 touchPoint;
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            touchPoint = touch.position;

            // z coordinate of game object on screen
            touchPoint.z = mZCoord;
            returnValue = Camera.main.ScreenToWorldPoint(touchPoint);
            returnValue.y = 0; // we are setting this like a constant to avoid flying above the game table
            // Convert it to world points
        }
        return returnValue;
    }

    void OnTouchDrag()
    {
        transform.position = GetTouchAsWorldPoint() + mOffset;
        transform.rotation = Quaternion.Euler(180.0f, 0.0f, 0.0f); // rotate pin to static position
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            transform.position = GetTouchAsWorldPoint() + mOffset;
            transform.rotation = Quaternion.Euler(180.0f, 0.0f, 0.0f);
        }
    }
}
