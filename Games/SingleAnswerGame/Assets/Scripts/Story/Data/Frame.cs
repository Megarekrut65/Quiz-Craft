using System;
using UnityEngine.Serialization;

namespace Story.Data
{
    [Serializable]
    public class Frame
    {
        public string id;
        
        public string background;
        public string textKey;

        public string music;

        public Images images;
        
        public Answer[] answers;
    }
}