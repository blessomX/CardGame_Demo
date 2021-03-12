using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePool : MonoBehaviour
{
    public List<SpriteRecord> Pool;

    public Sprite GetSprite(string Key)
    {
        foreach (SpriteRecord spriteRecord in Pool)
        {
            if (spriteRecord.Key == Key)
            {
                return spriteRecord.Value;
            }
        }
        return null;
    }

}

[System.Serializable]
public class SpriteRecord
{
    public string Key;
    public Sprite Value;
}