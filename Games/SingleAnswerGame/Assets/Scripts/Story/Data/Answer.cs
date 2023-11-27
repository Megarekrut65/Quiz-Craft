using System;

namespace Story.Data
{
    [Serializable]
    public class Answer
    {
        public string textKey;
        public string nextFrameId;
        public string action;

        public string clickSound;
    }
}