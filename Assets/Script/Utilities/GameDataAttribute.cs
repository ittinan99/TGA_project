using System;

namespace  TGA.GameData
{
    public class GameDataAttribute : Attribute
    {
        public GameDataAttribute(string attributeName)
        {
            JsonElement = attributeName;
        }

        public string JsonElement { get; set; }
    }
}
