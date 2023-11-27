using System;

namespace Story.Data
{
    [Serializable]
    public class Configuration
    {
        public string id;
        public string title;
        
        public string author;
        public string authorId;
        
        public string mainFrame;

        public string font;
    }
}