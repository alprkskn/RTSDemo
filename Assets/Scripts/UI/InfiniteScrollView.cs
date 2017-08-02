using System;
using System.Collections;
using System.Collections.Generic;
using RTSDemo;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfiniteScrollView : MonoBehaviour
{
    public event Action<Element> ElementSelected; 

    public class Element
    {
        public Sprite Content;
        public Type RepresentedType;
        public int ContentType;
    }

    public const float ElementSize = 64f;

    public RectTransform Content;

    public GameObject ElementPrefab;

    public Vector2 Spacing;

    public int BufferRowCount; // How many rows will be kept above and below the bounds as a buffer.

    private List<InfiniteScrollEntry> _entries;
    private List<Element> _availableContentList;
    private int _currentGridWidth = 0;
    private int _currentRowCount = 0;

    private float _topOffset = 0;

    public void OnDrag(BaseEventData e)
    {
        PointerEventData ptrEvent = (PointerEventData)e;

        if (ptrEvent != null)
        {
            OffsetElements(ptrEvent.delta.y);
        }
    }

    // Use this for initialization
    void Awake()
    {
        _availableContentList = new List<Element>();
        _entries = new List<InfiniteScrollEntry>();
        _topOffset = BufferRowCount * (ElementSize + Spacing.y);
    }

    public void SetAvailableElements(Dictionary<Type, Sprite> elements)
    {
        int index = 0;
        foreach (var pair in elements)
        {
            _availableContentList.Add(new Element()
            {
                Content = pair.Value,
                RepresentedType = pair.Key,
                ContentType = index++
            });
        }

        RearrangeElements();
    }

    // Update is called once per frame
    void Update()
    {
        if (AdjustGrid())
        {
            RearrangeElements();
        }
    }

    bool AdjustGrid()
    {
        float currentWidth = Content.rect.width;

        int gridWidth = (int)((currentWidth) / (ElementSize + Spacing.x));

        if (gridWidth != _currentGridWidth)
        {
            _currentGridWidth = gridWidth;

            return true;
        }

        return false;
    }

    void RearrangeElements()
    {

        if (_entries.Count > 0)
        {
            foreach (var entry in _entries)
            {
                Destroy(entry.gameObject);
            }

            _entries.Clear();
        }

        int heightCursor = (int)_topOffset;
        int height = (int)Content.rect.height;
        int width = (int)Content.rect.width;

        int alignmentOffset = (int)((width - (_currentGridWidth * (ElementSize + Spacing.x))) / 2f); // To align element objects horizontally to middle of the panel.

        int elementTypeCursor = 0;
        while (heightCursor > -height - BufferRowCount * (ElementSize + Spacing.y))
        {
            for (int i = 0; i < _currentGridWidth; i++)
            {
                var element = Instantiate<GameObject>(ElementPrefab); // TODO: Instantiations will use an ObjectPooling Mechanism.

                var entry = element.GetComponent<InfiniteScrollEntry>();
                entry.TransformHandle.anchoredPosition = new Vector2(alignmentOffset + i * (ElementSize + Spacing.x), heightCursor);
                entry.TransformHandle.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ElementSize);
                entry.TransformHandle.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ElementSize);
                entry.TransformHandle.SetParent(Content, false);
                entry.Element = _availableContentList[elementTypeCursor];
                entry.Initialize();
                entry.Selected += (e) =>
                {
                    if (ElementSelected != null)
                    {
                        ElementSelected(e.Element);
                    }
                };

                _entries.Add(entry);

                elementTypeCursor = (elementTypeCursor + 1) % _availableContentList.Count;
            }
            heightCursor -= (int)(ElementSize + Spacing.y);
        }
        _currentRowCount = (int)((Mathf.Floor((height / (ElementSize + Spacing.y))) + 2 * BufferRowCount));
    }

    void OffsetElements(float yOffset)
    {
        var rect = Content.rect;
        yOffset %= rect.height;

        foreach (var element in _entries)
        {
            //element.TransformHandle.Translate(0, yOffset, 0);
            var pos = element.TransformHandle.anchoredPosition; //Translate(0, yOffset, 0);
            pos.y += yOffset;
            element.TransformHandle.anchoredPosition = pos;
        }

        _topOffset += yOffset;
        float _bottomOffset = _topOffset - _currentRowCount * (ElementSize + Spacing.y);


        int replacedRows = 0;

        if (yOffset > 0 && _topOffset >= BufferRowCount * (ElementSize + Spacing.y))
        {
            while (_topOffset > BufferRowCount * (ElementSize + Spacing.y))
            {
                _topOffset -= (ElementSize + Spacing.y);
                _bottomOffset -= (ElementSize + Spacing.y);
                replacedRows++;
            }
        }
        else if (yOffset < 0 && _bottomOffset <= -rect.height - BufferRowCount * (ElementSize + Spacing.y))
        {
            while (_bottomOffset < -rect.height - BufferRowCount * (ElementSize + Spacing.y))
            {
                _topOffset += (ElementSize + Spacing.y);
                _bottomOffset += (ElementSize + Spacing.y);
                replacedRows++;
            }
        }

        if (replacedRows > 0)
        {
            int alignmentOffset = (int)((rect.width - (_currentGridWidth * (ElementSize + Spacing.x))) / 2f);

            int heightCursor = (int)((yOffset > 0) ? _bottomOffset + (replacedRows - 1) * (ElementSize + Spacing.y) : _topOffset);

            int elementCursor = 0;
            int elementTypeCursor = 0;

            if (yOffset < 0)
            {
                elementCursor = _entries.Count - _currentGridWidth * replacedRows;

                // if yOffset is negative, meaning its a scroll down
                // new element's cursor is a bit tricky to find due
                // to the dynamic nature of the grid width.
                // Otherwise we keep using the cursor as where it was.
                elementTypeCursor = _entries[0].Element.ContentType - _currentGridWidth * replacedRows;
                while (elementTypeCursor < 0) elementTypeCursor += _availableContentList.Count;
            }
            else
            {
                elementTypeCursor = (_entries[_entries.Count - 1].Element.ContentType + 1) % _availableContentList.Count;
            }

            var rowElements = _entries.GetRange(elementCursor, _currentGridWidth * replacedRows);
            _entries.RemoveRange(elementCursor, _currentGridWidth * replacedRows);

            int rowInProgress = 0;
            while (replacedRows > 0)
            {
                // Set cursor to the end of the row to be removed.
                for (int i = 0; i < _currentGridWidth; i++)
                {
                    //var cursor = (yOffset < 0) ? _entries.Count - 1 : elementCursor - i;
                    var entry = rowElements[_currentGridWidth * rowInProgress + i];

                    entry.TransformHandle.anchoredPosition = new Vector2(alignmentOffset + i * (ElementSize + Spacing.x), heightCursor);
                    entry.TransformHandle.SetParent(Content, false);

                    // TODO: Reset the entries visuals.
                    entry.Initialize();
                    entry.Selected += (e) =>
                    {
                        if (ElementSelected != null)
                        {
                            ElementSelected(e.Element);
                        }
                    };


                    //element.TransformHandle.GetComponentInChildren<Text>().text = _availableContentList[elementTypeCursor];
                    elementTypeCursor = (elementTypeCursor + 1) % _availableContentList.Count;

                    //_entries.RemoveAt(cursor);
                    _entries.Insert((yOffset < 0) ? (rowInProgress * _currentGridWidth) + i : _entries.Count, entry);
                }

                replacedRows--;
                heightCursor -= (int)(ElementSize + Spacing.y);
                rowInProgress++;
                //_topOffset += ((yOffset < 0) ? 1 : -1) * (ElementSize + Spacing.y);
            }
        }
    }
}
