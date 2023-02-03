using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    #region Properties
    [Header("References")]
    [SerializeField]
    GameObject barGO;

    [Header("Progress Bar values")]
    [SerializeField]
    [Range(1, 100)]
    int fullHealth = 10;
    [SerializeField]
    [Range(0, 100)]
    int currentHealth;
    int previousHealth;

    bool easingInProgress = false;

    [Header("Progress Speed")]
    [SerializeField]
    float timeToCompletition = 1f;


    #endregion

    #region EASING FUNCTIONS

    public delegate float EaserDelegate(float value);
    public EaserDelegate easer;

    public enum EasingType
    {
        easing_1, easeOutBounce, easeInOutQuint, easeInOutQuad, easeOutQuart
    }

    public EasingType easerType = EasingType.easeOutBounce;

    #region Set Ease
    private void SetEase(EasingType type)
    {
        switch (type)
        {
            case EasingType.easing_1:

                // Set correct function as Delegate
                easer += DoEasing_1;

                break;
            // ===================
            case EasingType.easeOutBounce:

                easer += EaseOutBounce;

                break;
            // ====================
            case EasingType.easeInOutQuint:

                easer += EaseInOutQuint;

                break;
            // ====================
            case EasingType.easeInOutQuad:

                easer += EaseInOutQuad;

                break;
            // ====================
            case EasingType.easeOutQuart:

                easer += EaseOutQuart;

                break;
            // ====================
            default:
                Debug.LogWarning("Unknown Easing type detected.");
                break;
        }
    }
    #endregion


    #region Easings
    private float DoEasing_1(float number)
    {
        return 1 - Mathf.Cos((number * Mathf.PI) / 2);
    }

    private float EaseOutBounce(float value)
    {
        float n1 = 7.5625f;
        float d1 = 2.75f;

        if (value < 1 / d1)
        {
            return n1 * value * value;
        }
        else if (value < 2 / d1)
        {
            return n1 * (value -= 1.5f / d1) * value + 0.75f;
        }
        else if (value < 2.5 / d1)
        {
            return n1 * (value -= 2.25f / d1) * value + 0.9375f;
        }
        else
        {
            return n1 * (value -= 2.625f / d1) * value + 0.984375f;
        }
    }

    private float EaseInOutQuint(float x)
    {
        return x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
    }

    private float EaseInOutQuad(float x)
    {
        return x < 0.5 ? 2 * x * x : 1 - ((-2 * x + 2) * (-2 * x + 2)) / 2;
    }

    private float EaseOutQuart(float x)
    {
        return 1 - (1 - x) * (1 - x) * (1 - x) * (1 - x);
    }
    #endregion
    #endregion


    #region Setup
    private void Start()
    {
        currentHealth = fullHealth;
        previousHealth = currentHealth;

        StartCoroutine(LoadProgress(currentHealth, previousHealth));
    }

    private void StartProgressChange(float current, float previous)
    {
        StartCoroutine(LoadProgress(currentHealth, previousHealth));
    }

    IEnumerator LoadProgress(float goalValue, float startValue)
    {
        float timeBetweenIncrements = 0.01f;
        WaitForSeconds wait = new WaitForSeconds(timeBetweenIncrements);

        // Calculate correct increment
        float increment = timeBetweenIncrements / timeToCompletition;





        SetEase(easerType);

        easingInProgress = true;

        float progress = 0f;
        while (progress < 1f)
        {
            yield return wait;

            // Increment the Progress Bar
            //SetFill(DoEasing_1(progress));
            //SetFill(EaseOutBounce(progress));
            SetFill(easer(progress));

            //CalculateFill()

            progress += increment;

        }

        easingInProgress = false;
    }

    #endregion


    

    #region Progress Bar

    private void CalculateFill(float original, float current)
    {

        if (current > original) { current = original; }


        // Set Progressbar fill according to Current Health/Progress
        float fill = (current / original);
        //Debug.Log(fill);

        SetFill(fill);
    }

    private void SetFill(float value)
    {
        Vector3 scale = new Vector3(value, 1);

        barGO.transform.localScale = scale;
    }


    private void Update()
    {
        if (!easingInProgress)
        {
            CalculateFill(fullHealth, currentHealth);

        }
    }
    #endregion


}
