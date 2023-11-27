using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Story
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