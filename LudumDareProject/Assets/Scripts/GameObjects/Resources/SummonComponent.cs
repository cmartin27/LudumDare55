using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SummonComponent : MonoBehaviour
{
    public float fadeDuration_ = 1.0f;
    SpriteRenderer spriteRenderer_;
    public AnimationCurve movingAnimationCurve_;
    public float movingAnimationDuration_;
    public float movingDistance_ = 0.2f;

    public bool isMovingAnimation_;

    public Vector3 initialPosition_;

    // Start is called before the first frame update
    public void InitialSetup(Vector3 initialPosition)
    {
        spriteRenderer_ = GetComponent<SpriteRenderer>();
        initialPosition_ = initialPosition;
        transform.position = initialPosition;
        isMovingAnimation_ = false;
        StartCoroutine(ShowSprite());
    }

    public void SetSprite(Sprite image)
    {
        spriteRenderer_.sprite = image;
    }

    private IEnumerator ShowSprite()
    {
        Color initialColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        Color targetColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration_)
        {
            elapsedTime += Time.deltaTime;
            spriteRenderer_.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeDuration_);
            yield return null;
        }

        isMovingAnimation_ = true;
        StartCoroutine(AnimatorMove());
    }

    private IEnumerator AnimatorMove()
    {
        float elapsedTime = 0.0f;

        while (isMovingAnimation_)
        {
            elapsedTime += Time.deltaTime;
            Vector3 newPosition = initialPosition_;
            newPosition.y += movingAnimationCurve_.Evaluate(elapsedTime / movingAnimationDuration_) * movingDistance_;
            transform.position = newPosition;
            if (elapsedTime > movingAnimationDuration_) elapsedTime = 0.0f;
            yield return null;
        }
    }


}
