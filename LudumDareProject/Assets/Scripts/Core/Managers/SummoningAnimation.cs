using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class SummoningAnimationComponent : MonoBehaviour
{

    [Header("Fade")]
    public Image fadeSprite_;
    public float fadeInDuration_;
    public float fadeDuration_;
    public float fadeOutDuration_;

    [Header("References")]
    public GameObject summoningCamera_;
    public Tilemap summoningCircle_;


    [Header("Animations")]
    public List<string> putResourceAnimations_;
    public List<string> goBackAnimations_;
    public GameObject positionOfReference_;
    public Vector3 playerInitialPosition_;




    public void StartSummoningAnimation(Vector3 initialPosition)
    {
        Debug.Log("Start Summoning Animation");
        StartCoroutine(FadeIn());
        transform.position = initialPosition;
    }

    private void EnableSummoningCamera()
    {
        GameManager.Instance.mainCamera_.SetActive(false);
        summoningCamera_.SetActive(true);
        GameManager.Instance.player_.transform.SetParent(positionOfReference_.transform);
        GameManager.Instance.player_.transform.position = playerInitialPosition_;
    }

    private IEnumerator FadeIn()
    {
        Color initialColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        Color targetColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);

        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration_)
        {
            elapsedTime += Time.deltaTime;
            fadeSprite_.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeInDuration_);
            yield return null;
        }

        yield return new WaitForSeconds(fadeInDuration_);
        EnableSummoningCamera();
        StartCoroutine(FadeOut());
    }


    private IEnumerator FadeOut()
    {
        Color initialColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        Color targetColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        float elapsedTime = 0f;

        while (elapsedTime < fadeOutDuration_)
        {
            elapsedTime += Time.deltaTime;
            fadeSprite_.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeOutDuration_);
            yield return null;
        }

        // Start pentagram logic
        StartCoroutine(ActivateCircle());
    }

    private IEnumerator ActivateCircle()
    {
        Color initialColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        Color targetColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        float elapsedTime = 0f;

        while (elapsedTime < fadeOutDuration_)
        {
            elapsedTime += Time.deltaTime;
            summoningCircle_.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeOutDuration_);
            yield return null;
        }

        GameManager.Instance.SetInputMode(EInputMode.Summoning);
    }

    public void AddResourceAnimation(int resourceId)
    {
        GameManager.Instance.player_.GetComponent<Animator>().Play(putResourceAnimations_[resourceId], -1, 0);
    }

    public void GoBackAnimation(int resourceId)
    {
        GameManager.Instance.player_.GetComponent<Animator>().Play(goBackAnimations_[resourceId], -1, 0);
        // Spawn resource
    }
}
