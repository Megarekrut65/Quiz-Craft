using System.Collections.Generic;
using Global;
using Global.Json;
using JetBrains.Annotations;
using UnityEngine;

namespace Story.Data
{
    public class JsonStoryParser
    {
        public SortedDictionary<string, Frame> Frames { get; private set; } = new SortedDictionary<string, Frame>();

        private readonly string _fullPath;
        public JsonStoryParser(string fullPath, string mainFrame)
        {
            _fullPath = fullPath;
            ReadFrame(mainFrame);
        }
        private void ReadFrame([CanBeNull] string frameName)
        {
            if(frameName == null || Frames.ContainsKey(frameName)) return;
            
            string path = $"{_fullPath}/Frames/{frameName}";

            Frame frame = JsonObjectParser<Frame>.Parse(path);
            Frames.Add(frame.id, frame);
            
            if(frame.answers == null) return;
            foreach(Answer answer in frame.answers)
            {
                ReadFrame(answer.nextFrameId);
            }
        }
    }
}