using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Spawner SpawnerPipes;
    public GameObject RainEffect;
    public GameObject ThunderEffect;
    public Bird bird;

    public Text ScoreText;
    public Text HighscoreText;
    public Text FPSCounterText;

    public GameObject PlayButton;
    public GameObject QuitButton;
    public GameObject GameOverImage;
    public GameObject FlappyBirdImage;

    public AudioSource ThunderSound;

    public static bool isPlay = true;

    private int _score;
    private int _highScore;
    private float _fps;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        GameOverImage.SetActive(false);
    }

    private void Start()
    {
        _highScore = PlayerPrefs.GetInt("highscore");
        HighscoreText.text = _highScore.ToString();

        InvokeRepeating("GetFPS", 1, 1);

        Instantiate(RainEffect, new Vector3(2, 6, 0), Quaternion.Euler(0, 0, -20));
        StartCoroutine(WaitForThunder());
    }

    IEnumerator WaitForThunder()
    {
        yield return new WaitForSeconds(5);
        Instantiate(ThunderEffect, Vector3.zero, Quaternion.Euler(0, 0, 0));

        InvokeRepeating("ThunderSoundPlay", 0, 5);
    }

    public void ThunderSoundPlay()
    {
        ThunderSound.Play();
    }

    public void Play()
    {
        SpawnerPipes.gameObject.SetActive(true);

        isPlay = true;
        Bird.isAlive = true;
        Bird.isExplosion = false;

        _score = 0;
        ScoreText.text = _score.ToString();

        GameOverImage.SetActive(false);
        PlayButton.SetActive(false);
        QuitButton.SetActive(false);
        FlappyBirdImage.SetActive(false);

        bird.Rigidbody.gravityScale = 1.8f;
        bird.enabled = true;
        bird.transform.position = new Vector3(-0.4f, 0, -1);
        bird.Animation.timeScale = 3;

        var pipes = FindObjectsOfType<Pipes>();

        foreach (var pipe in pipes)
        {
            pipe._managePool.Release(pipe);
        }
    }

    public void GameOver()
    {
        GameOverImage.SetActive(true);
        PlayButton.SetActive(true);
        QuitButton.SetActive(true);
        SpawnerPipes.gameObject.SetActive(false);

        bird.enabled = false;
    }

    public void IncreaseScore()
    {
        _score++;
        ScoreText.text = _score.ToString();

        UpdateHighScore();
    }

    private void UpdateHighScore()
    {
        if (_score > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", _score);

            _highScore = PlayerPrefs.GetInt("highscore");
            HighscoreText.text = _highScore.ToString();
        }
    }

    void GetFPS()
    {
        _fps = (int)(1f / Time.unscaledDeltaTime);
        string fpscounter = "FPS: " + _fps.ToString();
        FPSCounterText.text = fpscounter;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
