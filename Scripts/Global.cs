using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Global : MonoBehaviour
{
    // Start is called before the first frame update
    public bool mouseDown; // is mouse currently cliked | is display touched

    public GameObject menuButtons;

    public Camera[] cameras;

    public Mesh objectTable;

    public Material materialTable;

    //public Material materialGrass;
    private GameObject lodzik;

    void Update()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    /*public void createGrass(){
        var gameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
        gameObject.name = "Grass";
        gameObject.AddComponent<TableCollision>();
        gameObject.GetComponent<Renderer>().material = materialGrass;
        gameObject.transform.position = new Vector3(0,-1,0);
        gameObject.transform.localScale = new Vector3(25,1,25);
    }*/
    private void createTableBox()
    {
        //creating box for pins at position with mesh and scale

        var gameObject = new GameObject();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<Renderer>().material = materialTable;
        var meshFilter =
            gameObject.AddComponent<MeshFilter>().sharedMesh = objectTable;
        gameObject.name = ("TableBox");
        gameObject.AddComponent<TableCollision>();
        gameObject.transform.localScale = new Vector3(50f, 36.4f, 30f);
        gameObject.transform.position = new Vector3(13f, -28.3999996f, 13f);
        gameObject.AddComponent<MeshCollider>();
    }

    public void newGameLodzik()
    {
        // if game is not already exist calling starter function
        if (!lodzik)
        {
            GameObject.Find("GameObject").GetComponent<GameField>().Starter();
            lodzik = GameObject.Find("Lodzik");
        }
        else
        {
            lodzik.SetActive(!lodzik.activeSelf);
            GameObject.Find("GameObject").GetComponent<GameField>().Restart(); // same as starter plus destroying current game
        }
        Debug.Log("NewGame");
        toggleMenuButtons(); // hiding buttons of menu
    }

    public void resumeGameLodzik()
    {
        if (!lodzik)
        {
            // when game is not existing create new one
            GameObject.Find("GameObject").GetComponent<GameField>().Starter();
            lodzik = GameObject.Find("Lodzik");
        }
        else
        {
            lodzik.SetActive(!lodzik.activeSelf); // resume by activating self
            toggleMenuButtons(); // hiding buttons of menu
        }
    }

    public void giveUpGameLodzik()
    {
        if (!lodzik)
        {
        }
        else
        {
            toggleMenuButtons();
            lodzik.SetActive(!lodzik.activeSelf);
            GameObject
                .Find("GameObject")
                .GetComponent<GameField>()
                .functionLose(); // function that shows player right combination and disable to play anymore
        }
    }

    public void toggleMenuButtons()
    {
        //enable /disable visualisation of buttons
        menuButtons.SetActive(!menuButtons.activeSelf);
    }

    public void toggleGameObjects(string tag)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void RestartLodzik()
    {
        GameObject.Find("GameObject").GetComponent<GameField>().Starter();
    }

    public GameObject GetGameObjectByTagAndName(string tag, string name)
    {
        // custom function to find objects by name and tag
        GameObject[] tagField;
        tagField = GameObject.FindGameObjectsWithTag(tag);
        if (tagField.Length != 0)
        {
            foreach (GameObject tagElement in tagField)
            {
                if (tagElement.transform.name == name)
                {
                    return tagElement;
                }
            }
        }
        return null;
    }

    public void putOneObjectOnAnother(GameObject first, GameObject second)
    {
        // custom function to stick object together
        if (first && second)
        {
            first.transform.position =
                second.transform.position + new Vector3(0f, 2.0f, 0f);
            first.transform.rotation = Quaternion.Euler(180.0f, 0.0f, 0.0f);
        }
        else
        {
            print("Error no objects defined");
        }
    }
}
