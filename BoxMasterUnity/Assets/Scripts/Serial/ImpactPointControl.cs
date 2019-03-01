﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CRI.HitBox.Serial
{
    public class ImpactPointControl : MonoBehaviour
    {
        public delegate void ImpactPointControlEvent(Vector2 position, int playerIndex);
        /// <summary>
        /// This event is fired whenever an impact is detected.
        /// </summary>
        public static event ImpactPointControlEvent onImpact;

        /// <summary>
        /// The point grid.
        /// </summary>
        private GameObject[] _pointGrid;
        /// <summary>
        /// The led controller.
        /// </summary>
        private SerialLedController _ledController;

        /// <summary>
        /// X coordinate of current impact.
        /// </summary>
        private float _xG = 0f;
        /// <summary>
        /// Y coordinate of the current impact.
        /// </summary>
        private float _yG = 0f;
        /// <summary>
        /// Total pressure of current impact.
        /// </summary>
        private float _totG = 0;

        /// <summary>
        /// Min value to detect impact.
        /// </summary>
        [Tooltip("Min value to detect impact.")]
        public float threshImpact = 20;
        /// <summary>
        /// Minimum time (in ms) between 2 impacts to be validated (minimum 50ms <=> maximum 50 hits/s)
        /// </summary>
        [Tooltip("Minimum time (in ms) between 2 impacts to be validated (minimum 50ms <=> maximum 50 hits/s)")]
        public int delayOffHit = 50;
        /// <summary>
        /// Time of the last valid impact.
        /// </summary>
        private float _timerOffHit0 = 0;
        /// <summary>
        /// Position of the impact.
        /// </summary>
        [SerializeField]
        [Tooltip("Position of the impact.")]
        private Vector3 _position;

        /// <summary>
        /// Position of the impact.
        /// </summary>
        public Vector3 position { get { return _position; } }

        /// <summary>
        /// The index of the player.
        /// </summary>
        [Tooltip("The index of the player.")]
        public int playerIndex = 0;
        /// <summary>
        /// Number of hit.
        /// </summary>
        private int _countHit = 0;

        private void Start()
        {
            _pointGrid = GameObject.FindGameObjectsWithTag("datapoint").Where(x => x.GetComponent<DatapointControl>().playerIndex == playerIndex).ToArray();
            _ledController = GameObject.FindObjectsOfType<SerialLedController>().First(x => x.playerIndex == playerIndex);
        }

        private void Update()
        {
            // Get instant center of pressure
            float totG_ = 0.0f;   // instant total pressure
            float xG_ = 0f;       // instant X coordinate of center of pressure
            float yG_ = 0f;       // instant Y coordinate of center of pressure;

            for (int i = 0; i < _pointGrid.Length; i++)
            {
                var datapoint = _pointGrid[i];
                var dpc = datapoint.GetComponent<DatapointControl>();
                if (dpc.curDerivVal > this.threshImpact && IsColored(dpc.transform.position))
                {
                    dpc.threshImpact = (int)this.threshImpact;
                    totG_ += dpc.curRemapVal;
                    xG_ += dpc.curRemapVal * datapoint.transform.position.x;
                    yG_ += dpc.curRemapVal * datapoint.transform.position.y;
                    _timerOffHit0 = Time.time;
                }
            }

            // Get current impact
            if (1000 * (Time.time - _timerOffHit0) > this.delayOffHit && _totG != 0f)
            {
                // Get current impact positon
                _xG /= _totG;   // get X coordinate of current impact
                _yG /= _totG;   // get Y coordinate of current impact

                _position = new Vector3(_xG, _yG, 0);
                if (onImpact != null)
                    onImpact(_position, playerIndex);

                _xG = 0;     // reset X coordinate of current impact
                _yG = 0;     // reset Y coordinate of current impact
                _totG = 0;   // reset pressure of current impact

                _countHit++;   // increment number of hit
            }
            else
            {
                _xG += xG_;
                _yG += yG_;
                _totG += totG_;
            }
        }

        private bool IsColored(Vector3 position)
        {
            Vector3 point = _ledController.playerCamera.WorldToScreenPoint(position);
            return _ledController.cameraTexture.GetPixel((int)point.x, (int)point.y) != Color.black;
        }
    }
}