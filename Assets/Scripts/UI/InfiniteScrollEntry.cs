using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfiniteScrollEntry : MonoBehaviour
{
    public event Action<InfiniteScrollEntry> Selected; 

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
        // TODO: Set Image and callback func.
        _thumbnail.sprite = Element.Content;
        _button.onClick.RemoveAllListeners();
        
        // Clear old listeners.
        Selected = null;

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
}
