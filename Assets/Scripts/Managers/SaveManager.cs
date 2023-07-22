using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{

    public TestData TestData;

    private void Start()
    {
        TestData = LoadData<TestData>("testData");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SaveData("testData", TestData);
        }
    }

    public void SaveData<T>(string key, T toSerialize) 
    {
        string data = DataBehaviour.Serialize<T>(toSerialize);

        PlayerPrefs.SetString(key, data);
    }

    public T LoadData<T>(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string data = PlayerPrefs.GetString(key);

            return DataBehaviour.Deserialize<T>(data);
        }
        else
        {
            return default;
        }
    }
}
