using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]//이 스크립트가 동작하는 환경에 해당 컴포넌트가 있으면 통과, 없으면 생성 ,어트리뷰트
public class FunctionFade : MonoBehaviour
{
    public static FunctionFade Instance;

    private Image imgFade;
    [SerializeField] float fadeTime = 1.0f;//페이드 되는 시간 및 복구되는 시간

    [SerializeField] int targetFrm = 34;

    UnityAction actionFadeOut;//페이드 아웃 되었을때 동작할 기능
    UnityAction actionFadeIn;//페이드 인 되었을때 동작할 기능

    bool fade = true;//true가 In false가 Out;

    private void Awake()
    {
        imgFade = GetComponent<Image>();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(functionFade());
    }

    //private void Update()
    //{
    //    int currentFrm = (int)(1 / Time.deltaTime); //이전 프레임과 현재 프레임의 시간
    //    if (currentFrm < targetFrm) return;

    //    if (fade && imgFade.color.a > 0)
    //    {
    //        Color color = imgFade.color;
    //        color.a -= Time.unscaledDeltaTime / fadeTime;
    //        if (color.a < 0)
    //        {
    //            color.a = 0;
    //            invokeAction(fade);
    //        }
    //        imgFade.color = color;
    //    }
    //    else if (!fade && imgFade.color.a < 1)
    //    {
    //        Color color = imgFade.color;
    //        color.a += Time.unscaledDeltaTime / fadeTime;
    //        if (color.a > 1)
    //        {
    //            color.a = 1;
    //            invokeAction(fade);
    //        }
    //        imgFade.color = color;
    //    }

    //    imgFade.raycastTarget = imgFade.color.a != 0;
    //}

    public IEnumerator functionFade()
    {
        yield return new WaitForEndOfFrame();

        Time.timeScale = 0;

        int currentFrm = 0;
        while (currentFrm < targetFrm)
        {
            currentFrm = (int)(1 / Time.unscaledDeltaTime); //이전 프레임과 현재 프레임의 시간
            yield return null;
        }

        Color curColor = imgFade.color;
        Color targetColor = imgFade.color;
        targetColor.a = fade == true ? 0 : 1;

        float ratio = 0;


        float fakeLoadingTime = 1.5f;
        float timer = 0;

        //yield return new WaitUntil(() =>
        //{
        //    if (timer > fakeLoadingTime && curColor.a != 0)
        //    {
        //        return true;
        //    }
        //    timer += Time.unscaledDeltaTime;
        //    return false;
        //});

        while (timer < fakeLoadingTime && curColor.a != 0)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        while (ratio < 1)
        {
            ratio += Time.unscaledDeltaTime / fadeTime;
            if (ratio > 1)
            {
                Time.timeScale = 1.0f;
                ratio = 1;
            }

            imgFade.color = Color.Lerp(curColor, targetColor, ratio);
            yield return null;
        }

        invokeAction(fade);
        imgFade.raycastTarget = imgFade.color.a != 0;
    }

    private void invokeAction(bool _fade)
    {
        switch (_fade)
        {
            case true:
                {
                    if (actionFadeIn != null)
                    {
                        actionFadeIn.Invoke();
                        actionFadeIn = null;
                    }
                }
                break;

            case false:
                {
                    if (actionFadeOut != null)
                    {
                        actionFadeOut.Invoke();
                        actionFadeOut = null;
                    }
                }
                break;
        }
    }

    /// <summary>
    /// 페이드 기능을 수행
    /// </summary>
    /// <param name="_fade">true는 In false 는 Out</param>
    /// <param name="_action">ture일시 In이 될때 기능이, false일때 Out기능을 등록합니다</param>
    public void SetActive(bool _fade, UnityAction _action = null)
    {
        fade = _fade;
        switch (_fade)
        {
            case true: actionFadeIn = _action; break;
            case false: actionFadeOut = _action; break;
        }
        StartCoroutine(functionFade());
    }

    /// <summary>
    /// 알파값을 변경해주기 위한 함수
    /// </summary>
    /// <param name="_a"></param>
    public void SetImageAlpha(float _a)
    {
        Color color = imgFade.color;
        color.a = _a;
        imgFade.color = color;
    }

    public float DontEsc()
    {
        Color color = imgFade.color;
        return color.a;
    }
}
