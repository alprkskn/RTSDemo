using System;
using System.Collections;
using System.Collections.Generic;
using RTSDemo;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RTSDemo
{
    public class InfiniteScrollEntry : MonoBehaviour, IPoolable
    {
        public event Action<InfiniteScrollEntry> Selected;

        private GameObject _gameObject;
        private RectTransform _transformHandle;

        public RectTransform TransformHandle
        {
            get { return _transformHandle; }
        }

        public InfiniteScrollView.Element Element;

        private Image _thumbnail;
        private Button _button;


        // Use this for initialization
        void Awake()
        {
            _gameObject = this.gameObject;
            _transformHandle = GetComponent<RectTransform>();
            _thumbnail = _transformHandle.Find("Image").GetComponent<Image>();
            _button = GetComponent<Button>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Initialize()
        {
            _thumbnail.sprite = Element.Content;

            // Clear old listeners.
            Selected = null;
            _button.onClick.RemoveAllListeners();

            _button.onClick.AddListener(() =>
            {
                Debug.Log(Element.RepresentedType.Name);
                EventSystem.current.SetSelectedGameObject(null);
                if (Selected != null)
                {
                    Selected(this);
                }
            });
        }

        public void OnReturnToPool(Transform poolParent)
        {
            _gameObject.SetActive(false);
            _transformHandle.SetParent(poolParent, false);
        }

        public void OnGetFromPool()
        {
            _gameObject.SetActive(true);
        }
    }
}