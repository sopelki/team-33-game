using System.IO;
using UnityEngine;

public class SaveLoadManager
{
    private static string saveFilePath = Path.Combine(Application.dataPath, "Scripts", "Levels", "MyLevel.json");

    public static void SaveMapToFile(Field fieldToSave)
    {
        var data = fieldToSave.ExportToSaveData();
        var json = JsonUtility.ToJson(data, true); 
        
        File.WriteAllText(saveFilePath, json);
        
        Debug.Log($"Карта сохранена в файл!\nПуть: {saveFilePath}");
    }

    public static Field LoadMapFromFile()
    {
        if (File.Exists(saveFilePath))
        {
            var json = File.ReadAllText(saveFilePath);
            var data = JsonUtility.FromJson<FieldData>(json);
            
            var loadedField = new Field();
            loadedField.ImportFromFieldData(data);

            Debug.Log("<color=green>Карта успешно загружена из файла!</color>");
            return loadedField;
        }
        Debug.LogError("Файл сохранения не найден!");
        return null;
    }
}