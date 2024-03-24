using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player playerOne;
    [SerializeField] private Player playerTwo;
    [SerializeField] private GameObject healthBarOne;
    [SerializeField] private GameObject healthBarTwo;
    [SerializeField] private TextMeshProUGUI timerText, roundStartText, roundEndText;
    [SerializeField] private GameObject levelEndPanel;
    [SerializeField] private int timer = 10;
    [SerializeField] private Image playerOneWinEmptyImage1;
    [SerializeField] private Image playerOneWinEmptyImage2;
    [SerializeField] private Image playerOneWinEmptyImage3;
    [SerializeField] private Image playerTwoWinEmptyImage1;
    [SerializeField] private Image playerTwoWinEmptyImage2;
    [SerializeField] private Image playerTwoWinEmptyImage3;
    [SerializeField] private Sprite winFillImage1;
    [SerializeField] private Sprite winFillImage2;
    [SerializeField] private Sprite winFillImage3;
    private Slider sliderOne;
    private Slider sliderTwo;
    public int playerOneWins;
    public int playerTwoWins;
    public int roundNo = 1;


    private void Start()
    {
        sliderOne = healthBarOne.GetComponent<Slider>();
        sliderTwo = healthBarTwo.GetComponent<Slider>();
        UpdateHealthBars();
        roundNo = PlayerPrefs.GetInt("RoundNumber", 1);
        playerOneWins = PlayerPrefs.GetInt("PlayerOneWins", 0);
        playerTwoWins = PlayerPrefs.GetInt("PlayerTwoWins", 0);
        UpdateWinsBars();
        DisablePlayers();
        levelEndPanel.SetActive(false);
        timerText.text = timer.ToString();
        StartCoroutine(RoundStart());
    }


    IEnumerator RoundStart()
    {
        roundStartText.text = "Round " + roundNo.ToString();
        roundStartText.gameObject.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 1);
        yield return new WaitForSeconds(1);
        roundStartText.gameObject.transform.localScale = Vector3.one;
        roundStartText.gameObject.transform.DOScale(Vector3.one * 1.2f, 0.5f);
        roundStartText.text = "3";
        yield return new WaitForSeconds(0.5f);
        roundStartText.gameObject.transform.localScale = Vector3.one;
        roundStartText.gameObject.transform.DOScale(Vector3.one * 1.2f, 0.5f);
        roundStartText.text = "2";
        yield return new WaitForSeconds(0.5f);
        roundStartText.gameObject.transform.localScale = Vector3.one;
        roundStartText.gameObject.transform.DOScale(Vector3.one * 1.2f, 0.5f);
        roundStartText.text = "1";
        yield return new WaitForSeconds(0.5f);
        roundStartText.gameObject.transform.localScale = Vector3.one;
        roundStartText.gameObject.transform.DOScale(Vector3.one * 1.2f, 1.0f);
        roundStartText.text = "Fight";
        yield return new WaitForSeconds(1.0f);
        roundStartText.text = "";
        StartCoroutine(Timer());
    }


    IEnumerator Timer()
    {
        EnablePlayers();
        yield return new WaitForSeconds(1);
        timer--;
        timerText.text = timer.ToString();
        if (playerOne.health <= 0 || playerTwo.health <=0)
        {
            yield return new WaitForSeconds(1);
            DisablePlayers();
            StartCoroutine(RoundWinnerCheck());
        }
        else if (timer == 0)
        {
            DisablePlayers();
            StartCoroutine(RoundWinnerCheck());
        }
        else if (timer < 4)
        {
            timerText.color = Color.red;
            StartCoroutine(Timer());
        }
        else
        {
            StartCoroutine(Timer());
        }
    }


    IEnumerator RoundWinnerCheck()
    {
        yield return new WaitForSeconds(1);
        if (playerOne.health > playerTwo.health)
        {
            playerOneWins++;
            UpdateWinsBars();
            PlayerPrefs.SetInt("PlayerOneWins", playerOneWins);
            yield return new WaitForSeconds(1);
            levelEndPanel.SetActive(true);
            roundEndText.text = "Player One Wins!";
        }
        else if (playerOne.health == playerTwo.health)
        {
            roundEndText.text = "Draw";
        }
        else if (playerOne.health < playerTwo.health)
        {
            playerTwoWins++;
            UpdateWinsBars();
            PlayerPrefs.SetInt("PlayerTwoWins", playerTwoWins);
            yield return new WaitForSeconds(1);
            levelEndPanel.SetActive(true);
            roundEndText.text = "Player Two Wins!";

        }
        yield return new WaitForSeconds(2);
        if (roundNo == 3)
        {
            roundEndText.text = "GAME OVER";

            yield return new WaitForSeconds(1);
            PlayerPrefs.SetInt("RoundNumber", 1);
            PlayerPrefs.SetInt("PlayerOneWins", 0);
            PlayerPrefs.SetInt("PlayerTwoWins", 0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (roundNo == 1 || roundNo == 2) 
        {
            roundNo++;
            PlayerPrefs.SetInt("RoundNumber", roundNo);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    private void UpdateWinsBars()
    {
        if (playerOneWins == 1)
        {
            playerOneWinEmptyImage1.sprite = winFillImage1;
        }
        else if (playerOneWins == 2)
        {
            playerOneWinEmptyImage1.sprite = winFillImage1;
            playerOneWinEmptyImage2.sprite = winFillImage2;
        }
        else if (playerOneWins == 3)
        {
            playerOneWinEmptyImage1.sprite = winFillImage1;
            playerOneWinEmptyImage2.sprite = winFillImage2;
            playerOneWinEmptyImage3.sprite = winFillImage3;
        }
        if (playerTwoWins == 1)
        {
            playerTwoWinEmptyImage3.sprite = winFillImage3;
        }
        else if (playerTwoWins == 2)
        {
            playerTwoWinEmptyImage3.sprite = winFillImage3;
            playerTwoWinEmptyImage2.sprite = winFillImage2;
        }
        else if (playerTwoWins == 3)
        {
            playerTwoWinEmptyImage3.sprite = winFillImage3;
            playerTwoWinEmptyImage2.sprite = winFillImage2;
            playerTwoWinEmptyImage1.sprite = winFillImage1;
        }
    }

    
    private void Update()
    {
        UpdateHealthBars();
    }


    private void UpdateHealthBars()
    {
        sliderOne.value = playerOne.health / 100.0f;
        sliderTwo.value = playerTwo.health / 100.0f;
    }


    private void EnablePlayers()
    {
        playerOne.gameObject.GetComponentInChildren<Animator>().enabled = true;
        playerTwo.gameObject.GetComponentInChildren<Animator>().enabled = true;
        playerTwo.enabled = true;
        playerOne.enabled = true;
    }


    private void DisablePlayers()
    {
        playerOne.gameObject.GetComponentInChildren<Animator>().enabled = false;
        playerTwo.gameObject.GetComponentInChildren<Animator>().enabled = false;
        playerTwo.enabled = false;
        playerOne.enabled = false;
    }
}
