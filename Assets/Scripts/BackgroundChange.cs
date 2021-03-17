using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEngine.UI;

public class BackgroundChange : MonoBehaviour
{
    public List<Sprite> AvaibleSprites;
    public string CurrentName;
    public SpriteRenderer Table;
    public GameObject Panel;

    private void Start()
    {
        var Backgrounds = GameObject.FindGameObjectWithTag("SpritePool").GetComponent<SpritePool>();
        Table = GameObject.FindGameObjectWithTag("Table").GetComponent<SpriteRenderer>();
        AvaibleSprites = Backgrounds.Pool.Where(x => x.Key.IndexOf("table") > -1).Select(x => x.Value).ToList<Sprite>();
        CurrentName = AvaibleSprites[0].name;
        LoadCurrentBackground();
        Table.sprite = AvaibleSprites.Where(x => x.name == CurrentName).ToArray()[0];
    }

    public void OpenPanel()
    {
        Panel.SetActive(true);
    }

    public void ChangeBackground(GameObject Background)
    {
        Table.sprite = Background.GetComponent<Image>().sprite;
        CurrentName = Table.sprite.name;
        SaveCurrentBackground();
        Panel.SetActive(false);
    }
    
    public void LoadCurrentBackground()
    {
        if(File.Exists(Application.persistentDataPath + "/SaveData.sav"))
            CurrentName = File.ReadAllText(Application.persistentDataPath + "/SaveData.sav");
    }

    public void SaveCurrentBackground()
    {
        if (!File.Exists(Application.persistentDataPath + "/SaveData.sav"))
        {
            FileStream filenew = new FileStream(Application.persistentDataPath + "/SaveData.sav", FileMode.Create, FileAccess.Write);
            filenew.Close();
        }
        File.WriteAllText(Application.persistentDataPath + "/SaveData.sav", CurrentName);
    }
}
