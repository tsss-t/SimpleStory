//SceneFadeInOut.cs
using UnityEngine;
using System.Collections;

public class UISceneFader : MonoBehaviour
{
    public static UISceneFader _instance;
    public float fadeSpeed = 1.5f;

    private bool sceneStarting = true;

    private GUITexture thisTexture;

    bool switchFlag;
    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        iTween.AudioTo(GameObject.FindGameObjectWithTag(Tags.mainCamera), 0.15f, 1f, 1f); ;
        switchFlag = true;
        thisTexture = GetComponent<GUITexture>();
        thisTexture.pixelInset = new
            Rect(0f, 0f, Screen.width, Screen.height);
    }

    void Update()
    {
        if (switchFlag)
        {
            if (sceneStarting)
            {
                StartFadeIn();
            }
            else
            {
                StartFadeOut();
            }
        }
    }


    void StartFadeIn()
    {
        FadeToClear();
        if (thisTexture.color.a <= 0.05f)
        {
            thisTexture.color =
                Color.clear;
            sceneStarting = false;
            switchFlag = false;
            thisTexture.enabled = false;
        }
    }

    void StartFadeOut()
    {
        thisTexture.enabled = true;
        FadeToBlack();
        if (thisTexture.color.a >= 0.95f)
        {
            thisTexture.color = Color.black;
            sceneStarting = true;
            switchFlag = false;
        }
    }
    void FadeToClear()
    {
        thisTexture.color = Color.Lerp
            (thisTexture.color, Color.clear, fadeSpeed *
             Time.deltaTime);
    }

    void FadeToBlack()
    {
        thisTexture.color = Color.Lerp
            (thisTexture.color, Color.black, fadeSpeed *
             Time.deltaTime);

    }
    public void changeFade()
    {
        switchFlag = true;
    }
}