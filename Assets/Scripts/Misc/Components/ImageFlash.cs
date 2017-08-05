using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTSDemo.Misc.Components
{
    /// <summary>
    /// A simple, stupid image flash component to be used on
    /// views or other misc GameObjects to be used on the Game Board.
    /// </summary>
    public class ImageFlash : MonoBehaviour
    {
        public AnimationCurve flashPattern;

        public float flashDuration;

        private float currentTime;
        private Image _image;

        void Start()
        {
            _image = GetComponent<Image>();
            currentTime = 0f;

            Color clr = _image.color;
            clr.a = 0;
            _image.color = clr;
        }

        void Update()
        {
            currentTime += Time.deltaTime;

            currentTime = currentTime % flashDuration;

            Color clr = _image.color;
            clr.a = flashPattern.Evaluate(currentTime / flashDuration);
            _image.color = clr;
        }
    }
}