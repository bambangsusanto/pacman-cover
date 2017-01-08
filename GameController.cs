using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public Material superDotMaterial;
    public float duration;

    Ghost[] ghosts;

    public GameObject bonusObject;
    public Text scoreText;
    public Text bonusGetText;

    int dotCount;
    int score;

    private void Start()
    {
        ghosts = FindObjectsOfType<Ghost>();
    }

    private void Update()
    {
        // Make the super dot material blinks
        float lerp = Mathf.PingPong(Time.time, duration) / duration;
        superDotMaterial.color = Color.Lerp(Color.white, Color.clear, lerp);
    }

    public void AddDotCount()
    {
        dotCount++;
        AudioController.instance.PlaySFX("pacman_chomp");

        if (dotCount == 40 || dotCount == 70)
            ActivateBonus();

        if (dotCount >= 151)
            WinTheGame();
    }

    public void AddScore()
    {
        score += 10;
        UpdateScoreText();
    }

    public void AddScoreSuper()
    {
        score += 50;
        UpdateScoreText();
    }

    public void AddScoreBonus()
    {
        AudioController.instance.PlaySFX("pacman_eatfruit");

        bonusGetText.gameObject.SetActive(true);
        Invoke("DisableBonusText", 0.4f);

        for (int i = 0; i < ghosts.Length; i++)
            ghosts[i].SetDisableAfterEatingFruit();

        score += 200;
        UpdateScoreText();
    }

    private void DisableBonusText()
    {
        bonusGetText.gameObject.SetActive(false);
    }

    private void ActivateBonus()
    {
        Instantiate(bonusObject, new Vector3(8.5f, 0.5f, 4.5f), Quaternion.Euler(0f, 0f, 90f));
        Bonus bonusObjectToBeDestroyed = FindObjectOfType<Bonus>();
        Destroy(bonusObjectToBeDestroyed.gameObject, 8f);
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString("D5");
    }

    private void WinTheGame()
    {
        print("You Win!");
    }

}
