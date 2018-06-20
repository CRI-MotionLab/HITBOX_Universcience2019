﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System.Linq;
using System.Xml;
using System.Text;
using System.IO;
using System.IO.Ports;

[System.Serializable]
public struct SerialPortSettings
{
    public enum SerializableHandshake
    {
        [XmlEnum(Name = "None")]
        None = 0,
        [XmlEnum(Name = "XOnXOff")]
        XOnXOff = 1,
        [XmlEnum(Name = "RequestToSend")]
        RequestToSend = 2,
        [XmlEnum(Name = "RequestToSendXOnXOff")]
        RequestToSendXOnXOff = 3
    }
    /// <summary>
    /// Name of the serial port
    /// </summary>
    [XmlElement("name")]
    public string name;
    /// <summary>
    /// The baud rate of the serial port
    /// </summary>
    [XmlElement("baud_rate")]
    public int baudRate;
    /// <summary>
    /// The timeout of the read.
    /// </summary>
    [XmlElement("read_timeout")]
    public int readTimeOut;
    /// <summary>
    /// The handshake.
    /// </summary>
    [XmlElement("handshake")]
    public SerializableHandshake handshake;

    public SerialPortSettings(string name, int baudRate, SerializableHandshake handshake, int readTimeOut = 0)
    {
        this.name = name;
        this.baudRate = baudRate;
        this.handshake = handshake;
        this.readTimeOut = readTimeOut;
    }
}

[System.Serializable]
public struct ArduinoGrid
{
    /// <summary>
    /// The number of rows.
    /// </summary>
    [XmlAttribute("rows")]
    public int rows;
    /// <summary>
    /// The number of columns.
    /// </summary>
    [XmlAttribute("cols")]
    public int cols;

    public ArduinoGrid(int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;
    }
}

public enum ButtonType
{
    [XmlEnum("start")]
    Start,
    [XmlEnum("sound")]
    Sound,
    [XmlEnum("copyright")]
    Copyright,
    [XmlEnum("separator")]
    Separator,
}

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

[System.Serializable]
public class GameSettings
{
    /// <summary>
    /// The name of the application.
    /// </summary>
    public string name;
    /// <summary>
    /// Time until the application displays the tap screen
    /// </summary>
    [XmlElement("timeout_screen")]
    public int timeOutScreen = 15;
    /// <summary>
    /// Time until the application displays the home screen
    /// </summary>
    [XmlElement("timeout")]
    public int timeOut = 30;
    /// <summary>
    /// Time until the menu goes back to its initial position
    /// </summary>
    [XmlElement("timeout_menu")]
    public int timeOutMenu = 5;
    /// <summary>
    /// Game time.
    /// </summary>
    [XmlElement("game_time")]
    public int gameTime;
    /// <summary>
    /// All of the languages available for the translation of the application.
    /// </summary>
    [XmlArray("lang_app_available")]
    [XmlArrayItem(typeof(LangApp), ElementName = "lang_app")]
    [SerializeField]
    public LangApp[] langAppAvailable;

    /// <summary>
    /// Code of the enabled languages in priority order of appearance in the menus
    /// </summary>
    [XmlArray("lang_app_enable")]
    [XmlArrayItem(typeof(string), ElementName = "code")]
    [SerializeField]
    public string[] langAppEnableCodes;

    /// <summary>
    /// Enabled languages in priority order of appearance in the menus
    /// </summary>
    protected LangApp[] _langAppEnable;

    [XmlArray("page_settings")]
    [XmlArrayItem(typeof(ContentPageSettings), ElementName = "content_page")]
    [XmlArrayItem(typeof(TextOnlyPageSettings), ElementName = "text_page")]
    [XmlArrayItem(typeof(PlayerModeSettings), ElementName = "mode_page")]
    [XmlArrayItem(typeof(CatchScreenPageSettings), ElementName = "catchscreen_page")]
    [SerializeField]
    public PageSettings[] pageSettings;

    [XmlElement("catch_screen_video_path")]
    public string catchScreenVideoPath;
    /// <summary>
    /// Settings for the touch surface serial port
    /// </summary>
    [XmlArray("touch_surface_serial_ports")]
    [XmlArrayItem(typeof(SerialPortSettings), ElementName = "port")]
    [SerializeField]
    public SerialPortSettings[] touchSurfaceSerialPorts;
    /// <summary>
    /// The grid for the touch surface.
    /// </summary>
    [XmlElement("touch_surface_grid")]
    public ArduinoGrid touchSurfaceGrid;
    /// <summary>
    /// Settings for the led control serial port
    /// </summary>
    [XmlArray("led_control_serial_ports")]
    [XmlArrayItem(typeof(SerialPortSettings), ElementName = "port")]
    [SerializeField]
    public SerialPortSettings[] ledControlSerialPorts;
    /// <summary>
    /// The grid for the led control.
    /// </summary>
    [XmlElement("led_control_grid")]
    public ArduinoGrid ledControlGrid;
    /// <summary>
    /// Min value to detect impact
    /// </summary>
    [XmlElement("impact_threshold")]
    public int impactThreshold;
    /// <summary>
    /// The buttons of the menu in the order they appear from left to right. The start button will be in all the enabled langages.
    /// </summary>
    [XmlArray("menu_layout")]
    [XmlArrayItem(typeof(ButtonType), ElementName = "button_type")]
    public ButtonType[] menuLayout;

    [XmlIgnore]
    public IList<LangApp> langAppEnable
    {
        get
        {
            return _langAppEnable.ToList().AsReadOnly();
        }
    }

    /// <summary>
    /// Gets the default language, which is the first of the list.
    /// </summary>
    /// <value>The default language.</value>
    [XmlIgnore]
    public LangApp defaultLanguage
    {
        get
        {
            return _langAppEnable[0];
        }
    }

    public const int PlayerNumber = 2;


    /// <summary>
    /// Save the data to an XML file.
    /// </summary>
    /// <param name="path">The path where the XML file will be saved.</param>
    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(GameSettings));
        using (var stream = new FileStream(Path.Combine(Application.dataPath, path), FileMode.Create))
        using (var xmlWriter = new XmlTextWriter(stream, Encoding.UTF8))
        {
            xmlWriter.Formatting = Formatting.Indented;
            serializer.Serialize(xmlWriter, this);
        }
    }

    /// <summary>
    /// Loads a game setting data from the XML file at the specified path.
    /// </summary>
    /// <returns>The game setting data.</returns>
    /// <param name="path">The path here the XML file is located.</param>
    public static GameSettings Load(string path)
    {
        var serializer = new XmlSerializer(typeof(GameSettings));
        using (var stream = new FileStream(Path.Combine(Application.dataPath, path), FileMode.Open))
        {
            var gameSettings = serializer.Deserialize(stream) as GameSettings;
            gameSettings._langAppEnable = new LangApp[gameSettings.langAppEnableCodes.Length];
            for (int i = 0; i < gameSettings.langAppEnableCodes.Length; i++)
            {
                var code = gameSettings.langAppEnableCodes[i];
                var langApp = gameSettings.langAppAvailable.First(x => x.code == code);
                gameSettings._langAppEnable[i] = langApp;
            }
            return gameSettings;
        }
    }

    /// <summary>
    /// Loads a game setting from an XML text.
    /// </summary>
    /// <returns>The game setting data.</returns>
    /// <param name="text">A text in XML format that contains the data that will be loaded.</param>
    public static GameSettings LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(GameSettings));
        return serializer.Deserialize(new StringReader(text)) as GameSettings;
    }
}