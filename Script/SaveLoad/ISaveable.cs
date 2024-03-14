using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    DataDefination GetDataID();
    //注册保存的物体
    void RegisterSaveData() => DataManager.instance.RegisterSaveData(this);
    //注销已经注册的物体
    void UnRegisterSaveData() => DataManager.instance.UnRegisterSaveData(this);
    
    void GetSaveData(Data data);
    void LoadData(Data data);
}
