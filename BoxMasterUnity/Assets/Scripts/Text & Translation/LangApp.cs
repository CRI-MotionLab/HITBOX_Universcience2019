﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace CRI.HitBox.Lang
{
    /// <summary>
    /// Describes the lang of the application.
    /// </summary>
    [System.Serializable]
    public struct LangApp
    {
        [XmlAttribute("code")]
        /// <summary>
        /// The code ISO 639-1 of the language.
        /// </summary>
        public string code;
        [XmlAttribute("name")]
        /// <summary>
        /// The english name of the language.
        /// </summary>
        public string name;

        [XmlIgnoreAttribute]
        public Color color;

        [XmlAttribute("color")]
        public string colorAsHex
        {
            get { return ColorUtility.ToHtmlStringRGB(color); }
            set
            {
                Color color;
                ColorUtility.TryParseHtmlString(value, out color);
                this.color = color;
            }
        }

        public LangApp(string code, Color color, string name = "")
        {
            this.code = code;
            this.name = name;
            this.color = color;
            Debug.Log(color);
        }
    }

}
