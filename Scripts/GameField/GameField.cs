using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour
{
    private int gamePart;

    private Global global;

    private GameObject gameField;

    private GameObject AllPins;

    private GameObject AntiPins;

    public Mesh objectAntiPin;

    public Mesh objectPin;

    public Mesh objectPinCover;

    public Mesh objectWholeBox;

    public Material materialDefault;
    public Material materialReset;

    public Material materialRestart;

    public Material materialQuit;

    public Material materialSetColors;

    public int maxColumn;

    public int currentRow;

    public int maxRow;

    public ColorTable colorTable;

    public int aI;

    public string[]
        colors =
        {
            "Red",
            "Blue",
            "Cyan",
            "Magenta",
            "Green",
            "Yellow",
            "White",
            "Black"
        };

    public void Starter()
    {
        global = GameObject.Find("GameObject").GetComponent<Global>();
        gameField = new GameObject("Lodzik");
        AntiPins = new GameObject("AntiPins");
        constructAllPins();
        gamePart = 0;
        colorTable = new ColorTable();
        createGameField();
        colorTable.ResetTable();
        AllPins.transform.SetParent(gameField.transform);
        nextGamePart();
    }

    public GameObject getAllPins()
    {
        return AllPins;
    }

    private void functionWin()
    {
        destroyWithTag("PinCover");
    }

    public void functionLose()
    {
        destroyWithTag("PinCover");
        destroyWithTag("AntiPinBox");
    }

    public int getIntFromNameOfColor(string name)
    {
        for (int i = 0; i < colors.Length; i++)
        {
            if (name == colors[i])
            {
                //  print(name);
                return i;
            }
        }
        return 9; //**in future here will be some error managment
    }

    public void constructAllPins()
    {
        //just setting up hiearchy of objects
        AllPins = new GameObject("AllPins");

        GameObject Pins = new GameObject("Pins"); // Pins have index 0 of parent
        Pins.transform.SetParent(AllPins.transform);
        GameObject FreezedPins = new GameObject("FreezedPins"); // FreezedPins have index 1 of parent
        FreezedPins.transform.SetParent(AllPins.transform);

        GameObject MiniPins = new GameObject("MiniPins"); // MiniPins have index 2 of parent
        MiniPins.transform.SetParent(AllPins.transform);
        GameObject FreezedMiniPins = new GameObject("FreezedMiniPins"); // FreezedPins have index 3 of parent
        FreezedMiniPins.transform.SetParent(AllPins.transform);
        for (int i = 0; i < AllPins.transform.childCount; i++)
        {
            if (i == 0)
            {
                foreach (string color in colors)
                {
                    GameObject Color = new GameObject(color);
                    Color
                        .transform
                        .SetParent(AllPins.transform.GetChild(i).transform);
                }
            }
            else if (i == 2)
            {
                for (int j = colors.Length - 2; j < colors.Length; j++)
                {
                    GameObject Color = new GameObject(colors[j]);
                    Color
                        .transform
                        .SetParent(AllPins.transform.GetChild(i).transform);
                }
            }
        }
        GameObject Answer = new GameObject("Answer");
        Answer.transform.SetParent(AllPins.transform);
    }

    void OnDestroy()
    {
        //  print("Script was destroyed");
    }

    private void AddDrag(GameObject pin)
    {
#if UNITY_EDITOR
        pin.AddComponent<MouseDrag>();
#elif UNITY_STANDALONE_WIN
                
                pin.AddComponent<MouseDrag>();
            #elif UNITY_ANDROID
                pin.AddComponent<MouseDrag>();
                //pin.AddComponent<TouchDrag>();
            #endif
    }

    private void destroyDrag(GameObject pin)
    {
#if UNITY_EDITOR
        var component = pin.GetComponent<MouseDrag>();
#elif UNITY_STANDALONE_WIN
               var component = pin.GetComponent<MouseDrag>();
            #elif UNITY_ANDROID
              //  var component = pin.GetComponent<TouchDrag>();
              var component = pin.GetComponent<MouseDrag>();
            #endif


        if (component != null)
        {
            Destroy (component);
        }
    }

    private void ResetCollisons()
    {
        GameObject[] antiPinBoxes;
        antiPinBoxes = GameObject.FindGameObjectsWithTag("AntiPinBox");

        foreach (GameObject antiPinBox in antiPinBoxes)
        {
            Destroy(antiPinBox.GetComponent<Collisons>());
            antiPinBox.AddComponent<Collisons>();
        }
        antiPinBoxes = GameObject.FindGameObjectsWithTag("MiniAntiPinBox");
        foreach (GameObject antiPinBox in antiPinBoxes)
        {
            Destroy(antiPinBox.GetComponent<Collisons>());
            antiPinBox.AddComponent<Collisons>();
        }
    }

    public void BackToMenu()
    {
        global.toggleMenuButtons();
        gameField.SetActive(!gameField.activeSelf);
    }

    public void Restart()
    {
        destroyWithTag("FreezedPin");
        destroyWithTag("FreezedMiniPin");
        destroyWithTag("MiniPinToFreeze");
        destroyWithTag("MiniPin");
        destroyWithTag("PinToFreeze");
        destroyWithTag("PinCover");
        destroyWithName("AntiPins");
        AntiPins = new GameObject("AntiPins");
        createAntiPins();
        colorTable.ResetTable();
        gamePart = 0;
        nextGamePart();
    }

    public void FreezePins(string tag)
    {
        GameObject[] pinsToFreeze;
        pinsToFreeze = GameObject.FindGameObjectsWithTag(tag);

        if (pinsToFreeze.Length != 0)
        {
            if (currentRow != maxRow + 1)
            {
                GameObject row = new GameObject("Row: " + currentRow);
                foreach (GameObject pinToFreeze in pinsToFreeze)
                {
                    if (tag == "PinToFreeze")
                    {
                        //pin
                        pinToFreeze.tag = "FreezedPin";
                        row
                            .transform
                            .SetParent(AllPins.transform.GetChild(1).transform); // set position in hiearchy of objects
                        pinToFreeze.transform.SetParent(row.transform);
                    }
                    else if (tag == "MiniPinToFreeze")
                    {
                        //minipin
                        pinToFreeze.tag = "FreezedMiniPin";
                        row
                            .transform
                            .SetParent(AllPins.transform.GetChild(3).transform); // set position in hiearchy of objects
                        pinToFreeze.transform.SetParent(row.transform);
                    }

                    //destroy all physical interactions with other objects
                    destroyDrag (pinToFreeze);
                    BoxCollider[] gameObjectsBoxColliders =
                        pinToFreeze.transform.GetComponents<BoxCollider>();
                    foreach (BoxCollider
                        gameObjectsBoxCollider
                        in
                        gameObjectsBoxColliders
                    )
                    {
                        Destroy (gameObjectsBoxCollider);
                    }

                    Rigidbody gameObjectsRigidBody =
                        pinToFreeze.transform.GetComponent<Rigidbody>();
                    Destroy (gameObjectsRigidBody);
                }
            } // if it is the goal row
            else
            {
                foreach (GameObject pinToFreeze in pinsToFreeze)
                {
                    pinToFreeze.tag = "FreezedPin";
                    pinToFreeze
                        .transform
                        .SetParent(AllPins.transform.GetChild(4).transform);
                    destroyDrag (pinToFreeze);
                    BoxCollider[] gameObjectsBoxColliders =
                        pinToFreeze.transform.GetComponents<BoxCollider>();
                    foreach (BoxCollider
                        gameObjectsBoxCollider
                        in
                        gameObjectsBoxColliders
                    )
                    {
                        Destroy (gameObjectsBoxCollider);
                    }
                    Rigidbody gameObjectsRigidBody =
                        pinToFreeze.transform.GetComponent<Rigidbody>();
                    Destroy (gameObjectsRigidBody);
                }
            }
        }
    }

    public int[] GetFreezedPinsColors(string tag)
    {
        GameObject[] pinsToFreeze;
        pinsToFreeze = GameObject.FindGameObjectsWithTag(tag);
        int[] colors = new int[pinsToFreeze.Length];
        if (pinsToFreeze.Length != 0)
        {
            for (int i = 0; i < pinsToFreeze.Length; i++)
            {
                colors[i] =
                    getIntFromNameOfColor(pinsToFreeze[i].transform.name);
            }
        }
        else
        {
            // print("There are not pins to freeze");
        }
        return colors;
    }

    public bool gamePartEnd()
    {
        bool returner = false;
        GameObject[] pinsToFreeze;
        pinsToFreeze = GameObject.FindGameObjectsWithTag("PinToFreeze");
        if (pinsToFreeze.Length == maxColumn || gamePart == 0)
        {
            returner = true;
            if (currentRow != maxRow + 1)
            {
                compareAndExecute();
                int[] colors;
                colors = GetFreezedPinsColors("MiniPinToFreeze");
                recreatePins("MiniPin", colors);
                FreezePins("MiniPinToFreeze");
                colors = GetFreezedPinsColors("PinToFreeze");
                recreatePins("Pin", colors);
                FreezePins("PinToFreeze");
                DestroyLastReset(currentRow);
                if (currentRow != maxRow){
                    ResetRow(currentRow+1);
                }
                
            }
            gamePart++;
            nextGamePart();
        }
        return returner;
    }

    private void recreatePins(string tag, int[] colors)
    {
        foreach (int color in colors)
        {
            createPin (objectPin, materialDefault, color, tag);
        }
    }

    private void destroyWithTag(string tag)
    {
        GameObject[] tmpField;
        tmpField = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject gameObject in tmpField)
        {
            Destroy (gameObject);
        }
    }

    private void destroyWithName(string name)
    {
        GameObject toDestroy;
        toDestroy = GameObject.Find(name);
        Destroy (toDestroy);
    }

    private GameObject findFreePinWithColor(string tag, int color)
    {
        string colorName = "White";
        GameObject pin;
        if (color >= 0 && color < colors.Length)
        {
            colorName = colors[color];
        }
        pin = global.GetGameObjectByTagAndName(tag, colorName);

        return pin;
    }

    private void compareAndExecute()
    {
        string[] tryColors = new string[maxColumn]; // colors from player input
        string[] trueColors = new string[maxColumn]; // colors that are goal to guess
        bool[] blacked = new bool[maxColumn]; // colors that are on right spot
        bool[] whitedTrueColors = new bool[maxColumn]; // colors from goal that are already count as whited
        bool[] whitedTryColors = new bool[maxColumn]; // colors  from player input that are already count as whited
        int blackMiniPinCount = 0; // counter of minipins
        int whiteMiniPinCount = 0; // counter of minipins
        for (int i = 0; i < maxColumn; i++)
        {
            // for each pin
            tryColors[i] = colorTable.GetElement(currentRow, i + 1); // get guessed color from color table
            trueColors[i] = colorTable.GetElement(maxRow + 1, i + 1); // get goal color from color table
            if (tryColors[i] == trueColors[i])
            {
                // if they are matching
                blackMiniPinCount++; // add one mini black pin
                blacked[i] = true; // the position is done
            }
        }
        for (int i = 0; i < maxColumn; i++)
        {
            // do it again now we looking for white minipins (good color wrong place)
            if (!blacked[i])
            {
                // if it was not blacked
                for (int j = 0; j < maxColumn; j++)
                {
                    // from first to last spot ***j*** is here representing TRUE COLORS
                    if (!blacked[j] && !whitedTrueColors[j])
                    {
                        // if that goal spot was neighter blacked or whited
                        if (!whitedTryColors[i])
                        {
                            // if that guess spot was not already whited
                            if (tryColors[i] == trueColors[j])
                            {
                                // if the guess color is represented in goal color
                                whitedTrueColors[j] = true; // goal color is whited
                                whitedTryColors[i] = true; // guess color is whited
                                whiteMiniPinCount++; // there will be one more white mini pin
                            }
                        }
                    }
                }
            }
        }
        setMiniPins (blackMiniPinCount, whiteMiniPinCount);
        if (blackMiniPinCount == maxColumn)
        {
            // if there are all black pins it means VICTORY
            functionWin();
        }
        else if (currentRow == maxRow)
        {
            // if it is last row it means DEFEAT
            functionLose();
        }
        else
        {
            // make another row to guess
            Debug.Log("compareAndExecute" + currentRow + 1);
            createRowAntiPins(currentRow + 1);
        }
    }

    private void setMiniPins(int blackMiniPinCount, int whiteMiniPinCount)
    {
        GameObject whiteMiniPin;
        GameObject blackMiniPin;
        GameObject antiPin;
        for (int i = 0; i < blackMiniPinCount + whiteMiniPinCount; i++)
        {
            antiPin = findAntiPin("MiniAntiPinBox", i + 1, currentRow);
            if (i < blackMiniPinCount)
            {
                blackMiniPin =
                    findFreePinWithColor("MiniPin",
                    getIntFromNameOfColor("Black"));
                global.putOneObjectOnAnother (blackMiniPin, antiPin);
                blackMiniPin.transform.tag = "MiniPinToFreeze";
            }
            else
            {
                whiteMiniPin =
                    findFreePinWithColor("MiniPin",
                    getIntFromNameOfColor("White"));
                global.putOneObjectOnAnother (whiteMiniPin, antiPin);
                whiteMiniPin.transform.tag = "MiniPinToFreeze";
            }
        }
    }

    private GameObject findAntiPin(string tag, int column, int row)
    {
        //print("findAntiPin" + " Tag: " + tag + " Column: " + column + " Row: " + row);
        return global
            .GetGameObjectByTagAndName(tag, getNameFromCollumnRow(column, row));
    }

    private int[] generateRandomRow()
    {
        int[] numbers = new int[maxColumn];

        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = Random.Range(0, colors.Length);
        }
        return numbers;
    }

    public string getNameFromCollumnRow(int column, int row)
    {
        string name = "";
        if (row < 10)
        {
            name = "0";
        }
        name = name + row;
        if (column < 10)
        {
            name = name + "0";
        }
        name = name + column;
        return name;
    }

    private void generateRandomSecretPinColorRow(int row)
    {
        int[] randomColors;
        GameObject[] pins = new GameObject[maxColumn];
        GameObject[] antiPins = new GameObject[maxColumn];
        randomColors = generateRandomRow();
        for (int i = 0; i < maxColumn; i++)
        {
            pins[i] = findFreePinWithColor("Pin", randomColors[i]);
            antiPins[i] = findAntiPin("AntiPinBox", i + 1, row);
            if (pins[i] != null && antiPins[i] != null)
            {
                pins[i].transform.tag = "PinToFreeze";
                global.putOneObjectOnAnother(pins[i], antiPins[i]);
                destroyWithName("AntiPin" + (i + 1) + row);
                colorTable.SetElement(row, i + 1, pins[i].transform.name);
            }
            else
            {
                //  ** some error handling in the future
            }
        }
    }
    private void generateRandomPinColorRow(int row)
    {
        int[] randomColors;
        randomColors = generateRandomRow();
        GameObject[] pins = new GameObject[maxColumn];
        GameObject[] antiPins = new GameObject[maxColumn];
        
        for (int i = 0; i < maxColumn; i++)
        {
            pins[i] = findFreePinWithColor("Pin", randomColors[i]);
            antiPins[i] = findAntiPin("AntiPinBox", i + 1, row);
            if (pins[i] != null && antiPins[i] != null)
            {
                pins[i].transform.tag = "PinToFreeze";
                global.putOneObjectOnAnother(pins[i], antiPins[i]);
            }
            else
            {
                //  ** some error handling in the future
            }
        }
    }
    private void firstGamePart()
    {
        currentRow = maxRow + 1;
        generateRandomSecretPinColorRow (currentRow);
        GameObject WholeCover = new GameObject("Cover");
        WholeCover.transform.SetParent(gameField.transform);
        GameObject cover;
        for (
            int numberAPColumn = 1;
            numberAPColumn <= maxColumn;
            numberAPColumn++
        )
        {
            cover =
                createPinCover(objectPinCover,
                materialDefault,
                numberAPColumn,
                maxRow + 2);
            cover.transform.SetParent(WholeCover.transform);
        }
        FreezePins("PinToFreeze");
        if (gamePartEnd())
        {
            // automatically proceding to next gamepart
            nextGamePartAnimation(); //** for future animations
        }
    }

    public void nextGamePart()
    {
        currentRow = gamePart;
        if (gamePart == 0)
        {
            firstGamePart();
        }
        else if (gamePart == 1)
        {
            destroyWithTag("Pin");
            createAllPins(8);
            global.cameras[0].enabled = true;
            global.cameras[1].enabled = false;
            GameObject[] pins = GameObject.FindGameObjectsWithTag("Pin");
            GameObject[] miniPins =
                GameObject.FindGameObjectsWithTag("MiniPin");
            foreach (GameObject miniPin in miniPins)
            {
                destroyDrag (miniPin);
            }
        }
    }

    private void nextGamePartAnimation()
    {
    }

    private void createBoxColider(
        GameObject objectForColiders,
        Vector3 center,
        Vector3 size
    )
    {
        // polymorhping just creating box colider with center and size
        BoxCollider _bc =
            (BoxCollider) objectForColiders.AddComponent(typeof (BoxCollider));
        _bc.center = center;
        _bc.size = size;
    }

    private void createBoxColider(GameObject objectForColiders)
    {
        // polymorhping just creating box colider
        BoxCollider _bc =
            (BoxCollider) objectForColiders.AddComponent(typeof (BoxCollider));
        _bc.center = Vector3.zero;
        _bc.size = Vector3.one;
    }

    private GameObject
    createPinCover(
        Mesh objectToCreate,
        Material materialToCreate,
        int Column,
        int Row
    )
    {
        // to not let the player see what is his goal to guess
        var gameObject = new GameObject("Cover" + Column + Row);
        gameObject.tag = "PinCover";
        var meshFilter =
            gameObject.AddComponent<MeshFilter>().sharedMesh = objectToCreate;
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<Renderer>().material = materialToCreate;
        if (Row == maxRow + 1)
        {
            Row++;
        }
        gameObject.transform.position =
            new Vector3(gameObject.transform.localScale.x * 2 * (Column - 1) +
                0.3f,
                0.0f,
                gameObject.transform.localScale.z * 2 * (Row - 0.5f) - 1);
        gameObject.transform.rotation = Quaternion.Euler(0.0f, 180, 0.0f);
        return gameObject;
    }

    private GameObject
    createAntiPin(
        Mesh objectToCreate,
        Material materialToCreate,
        int Column,
        int Row
    )
    {
        // ant pin for holding pins
        var gameObject = new GameObject("AntiPin" + Column + Row);
        createBoxColider(gameObject,
        new Vector3(-0.775f, 0, 0),
        new Vector3(0.45f, 2, 2));
        createBoxColider(gameObject,
        new Vector3(0.2285f, 0, 0.781f),
        new Vector3(1.53f, 2, 0.438f));
        createBoxColider(gameObject,
        new Vector3(0.779f, 0, -0.23f),
        new Vector3(0.42f, 2, 1.56f));
        createBoxColider(gameObject,
        new Vector3(0, 0, -0.777f),
        new Vector3(1.1f, 2, 0.45f));
        var meshFilter =
            gameObject.AddComponent<MeshFilter>().sharedMesh = objectToCreate;
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<Renderer>().material = materialToCreate;
        if (Row == maxRow + 1)
        {
            Row++;
        }
        gameObject.transform.position =
            new Vector3(gameObject.transform.localScale.x * 2 * (Column - 1),
                0.0f,
                gameObject.transform.localScale.z * 2 * (Row - 1));
        return gameObject;
    }
    public void DestroyLastReset(int row){
        destroyWithName("ResetRowButton" + row);
    }
    private void ResetRow(int row){
        int[] colors;
        colors = GetFreezedPinsColors("PinToFreeze");
        recreatePins("Pin", colors);
        destroyWithTag("PinToFreeze");
        destroyWithTag("AntiPinBox");
        destroyWithName("ResetRowButton"+row);
        for (int i = 1; i <= maxColumn; i++)
        {
            destroyWithName("AntiPin"+i+row);
            colorTable.SetElement(row, i, colorTable.GetBlank());
        }
        Debug.Log("ResetRow" + row);
        createRowAntiPins(row);

    }
    public void resetRowButton(){
        bool toReset = false;
        for (int i = 1; i <= maxColumn; i++)
        {
            if(colorTable.GetElement(currentRow,i)!=colorTable.GetBlank()){
                ResetRow(currentRow);
                toReset = true;
                break;
            }
        }
        if(!toReset){
            Debug.Log("LOG");
            generateRandomPinColorRow(currentRow);
        }
        
    }
    private GameObject createResetBox (Material materialToCreate,
        int Row){
        var gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObject.name = "ResetRowButton" + Row;
        gameObject.AddComponent<ResetRow>();
       
        
        gameObject.GetComponent<Renderer>().material = materialToCreate;
        gameObject.transform.position =
            new Vector3(gameObject.transform.localScale.x * -8,
                0.0f,
                gameObject.transform.localScale.z * 2 * (Row - 1));
            
        
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        return gameObject;
        
        }
    private GameObject
    createMiniAntiPin(
        Mesh objectToCreate,
        Material materialToCreate,
        int Column,
        int Row
    )
    {
        // small anti pin for small pins
        var gameObject = new GameObject("MiniAntiPin" + Row + Column);
        createBoxColider(gameObject,
        new Vector3(-0.775f, 0, 0),
        new Vector3(0.45f, 2, 2));
        createBoxColider(gameObject,
        new Vector3(0.2285f, 0, 0.781f),
        new Vector3(1.53f, 2, 0.438f));
        createBoxColider(gameObject,
        new Vector3(0.779f, 0, -0.23f),
        new Vector3(0.42f, 2, 1.56f));
        createBoxColider(gameObject,
        new Vector3(0, 0, -0.777f),
        new Vector3(1.1f, 2, 0.45f));
        var meshFilter =
            gameObject.AddComponent<MeshFilter>().sharedMesh = objectToCreate;
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<Renderer>().material = materialToCreate;
        gameObject.transform.position =
            new Vector3(gameObject.transform.localScale.x * -1 * (Column + 1),
                0.0f,
                gameObject.transform.localScale.z * 2 * (Row - 1));
        gameObject.transform.localScale = new Vector3(0.5f, 0.8f, 0.5f);
        return gameObject;
    }

    private GameObject createMiniAntiPinBox(int Column, int Row)
    {
        // this small box inside every small antipin is used for chcecking collisions
        var gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

        gameObject.name = "" + Row;
        if (Row < 10)
        {
            gameObject.name = "0" + gameObject.name;
        }
        if (Column < 10)
        {
            gameObject.name = gameObject.name + "0"; // to make the name same lenght
        }
        gameObject.name = gameObject.name + Column;
        gameObject.tag = "MiniAntiPinBox";
        gameObject
            .GetComponent<Renderer>()
            .material
            .SetColor("_Color", Color.black);
        gameObject.transform.position =
            new Vector3(gameObject.transform.localScale.x * -1 * (Column + 1),
                -0.0f,
                gameObject.transform.localScale.z * 2 * (Row - 1));
        gameObject.transform.localScale = new Vector3(0.5f, 0.8f, 0.5f);
        createBoxColider(gameObject,
        new Vector3(0, 0, 0),
        new Vector3(1, 2, 1));
        gameObject.AddComponent<Collisons>();
        return gameObject;
    }

    public void createQuitButton()
    {
        // "button" that is used to get to main menu ** in future it will have some visualisation that helps to new player understand
        var gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObject.transform.SetParent(gameField.transform);
        gameObject.name = "QuitButton";
        gameObject.AddComponent<Quit>();

        gameObject.GetComponent<Renderer>().material = materialQuit;
        gameObject.transform.position = new Vector3(35, 0, 10);
        gameObject.transform.localScale = new Vector3(3, 1, 3);
    }

    public void createSetColorsButton()
    {
        // "button" that is used to set your guess ** in future it will have some visualisation that helps to new player understand
        var gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObject.transform.SetParent(gameField.transform);
        gameObject.name = "SetColorsButton";
        gameObject.AddComponent<SetColors>();
        gameObject.GetComponent<Renderer>().material = materialSetColors;
        gameObject.transform.position = new Vector3(4, 0, -2);
        gameObject.transform.localScale = new Vector3(10, 2, 1);
    }

    private GameObject createAntiPinBox(int Column, int Row)
    {
        // small box that is inside antipin, is for collision detection
        var gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObject.name = "" + Row;
        if (Row < 10)
        {
            gameObject.name = "0" + gameObject.name;
        }
        if (Column < 10)
        {
            gameObject.name = gameObject.name + "0";
        }
        gameObject.name = gameObject.name + Column;
        gameObject.tag = "AntiPinBox" ;
        gameObject
            .GetComponent<Renderer>()
            .material
            .SetColor("_Color", Color.black);
        createBoxColider(gameObject,
        new Vector3(0, 0, 0),
        new Vector3(1, 2, 1));
        gameObject.AddComponent<Collisons>();
        if (Row == maxRow + 1)
        {
            Row++;
        }
        gameObject.transform.position =
            new Vector3(gameObject.transform.localScale.x * 2 * (Column - 1),
                -0.0f,
                gameObject.transform.localScale.z * 2 * (Row - 1));
        return gameObject;
    }

    private void SetChild(GameObject parent, GameObject child)
    {
        child.transform.SetParent(parent.transform);
    }

    private void createAntiPins()
    {
        // one of starter functions all we need at start is first and last antipins
        GameObject antiPins = new GameObject("antiPins");
        GameObject miniAntiPins = new GameObject("miniAntiPins");

        AntiPins.transform.SetParent(gameField.transform);
        antiPins.transform.SetParent(AntiPins.transform);
        miniAntiPins.transform.SetParent(AntiPins.transform);
        createRowAntiPins(1);
        createLastAntiPins (antiPins);
    }
  
    private void createRowAntiPins(int iRow)
    {
        // row of antipins are created here with small boxes inside
        GameObject row = new GameObject("Row: " + iRow);
        GameObject miniRow = new GameObject("Row: " + iRow);
        GameObject antiPin;
        GameObject box;
        GameObject resetRowButton = new GameObject("ResetButton: " + iRow);;
        
        row.transform.SetParent(AntiPins.transform.GetChild(0).transform);
        miniRow.transform.SetParent(AntiPins.transform.GetChild(1).transform);
        for (
            int numberAPColumn = 1;
            numberAPColumn <= maxColumn;
            numberAPColumn++
        )
        {
            antiPin =
                createAntiPin(objectAntiPin,
                materialDefault,
                numberAPColumn,
                iRow);
            antiPin.transform.SetParent(row.transform);
            box = createAntiPinBox(numberAPColumn, iRow);
            box.transform.SetParent(antiPin.transform);

            antiPin =
                createMiniAntiPin(objectAntiPin,
                materialDefault,
                numberAPColumn,
                iRow);
            antiPin.transform.SetParent(miniRow.transform);
            box = createMiniAntiPinBox(numberAPColumn, iRow);
            box.transform.SetParent(antiPin.transform);
            
        }
        Debug.Log("createRowAntiPins" + iRow);
        resetRowButton.transform.SetParent(AntiPins.transform.GetChild(0).transform);
        resetRowButton = createResetBox(materialReset,iRow);
    }

    private void createLastAntiPins(GameObject antiPins)
    {
        // last antipins holding the goal pins
        GameObject lastRow = new GameObject("Row: " + (maxRow + 1));
        GameObject antiPin;
        GameObject box;
        lastRow.transform.SetParent(antiPins.transform);
        for (
            int numberAPColumn = 1;
            numberAPColumn <= maxColumn;
            numberAPColumn++
        )
        {
            antiPin =
                createAntiPin(objectAntiPin,
                materialDefault,
                numberAPColumn,
                maxRow + 1);
            antiPin.transform.SetParent(lastRow.transform);
            box = createAntiPinBox(numberAPColumn, maxRow + 1);
            box.transform.SetParent(antiPin.transform);
        }
    }

    private void createWholeBox()
    {
        // creates box for all the pins
        GameObject WholeBox = new GameObject("BoxForPins");
        WholeBox.transform.SetParent(gameField.transform);
        WholeBox.AddComponent<MeshRenderer>();
        Rigidbody gameObjectsRigidBody = WholeBox.AddComponent<Rigidbody>();
        var meshFilter =
            WholeBox.AddComponent<MeshFilter>().sharedMesh = objectWholeBox;
        WholeBox.GetComponent<Renderer>().material = materialDefault;
        WholeBox.transform.localScale = new Vector3(1f, 1f, 1.75f);
        WholeBox.transform.position =
            new Vector3((
                WholeBox.transform.localScale.z * ((maxColumn + 0.5f)) * 3
                ),
                0.0f,
                0.0f);
        WholeBox.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        gameObjectsRigidBody.constraints = RigidbodyConstraints.FreezeAll;
        gameObjectsRigidBody.isKinematic = true;
        WholeBox.AddComponent<MeshCollider>();
    }

    public void createPin(
        Mesh Pin,
        Material materialToCreate,
        int color,
        string tag
    )
    {
        //creating single pin with color
        var gameObject = new GameObject("");
        gameObject.tag = tag;
        gameObject.AddComponent<MeshRenderer>();
        

        if (tag == "MiniPin")
        {
            gameObject.GetComponent<Renderer>().material = materialReset;
            gameObject.transform.localScale = new Vector3(0.5f, 0.8f, 0.5f);
            gameObject
                .transform
                .SetParent(AllPins
                    .transform
                    .GetChild(2)
                    .transform
                    .GetChild(color - (colors.Length - 2)));
        }
        else
        {
            gameObject.GetComponent<Renderer>().material = materialToCreate;
            gameObject
                .transform
                .SetParent(AllPins
                    .transform
                    .GetChild(0)
                    .transform
                    .GetChild(color));
            AddDrag (gameObject);
        }
        createBoxColider(gameObject,
        new Vector3(0, 0.25f, 0),
        new Vector3(2, 1.31f, 2));
        createBoxColider(gameObject,
        new Vector3(0, 1.6f, 0),
        new Vector3(1.0f, 1.0f, 1.0f));
        var meshFilter = gameObject.AddComponent<MeshFilter>().sharedMesh = Pin;

        Rigidbody gameObjectsRigidBody = gameObject.AddComponent<Rigidbody>(); // Add the rigidbody.
        var newColor = gameObject.GetComponent<Renderer>().material;
        switch (color // manually setting name depending on color
        )
        {
            case 0:
                newColor.SetColor("_Color", Color.red);
                gameObject.name = gameObject.name + "Red";
                break;
            case 1:
                newColor.SetColor("_Color", Color.blue);
                gameObject.name = gameObject.name + "Blue";
                break;
            case 2:
                newColor.SetColor("_Color", Color.cyan);
                gameObject.name = gameObject.name + "Cyan";
                break;
            case 3:
                newColor.SetColor("_Color", Color.magenta);
                gameObject.name = gameObject.name + "Magenta";
                break;
            case 4:
                newColor.SetColor("_Color", Color.green);
                gameObject.name = gameObject.name + "Green";
                break;
            case 5:
                newColor.SetColor("_Color", Color.yellow);
                gameObject.name = gameObject.name + "Yellow";
                break;
            case 6:
                newColor.SetColor("_Color", Color.white);
                gameObject.name = gameObject.name + "White";
                break;
            case 7:
                newColor.SetColor("_Color", Color.black);
                gameObject.name = gameObject.name + "Black";
                break;
        }

        // putting pin somwhere above the pinbox
        gameObject.transform.position =
            new Vector3(Random
                    .Range(3 + ((maxColumn + 0) * 2),
                    -2 + ((Mathf.RoundToInt(maxColumn * 1.5f) + 1) * 3)),
                Random.Range(1, 40),
                Random.Range(1, (maxRow * 2) - 3));
    }

    private void createAllPins(int nColors)
    {
        for (int i = 0; i < maxColumn; i++)
        {
            // for each column there is one pin of every color
            for (int color = 0; color < nColors; color++)
            {
                createPin(objectPin, materialDefault, color, "Pin");
                if (color == 6 || color == 7)
                {
                    // 6 and 7 are also black and white minipins
                    createPin(objectPin, materialDefault, color, "MiniPin");
                }
            }
        }
    }

    private void createGameField()
    {
        createWholeBox();
        createAntiPins();
        createAllPins(8);
        createQuitButton();
        createSetColorsButton();
    }

    public class ColorTable
    {
        private int i;

        private GameField lodzik;

        private string[] clrTable;

        public string GetBlank()
        {
            return "blank";
        }

        public void ResetTable()
        {
            // reseting table to blank values
            string[] tmpTable = new string[55];
            lodzik = GameObject.Find("GameObject").GetComponent<GameField>();
            for (i = 0; i < tmpTable.Length; i++)
            {
                tmpTable[i] = GetBlank();
            }
            clrTable = tmpTable;
        }

        public string GetElement(int row, int column)
        {
            //getter from table
            row--;
            column--;
            if (
                row * lodzik.maxColumn + column < clrTable.Length &&
                row * lodzik.maxColumn + column >= 0
            )
            {
                //element is inside the table
                return clrTable[row * lodzik.maxColumn + column]; //return color
            }
            else
            {
                return GetBlank(); //return blank here will be some error managment in the future
            }
        }

        public void SetElement(int row, int column, string NewValue)
        {
            // when pin is puttet into spot this function is called
            row--;
            column--;
            if (
                row * lodzik.maxColumn + column < clrTable.Length &&
                row * lodzik.maxColumn + column >= 0
            )
            {
                // checks if the pin is inside the table
                clrTable[row * lodzik.maxColumn + column] = NewValue; // set the color to color of pin
            }
            else
            {
                clrTable[row * lodzik.maxColumn + column] = GetBlank(); //here will be some error managment in the future
            }
        }

        public void eraseElement(int row, int column)
        {
            row--;
            column--;
            if (
                row * lodzik.maxColumn + column < clrTable.Length &&
                row * lodzik.maxColumn + column >= 0
            )
            {
                // checks if it is inside table
                clrTable[row * lodzik.maxColumn + column] = GetBlank(); //when pin is removed his virtualization in table is also removed by this statement
            }
        }

        public bool isRowFull(int row)
        {
            // if current row is full return true
            row--;
            for (
                i = row * lodzik.maxColumn;
                i < row * lodzik.maxColumn + lodzik.maxColumn;
                i++
            )
            {
                if (clrTable[i] == GetBlank())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
