using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FunctionLoading : MonoBehaviour
{
    [Header("로딩설정")]
    [SerializeField] private float loadingTime; //로딩이 끝날 시간
    private float loadingTimer; //로딩 중
    private bool loadingEnd = false; //로딩이 끝났는지 체크

    private void Awake()
    {
        FunctionFade.Instance.gameObject.SetActive(false);
        FunctionFade.Instance.SetImageAlpha(1f);

        Time.timeScale = 1;
    }

    private void Update()
    {
        if (loadingEnd == false)
        {
            loadingTimer += Time.deltaTime;

            if (loadingTimer >= loadingTime)
            {
                FunctionFade.Instance.gameObject.SetActive(true);

                string get = PlayerPrefs.GetString("saveSceneName");
                string getScene = JsonConvert.DeserializeObject<string>(get);
                SceneManager.LoadSceneAsync(getScene);
                loadingTimer = 0;
                loadingEnd = true;
            }
        }
    }
}
