using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[Serializable]
public class Path
{
    public List<Vector3> points_;
}


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
    public List<Path> resourcesPath_;
    public List<Path> backPath_;
    public GameObject positionOfReference_;
    public Vector3 playerInitialPosition_;

    [Header("Spawn")]
    public GameObject resourceObjectPrefab_;
    public GameObject summonedObjectPrefab_;
    public List<Vector3> resourceSpawnPosition_;
    public Vector3 summonedObjectSpawnPosition_;

    [Header("End Animation")]
    public float finalAnimationDuration_;

    bool isGoingBack_;
    GameObject summonedObject_;
    UnityEvent fadeInCompleted_;
    UnityEvent fadeOutCompleted_;

    public void Start()
    {
        GameManager.Instance.player_.GetComponent<MovementComponent>().pathCompleted_.AddListener(OnPathCompleted);
    }

    public void StartSummoningAnimation(Vector3 initialPosition)
    {
        Debug.Log("Start Summoning Animation");
        isGoingBack_ = false;
        transform.position = initialPosition;
        fadeInCompleted_.AddListener(OnStartFadeInCompleted);
        fadeOutCompleted_.AddListener(OnStartFadeOutCompleted);


        StartCoroutine(FadeIn());
    }

    void OnStartFadeInCompleted()
    {
        InitialSetup();
        StartCoroutine(FadeOut());
    }


    void OnStartFadeOutCompleted()
    {
        fadeInCompleted_.RemoveAllListeners();
        fadeOutCompleted_.RemoveAllListeners();
        StartCoroutine(ActivateCircle());
    }

    private void InitialSetup()
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

        yield return new WaitForSeconds(fadeDuration_);
        fadeInCompleted_.Invoke();

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
        fadeOutCompleted_.Invoke();
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


    private IEnumerator HidCircle()
    {
        Color initialColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Color targetColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        float elapsedTime = 0f;

        while (elapsedTime < fadeOutDuration_)
        {
            elapsedTime += Time.deltaTime;
            summoningCircle_.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeOutDuration_);
            yield return null;
        }

        GameManager.Instance.SetInputMode(EInputMode.InGame);
    }

    public void AddResourceAnimation(int resourceId)
    {
        GameManager.Instance.player_.GetComponent<MovementComponent>().StartPath(resourcesPath_[resourceId].points_);

    }

    public void AnimationFinished()
    {
        Debug.Log("Animation finished");

    }

    public void GoBackAnimation(int resourceId)
    {
        isGoingBack_ = true;
        GameManager.Instance.player_.GetComponent<MovementComponent>().StartPath(backPath_[resourceId].points_);
    }

    public void OnPathCompleted()
    {
        if (isGoingBack_) 
        {
            summonedObject_ = Instantiate(summonedObjectPrefab_, positionOfReference_.transform);
            summonedObject_.GetComponent<SummonComponent>().InitialSetup(summonedObjectSpawnPosition_);
        }
        else
        {
            GameManager.Instance.SetInputMode(EInputMode.Summoning);
            GameObject resource = Instantiate(resourceObjectPrefab_, positionOfReference_.transform);

            int resourceId = GameManager.Instance.summoningManager_.GetCurrentResource();
            //resource.GetComponent<SummonComponent>().SetSprite();
            resource.GetComponent<SummonComponent>().InitialSetup(resourceSpawnPosition_[resourceId]);
        }
    }


    IEnumerator EndAnimation(int resourceId)
    {
        yield return new WaitForSeconds(finalAnimationDuration_);
        fadeInCompleted_.AddListener(OnEndFadeInCompleted);
        fadeOutCompleted_.AddListener(OnEndFadeOutCompleted);
        StartCoroutine(FadeIn());
    }

    void OnEndFadeInCompleted()
    {
        GameManager.Instance.mainCamera_.SetActive(true);
        summoningCamera_.SetActive(false);
        GameManager.Instance.player_.transform.parent = null;
        StartCoroutine(FadeOut());
    }

    void OnEndFadeOutCompleted()
    {
        fadeInCompleted_.RemoveAllListeners();
        fadeOutCompleted_.RemoveAllListeners();
        StartCoroutine(HidCircle());
    }
}
