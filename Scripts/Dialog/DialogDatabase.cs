using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

public partial class DialogDatabase : Node
{
    private const string PATH = "res://Database/Dialog.json";

    private List<DialogData> LevelOne = new List<DialogData>();
    public override void _Ready()
    {
        base._Ready();
        LoadDatabase();
    }

    public List<DialogData> GetLevelDialog(int level)
    {
        switch (level)
        {
            case 1:
                return LevelOne;
        }

        return null;
    }

    public DialogData GetDialog(string id, int level)
    {
        switch (level)
        {
            case 1:
                foreach(var levelOneDialog in LevelOne)
                    if (levelOneDialog.DialogID == id)
                        return levelOneDialog;
                break;
        }

        return null;
    }

    private void LoadDatabase()
    {
        var file = FileAccess.Open(PATH, FileAccess.ModeFlags.Read);
        if (file != null && file.IsOpen())
        {
            string text = file.GetAsText();
            var objects = JArray.Parse(text);

            foreach (var o in objects)
            {
                JToken token = o;
                DialogJson data = JsonConvert.DeserializeObject<DialogJson>(token.ToString());
                string[] nextID = data.Action.Split("NextID:");
                DialogData newDialog = new DialogData(data.ID, data.Name, data.Message, nextID[1]);
                AddToLevel(newDialog, data.Level);
            }
        }
    }

    public void AddToLevel(DialogData data, int level)
    {
        switch (level)
        {
            case 1:
                LevelOne.Add(data);
                break;
        }
    }

}

public class DialogJson
{
    [JsonProperty] public string ID;
    [JsonProperty] public string Name;
    [JsonProperty] public int Level;
    [JsonProperty] public string Message;
    [JsonProperty] public string Action;
}