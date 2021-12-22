using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    // Start is called before the first frame update
    GameField Lodzik;
    int Counter;
    
    void Start()
    {
       Lodzik = GameObject.Find("GameObject").GetComponent<GameField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseUp(){
        if(Counter == 1){
            Counter = 0;
            Lodzik.BackToMenu();
            transform.localScale = new Vector3(3f,1f,3f);
        }
        else{
            Counter++;
        }
    }
    void OnMouseDown(){
        transform.localScale = new Vector3(3f,0.25f,3f);
    }
}
