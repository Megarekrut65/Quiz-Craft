using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Quiz
{
    public class FontManager: MonoBehaviour
    {
        [SerializeField] private Font[] fonts;

        [CanBeNull]
        public Font GetFont(string fontName)
        {
            return Array.Find(fonts, font => font.name == fontName);
        }
    }
}