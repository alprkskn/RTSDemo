using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScrollEntry : MonoBehaviour
{
    private RectTransform _transformHandle;

    public RectTransform TransformHandle
    {
        get { return _transformHandle; }
    }

    public InfiniteScrollView.Element Element;

    private Image _thumbnail;

    // Use this for initialization
    void Awake()
    {
        _transformHandle = GetComponent<RectTransform>();
        _thumbnail = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize()
    {
        // TODO: Set Image and callback func.
        _thumbnail.sprite = Element.Content;
    }
}
