using System;
using System.Collections;
using System.Collections.Generic;
using RTSDemo;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RTSDemo
{
    public class InfiniteScrollEntry : MonoBehaviour, IPoolable
    {
        public event Action<InfiniteScrollEntry> Selected;
        public event Action<PointerEventData> ViewDragged;

        private EventTrigger _eventTrigger;
        private GameObject _gameObject;
        private RectTransform _transformHandle;

        private RectTransform _tooltipRect;
        private GameObject _tooltipGO;
        private Text _tooltipText;

        private bool _updateTooltip = false;

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

            _eventTrigger = GetComponent<EventTrigger>();

            if (_eventTrigger == null)
            {
                _eventTrigger = gameObject.AddComponent<EventTrigger>();
            }

            _gameObject = this.gameObject;
            _transformHandle = GetComponent<RectTransform>();
            _thumbnail = _transformHandle.Find("Image").GetComponent<Image>();
            _button = GetComponent<Button>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_updateTooltip)
            {
                UpdateTooltipPosition();
            }

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

            _eventTrigger.triggers.Clear();

            var ptrEnterEvent = new EventTrigger.TriggerEvent();
            var ptrExitEvent = new EventTrigger.TriggerEvent();
            var ptrDragEvent = new EventTrigger.TriggerEvent();


            var enterAction = new UnityAction<BaseEventData>(OnPointerEnter);
            var exitAction = new UnityAction<BaseEventData>(OnPointerExit);
            var dragAction = new UnityAction<BaseEventData>(OnPointerDrag);

            ptrEnterEvent.AddListener(enterAction);
            ptrExitEvent.AddListener(exitAction);
            ptrDragEvent.AddListener(dragAction);

            var triggerList = new List<EventTrigger.Entry>()
            {
                new EventTrigger.Entry()
                {
                    eventID = EventTriggerType.PointerEnter,
                    callback = ptrEnterEvent
                },
                new EventTrigger.Entry()
                {
                    eventID = EventTriggerType.PointerExit,
                    callback = ptrExitEvent
                },
                new EventTrigger.Entry()
                {
                    eventID = EventTriggerType.Drag,
                    callback = ptrDragEvent
                }
            };

            _eventTrigger.triggers = triggerList;
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

        public void RegisterTooltip(RectTransform rt, Text text)
        {
            _tooltipRect = rt;
            _tooltipGO = rt.gameObject;
            _tooltipText = text;
        }

        public void OnPointerEnter(BaseEventData e)
        {
            PointerEventData ptrEvent = (PointerEventData) e;

            var typename = Element.RepresentedType.Name;
            _tooltipText.text = typename.Substring(0, typename.Length - "Model".Length);

            _tooltipGO.SetActive(true);
            _updateTooltip = true;
        }

        public void OnPointerExit(BaseEventData e)
        {
            PointerEventData ptrEvent = (PointerEventData) e;
            _updateTooltip = false;
            _tooltipGO.SetActive(false);
        }

        public void OnPointerDrag(BaseEventData e)
        {
            PointerEventData ptrEvent = (PointerEventData) e;

            if (ViewDragged != null)
            {
                ViewDragged.Invoke(ptrEvent);
            }
        }

        private void UpdateTooltipPosition()
        {
            _tooltipRect.position = Input.mousePosition + new Vector3(1, -1, 0) * 10f;

            //var pos = ptrEvent.position;
            //pos.y -= -1;
            //_tooltipRect.anchoredPosition = pos;
        }
    }
}