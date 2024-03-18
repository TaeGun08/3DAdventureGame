using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputController;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public class SaveData
    {
        public int idleChange;
        public float moveSpeed;
        public float maxStamina;
        public float curStamina;
        public float playerMaxExp;
        public int playerCurExp;
        public float levelPoint;
        public int statusPoint;
        public int skillPoint;
        public int weaponLevel;
        public float playerDamage;
        public float playerMaxHp;
        public float playerCurHp;
        public float playerArmor;
        public int weaponNumber;
    }

    private SaveData saveData = new SaveData();

    private string playerDataSaveKey = "playerDataSaveKey";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (PlayerPrefs.GetString(playerDataSaveKey) == string.Empty)
        {
            PlayerPrefs.SetString(playerDataSaveKey, string.Empty);
        }
    }

    /// <summary>
    /// 플레이어 데이터를 저장하기 위한 함수
    /// </summary>
    /// <param name="_playerData"></param>
    public void SetPlayerData(PlayerData _playerData)
    {
        saveData.idleChange = _playerData.idleChange;
        saveData.moveSpeed = _playerData.moveSpeed;
        saveData.maxStamina = _playerData.maxStamina;
        saveData.curStamina = _playerData.curStamina;
        saveData.playerMaxExp = _playerData.playerMaxExp;
        saveData.playerCurExp = _playerData.playerCurExp;
        saveData.levelPoint = _playerData.levelPoint;
        saveData.statusPoint = _playerData.statusPoint;
        saveData.skillPoint = _playerData.skillPoint;
        saveData.weaponLevel = _playerData.weaponLevel;
        saveData.playerDamage = _playerData.playerDamage;
        saveData.playerMaxHp = _playerData.playerMaxHp;
        saveData.playerCurHp = _playerData.playerCurHp;
        saveData.playerArmor = _playerData.playerArmor;
        saveData.weaponNumber = _playerData.weaponNumber;

        string playerSaveData = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(playerDataSaveKey, playerSaveData);
    }

    /// <summary>
    /// 플레이어 데이터를 불어오기 위한 함수
    /// </summary>
    public void GetPlayerData()
    {
        string playerSaveData = PlayerPrefs.GetString(playerDataSaveKey);
        saveData = JsonUtility.FromJson<SaveData>(playerSaveData);
    }

    public SaveData GetSaveData()
    {
        return saveData;
    }
}
