using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    const float width = 3.7f;
    const float height = 7f;

    public float snakeSpeed = 1;

    public BodyPart bodyPrefab = null;

    public GameObject rockPrefab = null;
    public GameObject eggPrefab = null;
    public GameObject goldeggPrefab = null;

    public Sprite tailSprite = null;

    public Sprite bodySprite = null;

    public SnakeHead snakeHead = null;

    public bool alive = true;

    public bool WaitingToPlay = true;

    List<Egg> eggs = new List<Egg>();

    int level = 0;
    int numberOfEggForNextLevel = 0;

    public int score = 0;
    public int hiScore = 0;

    public Text scoreText = null;
    public Text hiScoreText = null;
    public Text gameOverText = null;
    public Text tapToPlayText = null;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Debug.Log("Starting Snake Game");
        CreateWalls();
        alive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(WaitingToPlay)
        {
            foreach(Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Ended)
                {
                    StartGameplay();
                }
            }
            if (Input.GetMouseButtonUp(0))
                StartGameplay();
        }
    }

   
    public void GameOver()
    {
        alive = false;
        WaitingToPlay = true;

        gameOverText.gameObject.SetActive(true);
        tapToPlayText.gameObject.SetActive(true);
    }

    void StartGameplay()
    {

        score = 0;
        level = 0;

        scoreText.text = "Score = " + score;
        hiScoreText.text = "HiScore =" + hiScore;

        tapToPlayText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);

        WaitingToPlay = false;
        alive = true;

        KillOldEggs();
        

        LevelUp();
    }

    void LevelUp()
    {
        level++;

        snakeSpeed = 1f + (level / 4f);
        if (snakeSpeed > 6) snakeSpeed = 6;

        numberOfEggForNextLevel = 4 + (level * 2);

        snakeHead.ResetSnake();
        CreateEgg();


    }

    public void EggEaten(Egg egg)
    {

        score++;

        numberOfEggForNextLevel--;
        if(numberOfEggForNextLevel==0)
        {
            score += 10;
            LevelUp();
        }


        else if (numberOfEggForNextLevel == 1)
            CreateEgg(true);
        else
            CreateEgg(false);


        if (score > hiScore)
        {
            hiScore = score;

            hiScoreText.text = "HiScore =" + hiScore;
        }

        scoreText.text = "Score = " + score;

        eggs.Remove(egg);
        Destroy(egg.gameObject);
    }

    void CreateWalls()
    {
        float z = -1f;
        Vector3 start = new Vector3(-width, -height, z);
        Vector3 finish = new Vector3(-width, +height, z);

        CreateWall(start, finish);

         start = new Vector3(+width, -height, z);
         finish = new Vector3(+width, +height, z);

        CreateWall(start, finish);

         start = new Vector3(-width, -height, z);
         finish = new Vector3(+width, -height, z);

        CreateWall(start, finish);

         start = new Vector3(-width, +height, z);
         finish = new Vector3(+width, +height, z);

        CreateWall(start, finish);
    }

    void CreateWall(Vector3 start, Vector3 finish)
    {
        float distance = Vector3.Distance(start, finish);
        int numberOfRocks = (int)(distance * 3f);
        Vector3 delta = (finish - start) / numberOfRocks;

        Vector3 position = start;
        for(int rock = 0;  rock<=numberOfRocks; rock++)
        {
            float rotation = Random.Range(0, 360f);
            float scale = Random.Range(1.5f, 2f);
            CreateRock(position, scale, rotation);
            position = position + delta;
        }
    }

    void CreateRock(Vector3 position, float scale, float rotation)
    {
        GameObject rock = Instantiate(rockPrefab, position, Quaternion.Euler(0, 0, rotation));
        rock.transform.localScale = new Vector3(scale, scale, 1);
        
    }

    void CreateEgg(bool golden = false)
    {
        Vector3 position;
        position.x = -width + Random.Range(1f, (width*2)-2f);
        position.y = -height + Random.Range(1f, (height * 2) - 2f);
        position.z = -1;
        Egg egg = null;
        if (golden)
           egg = Instantiate(goldeggPrefab, position, Quaternion.identity).GetComponent<Egg>();
        else
        egg = Instantiate(eggPrefab, position, Quaternion.identity).GetComponent<Egg>();

        eggs.Add(egg);
    }


    void KillOldEggs()
    {
        foreach(Egg egg in eggs)
        {
            Destroy(egg.gameObject);
        }
        eggs.Clear();
    }

}
