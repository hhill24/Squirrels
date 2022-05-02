using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;



public class AcornPuzzle : MonoBehaviour
{
    
    [Header("DRAG & DROP")]
	[Space]
    [SerializeField] public GameObject puzzle;
    // text for guesses remaining
    public Text textGuesses;
    // text label for acorns
    public Text textAcornsFound;
    // [SerializeField] public GameObject enemy;
    // panel to change colour depending on outcome
    public GameObject panel;
    // describes outcome of puzzle
    public Text outcomeText;
    // describes effect of outcome on gameplay
    public Text descriptionText;
    // Audio clip for success
    [SerializeField] public AudioClip successEffect;
    private AudioSource soundSource;
    

    [Header("PUZZLE VARIABLES")]
	[Space]
    public int guessesRemaining = 5;
    public int acornsFound = 0;
    public bool won;
    public Image[] images; 
    public Tile[,] grid = new Tile[4,4];

    [Header("EVENTS")]
	[Space]
    // called when puzzle finisges
    public UnityEvent OnFinishEvent;
    
    void Start(){
        soundSource = GetComponent<AudioSource>();
        soundSource.clip = successEffect;
    }
    
    // starts puzzle
    public void StartPuzzle(){
        //reset puzzle variables
        

        guessesRemaining = 5;
        acornsFound = 0;
        textGuesses.text = "Remaining guesses: " + guessesRemaining.ToString();


        for (int x=0; x<images.Length; x++){
            images[x].enabled = true;
        }
        
        // clears any previous tiles from grid
        for (int x=0; x<4; x++){
            for (int y =0; y<4; y++){
               if (grid[x,y] != null){
                   Tile tile = grid[x,y];
                    Destroy(tile.gameObject);
                    grid[x,y]=null;
               }   
            }
        }
        // place acorns randomly
        placeAcorns();
        // fill in blanks for the rest of grid
        placeGrid();
    }

    // Update is called once per frame
    void Update()
    {
        if (guessesRemaining > 0 && acornsFound < 3){
            //check user input while puzzle is not completed
            CheckInput();
        }else{
            //puzzle completed scenarios
            if (guessesRemaining == 0 && acornsFound == 3 && !won){
                if (!soundSource.isPlaying)
                {
                    soundSource.Play();
                }
            
            won = true;
            outcomeText.text = "Acorn Puzzle Complete";
            descriptionText.text = "Enemy will be frozen in place for 5 seconds when you return to the game ";

            } else if (guessesRemaining == 0 && acornsFound < 3){
            won = false;
            outcomeText.text = "Acorn Puzzle Failed";
            descriptionText.text = "Enemy will chase you immediately when you return to the game ";
            panel.SetActive(true);

            }else if (acornsFound == 3 && guessesRemaining >0 && !won){
                if (!soundSource.isPlaying)
                {
                    soundSource.Play();
                }
            won = true;
            outcomeText.text = "Acorn Puzzle Complete";
            descriptionText.text = "Enemy will be frozen in place for 5 seconds when you return to the game ";
            }

            OnFinishEvent.Invoke();
        }
    }

    //checks where player is clicking
    void CheckInput(){
        if (Input.GetButtonDown("Fire1")){
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(mousePosition.x);
            int y = Mathf.RoundToInt(mousePosition.y);

            if (x>-1 && x<4 && y>-1 && y<4){
                //uncover the users clicked tile    
                Tile tile = grid[x,y];
                
                tile.SetIsCovered(false);

                // if the user uncovers an acorn
                if (tile.tileKind == Tile.TileKind.Acorn){
                    images[acornsFound].enabled = false;
                    acornsFound ++;
                }

                guessesRemaining--;
                textGuesses.text = "Remaining guesses: " + guessesRemaining.ToString();
                
            }
        }
    }

    // place 3 acorns randomly in the grid
    void placeAcorns(){
        // choose random location for first acorn
        int x = UnityEngine.Random.Range(0,2);
        int y = UnityEngine.Random.Range(1,4);
        // acorns always are buried in the same triangle pattern
        int x2 = x+2;
        int y2 = y;
        // acorns always are buried in the same triangle pattern
        int x3 = x+1;
        int y3 = y-1; 

        Tile acorn1 = Instantiate(Resources.Load("Prefabs/acornTile", typeof(Tile)), new Vector3(x,y,-6), Quaternion.identity) as Tile;
        grid[x,y] = acorn1;
        acorn1.tileKind = Tile.TileKind.Acorn;
        acorn1.transform.parent = puzzle.transform;
        

        Tile acorn2 = Instantiate(Resources.Load("Prefabs/acornTile", typeof(Tile)), new Vector3(x2,y2,-6), Quaternion.identity) as Tile;
        grid[x2,y2] =acorn2;
        acorn2.tileKind = Tile.TileKind.Acorn;
        acorn2.transform.parent = puzzle.transform;
        

        Tile acorn3 = Instantiate(Resources.Load("Prefabs/acornTile", typeof(Tile)), new Vector3(x3,y3,-6), Quaternion.identity) as Tile;
        grid[x3,y3] =acorn3;
        acorn3.tileKind = Tile.TileKind.Acorn;
        acorn3.transform.parent = puzzle.transform;  
    }

    // fill the rest of the grid with blank tiles
    void placeGrid(){
        for (int x=0; x<4; x++){
            for (int y =0; y<4; y++){
                if (grid[x,y]==null){
                    Tile acorn = Instantiate(Resources.Load("Prefabs/dirtTile", typeof(Tile)), new Vector3(x,y,-6), Quaternion.identity) as Tile;
                    grid[x,y] = acorn;
                    acorn.transform.parent = puzzle.transform;
                }
            }
        }
    }
}
