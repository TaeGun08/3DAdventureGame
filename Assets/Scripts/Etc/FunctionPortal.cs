using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FunctionPortal : MonoBehaviour
{
    [Header("현재 이동할 씬")]
    [SerializeField] private string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            FunctionFade.Instance.SetActive(false, () =>
            {
                string setLoding = JsonConvert.SerializeObject(sceneName);
                PlayerPrefs.SetString("saveSceneName", setLoding);
                SceneManager.LoadSceneAsync("Loading");

                FunctionFade.Instance.SetActive(true);
            });
        }
    }
}
