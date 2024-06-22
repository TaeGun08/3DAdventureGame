using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    private static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DataManager();
            }
            return instance;
        }
    }

    private List<Component> listControllers = new List<Component>();

    public void Add(Component _value)
    {
        listControllers.Add(_value);
    }

    public void Remove(Component _value)
    {
        listControllers.Remove(_value);
    }

    public T Get<T>(System.Type _value) where T : class
    {
        return listControllers.Find(x => x.GetType() == _value) as T;
    }
}
