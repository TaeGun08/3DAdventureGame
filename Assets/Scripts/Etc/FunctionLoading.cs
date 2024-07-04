using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FunctionLoading : MonoBehaviour
{
    [Header("�ε�����")]
    [SerializeField] private float loadingTime; //�ε��� ���� �ð�
    private float loadingTimer; //�ε� ��
    private bool loadingEnd = false; //�ε��� �������� üũ

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
