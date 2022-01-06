using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [HideInInspector] public float multiplier = 1;

    [HideInInspector] public int points = 0;

    [HideInInspector] public bool canRestart, gameEnded, gameStarted;

    [SerializeField] ChunkGeneration chunkGen;

    [SerializeField] CloudGeneration cloudGen;

    [SerializeField] float clearTimer, degreeIncrement, difficultyIncrement, difficultyTimer, maxDifficulty;

    [SerializeField] PlayerAnimations playerAnim;

    [SerializeField] PlayerControls playerCont;

    [SerializeField] Text finalScore, score, start, title;

    private Color finalScoreColor, scoreColor, startColor, titleColor;

    private float timer;

    private void Start()
    {
        timer = clearTimer;
        finalScoreColor = finalScore.color;
        scoreColor = score.color;
        startColor = start.color;
        titleColor = title.color;
    }

    private void Update()
    {
        if (!gameStarted && Input.anyKey)
        {
            gameStarted = true;
            Invoke("UpdateDifficulty", difficultyTimer);
            ClearText();
        }

        if (canRestart && Input.anyKey)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void Defeat()
    {
        gameEnded = true;
        finalScore.text = "Final score: " + points;
        start.text = "Press any key to restart";
        title.text = "You lose";
        chunkGen.enabled = false;
        cloudGen.enabled = false;
        playerCont.enabled = false;
        timer = clearTimer;
        ShowText();
    }

    public void UpdateDifficulty()
    {
        if (!gameEnded)
        {
            multiplier = Mathf.Clamp(multiplier - difficultyIncrement, maxDifficulty, 1);
            playerAnim.originalMaxRotation = Mathf.Clamp(playerAnim.originalMaxRotation + degreeIncrement, playerAnim.maxRotation, 45);
            Invoke("UpdateDifficulty", difficultyTimer);
        }
    }

    public void UpdateScore()
    {
        points++;
        score.text = "Score: " + points;
    }

    private void ClearText()
    {
        timer = Mathf.Clamp(timer - Time.deltaTime, 0, clearTimer);
        startColor.a = titleColor.a = timer / clearTimer;
        scoreColor.a = 1 - (timer / clearTimer);
        score.color = scoreColor;
        start.color = startColor;
        title.color = titleColor;

        if (timer > 0)
        {
            Invoke("ClearText", Time.deltaTime);
        }
    }

    private void ShowText()
    {
        timer = Mathf.Clamp(timer - Time.deltaTime, 0, clearTimer);
        finalScoreColor.a = startColor.a = titleColor.a =  1 - (timer / clearTimer);
        scoreColor.a = timer / clearTimer;
        finalScore.color = finalScoreColor;
        score.color = scoreColor;
        start.color = startColor;
        title.color = titleColor;

        if (timer > 0)
        {
            Invoke("ShowText", Time.deltaTime);
        }

        else
        {
            canRestart = true;
        }
    }
}