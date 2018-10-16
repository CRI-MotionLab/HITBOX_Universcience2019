﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CRI.HitBox.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace CRI.HitBox.Settings
{
    [Serializable]
    public struct GameSettings
    {
        /// <summary>
        /// Is the projection of the camera orthographic ?
        /// </summary>
        [XmlIgnore]
        public bool orthographicProjection { get; private set; }
        /// <summary>
        /// Serialized version of the orthographic projection field.
        /// </summary>
        [XmlElement("orthographic_projection")]
        public string orthographicProjectionSerialized
        {
            get { return this.orthographicProjection ? "True" : "False"; }
            set
            {
                if (value.ToUpper().Equals("TRUE"))
                    this.orthographicProjection = true;
                else if (value.ToUpper().Equals("FALSE"))
                    this.orthographicProjection = false;
                else
                    this.orthographicProjection = XmlConvert.ToBoolean(value);
            }
        }
        /// <summary>
        /// If true, the best score will be saved even if the application shuts down.
        /// </summary>
        [XmlIgnore]
        public bool saveBestScore { get; private set; }
        /// <summary>
        /// Serialized version of the save best score field.
        /// </summary>
        [XmlElement("save_best_score")]
        public string saveBestScoreSerialized
        {
            get { return this.saveBestScore ? "True" : "False"; }
            set
            {
                if (value.ToUpper().Equals("TRUE"))
                    this.saveBestScore = true;
                else if (value.ToUpper().Equals("FALSE"))
                    this.saveBestScore = false;
                else
                    this.saveBestScore = XmlConvert.ToBoolean(value);
            }
        }
        /// <summary>
        /// Duration of the game.
        /// </summary>
        [XmlElement("game_duration")]
        public int gameDuration;
        /// <summary>
        /// The starting value of the combo multiplier.
        /// </summary>
        [XmlElement("combo_min")]
        public int comboMin;
        /// <summary>
        /// The max value of the combo multiplier.
        /// </summary>
        [XmlElement("combo_max")]
        public int comboMax;
        /// <summary>
        /// The duration of the combo bar.
        /// </summary>
        [XmlElement("combo_duration")]
        public float comboDuration;
        /// <summary>
        /// The multiplier of the duration of the combo bar according to the combo value.
        /// </summary>
        [XmlElement("combo_duration_multiplier")]
        public float comboDurationMultiplier;
        /// <summary>
        /// How much of the combo bar is incremented whenever a player hits a target.
        /// The value goes from 0.0 (nothing) to 1.0 (the full bar).
        /// /// </summary>
        [XmlElement("combo_increment")]
        public float comboIncrement;
        /// <summary>
        /// Rotation speed of the target when hit by a player.
        /// </summary>
        [XmlElement("target_rotation_speed")]
        public float targetRotationSpeed;
        /// <summary>
        /// The Z rotation speed of the movement controller.
        /// </summary>
        [XmlElement("target_z_rotation_speed")]
        public float targetZRotationSpeed;
        /// <summary>
        /// The horizontal movement speed of the movement controller when the difficulty level is high enough.
        /// </summary>
        [XmlElement("target_horizontal_movement_speed")]
        public float targetHorizontalMovementSpeed;
        /// <summary>
        /// Max angular velocity of the targets.
        /// </summary>
        [XmlElement("target_max_angular_velocity")]
        public float targetMaxAngularVelocity;
        /// <summary>
        /// Delay before the next target is activated in P1 mode (in seconds).
        /// </summary>
        [XmlElement("target_activation_delay")]
        public float targetP1ActivationDelay;
        /// <summary>
        /// Time until a target is ready to be activated again after its last deactivation (in seconds).
        /// </summary>
        [XmlElement("target_cooldown")]
        public float targetCooldown;
        /// <summary>
        /// The min amount of points a player can get while hitting a sphere.
        /// </summary>
        [XmlElement("hit_min_points")]
        public int hitMinPoints;
        /// <summary>
        /// The max amount of points a player can get while hitting a sphere.
        /// </summary>
        [XmlElement("hit_max_points")]
        public int hitMaxPoints;
        /// <summary>
        /// The max distance from the center of the target to get the maximum amount of points.
        /// The distance goes from 0.0 (the center of the target) to 1.0 (the border of the target).
        /// </summary>
        [XmlElement("hit_tolerance")]
        public float hitTolerance;
        /// <summary>
        /// Threshold for increasing the number of targets.
        /// </summary>
        [XmlArray("target_count_threshold")]
        [XmlArrayItem(typeof(int), ElementName = "threshold")]
        public int[] targetCountThreshold;
        /// <summary>
        /// Precision required to get the max number of stars.
        /// </summary>
        [XmlElement("max_precision_rating")]
        public float maxPrecisionRating;
        /// <summary>
        /// Under this precision value, the player will get 0 star.
        /// </summary>
        [XmlElement("min_precision_rating")]
        public float minPrecisionRating;
        /// <summary>
        /// Speed required to get the max number of stars. The speed is the average time between hits in seconds.
        /// </summary>
        [XmlElement("max_speed_rating")]
        public float maxSpeedRating;
        /// <summary>
        /// Speed required to get the min number of stars. The speed is the average time between hits in seconds.
        /// </summary>
        [XmlElement("min_speed_rating")]
        public float minSpeedRating;
        /// <summary>
        /// Path of the audio file used whenever a player makes a successful hit.
        /// </summary>
        [XmlElement("hit_audio")]
        public StringCommon successfulHitAudioPath;

        public StringCommon[] allGameplayAudioPaths
        {
            get
            {
                return new StringCommon[]{ successfulHitAudioPath }
                    .Where(x => !String.IsNullOrEmpty(x.key))
                    .Distinct()
                    .ToArray();
            }
        }
    }
}