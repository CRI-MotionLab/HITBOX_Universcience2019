﻿using CRI.HitBox.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CRI.HitBox.Database
{
    public class HitData : DataEntry
    {
        /// <summary>
        /// Index of the hit.
        /// </summary>
        [Field("id"), AutoIncrement, PrimaryKey]
        public int id { get; set; }
        /// <summary>
        /// The index of the player that caused the hit.
        /// </summary>
        [Field("player_id")]
        public int playerId { get; set; }
        /// <summary>
        /// Time when the hit occured.
        /// </summary>
        [Field("time")]
        public DateTime time { get; set; }
        /// <summary>
        /// X position of the hit.
        /// </summary>
        [Field("position_x")]
        public float positionX { get; set; }
        /// <summary>
        /// Y position of the hit.
        /// </summary>
        [Field("position_y")]
        public float positionY { get; set; }
        /// <summary>
        /// Was the hit successful ?
        /// </summary>
        [Field("successful")]
        public bool successful { get; set; }
        /// <summary>
        /// X position of the center of the target that was hit.
        /// </summary>
        [Field("target_center_x")]
        public float? targetCenterX { get; set; }
        /// <summary>
        /// Y position of the center of the target that was hit.
        /// </summary>
        [Field("target_center_y")]
        public float? targetCenterY { get; set; }
        /// <summary>
        /// Z position of the center of the target that was hit.
        /// </summary>
        [Field("target_center_z")]
        public float? targetCenterZ { get; set; }
        /// <summary>
        /// X position of the target.
        /// </summary>
        [Field("target_speed_vector_x")]
        public float? targetSpeedVectorX { get; set; }
        /// <summary>
        /// Y position of the target.
        /// </summary>
        [Field("target_speed_vector_y")]
        public float? targetSpeedVectorY { get; set; }
        /// <summary>
        /// Z position of the target.
        /// </summary>
        [Field("target_speed_vector_z")]
        public float? targetSpeedVectorZ { get; set; }

        public Vector2 position
        {
            get
            {
                return new Vector2(positionX, positionY);
            }
            set
            {
                positionX = value.x;
                positionY = value.y;
            }
        }

        public Vector3? targetCenter
        {
            get
            {
                if (targetCenterX == null || targetCenterY == null || targetCenterZ == null)
                    return null;
                return new Vector3(targetCenterX.Value, targetCenterY.Value, targetCenterZ.Value);
            }
            set
            {
                if (value == null)
                    return;
                Vector3 vector = value.Value;
                targetCenterX = vector.x;
                targetCenterY = vector.y;
                targetCenterZ = vector.z;
            }
        }

        public Vector3? targetSpeedVector
        {
            get
            {
                if (targetSpeedVectorX == null || targetSpeedVectorY == null || targetSpeedVectorZ == null)
                    return null;
                return new Vector3(targetSpeedVectorX.Value, targetSpeedVectorY.Value, targetSpeedVectorZ.Value);
            }
            set
            {
                if (value == null)
                    return;
                Vector3 vector = value.Value;
                targetSpeedVectorX = vector.x;
                targetSpeedVectorY = vector.y;
                targetSpeedVectorZ = vector.z;
            }
        }

        public const string name = "hit";
        public const string tableName = "hits";

        public override string GetTypeName()
        {
            return name;
        }

        public override string GetTableName()
        {
            return tableName;
        }

        public HitData(int id, PlayerData player, DateTime time, Vector2 position, bool successful, Vector3? targetCenter, Vector3? targetSpeedVector)
        {
            this.id = id;
            playerId = player.id;
            this.time = time;
            this.position = position;
            this.successful = successful;
            this.targetCenter = targetCenter;
            this.targetSpeedVector = targetSpeedVector;
        }

        public HitData() { }
    }
}
