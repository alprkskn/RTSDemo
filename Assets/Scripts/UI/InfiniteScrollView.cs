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

    private bool _landscape;

    public bool Landscape
    {
        get { return _landscape; }
        set
        {
            _landscape = value;
            RearrangeElements();
        }
    }

    public const float ElementSize = 64;
    public RectTransform Content;
    public GameObject ElementPrefab;
    public Vector2 Spacing;
    public int BufferLineCount; // How many rows will be kept above and below the bounds as a buffer.

    private ObjectPool<InfiniteScrollEntry> _entryPool;
    private List<InfiniteScrollEntry> _entries;
    private List<Element> _availableContentList;
    private int _currentGridWidth = 0;
    private int _currentGridHeight = 0;
    private int _currentLineCount = 0;
    private float _topOffset = 0;
    private bool _initialized = false;

    private RectTransform _tooltipRT;
    private Text _tooltipText;

    public void OnDrag(BaseEventData e)
    {
        PointerEventData ptrEvent = (PointerEventData)e;

        if (ptrEvent != null)
        {
            if (_landscape)
            {
                OffsetElements(ptrEvent.delta.y);
            }
            else
            {
                OffsetElements(ptrEvent.delta.x);
            }
        }
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

    void Awake()
    {
        if(!_initialized)
            Initialize();
    }

    void Initialize()
    {
        _availableContentList = new List<Element>();
        _entries = new List<InfiniteScrollEntry>();

        // Using arbitrary sizes here.
        _entryPool = new ObjectPool<InfiniteScrollEntry>(64, CreateEntry, 16);
        _initialized = true;
    }

    void Update()
    {
        if (AdjustGrid())
        {
            RearrangeElements();
        }
    }

    /// <summary>
    /// Decides the width and height of the grid
    /// in number of entries.
    /// </summary>
    /// <returns>Returns true if grid entry count is changed, false otherwise.</returns>
    bool AdjustGrid()
    {
        float currentWidth = Content.rect.width;
        float currentHeight = Content.rect.height;

        int gridWidth = (int)((currentWidth) / (ElementSize + Spacing.x));
        int gridHeight = (int)((currentHeight) / (ElementSize + Spacing.y));

        if (gridWidth != _currentGridWidth || gridHeight != _currentGridHeight)
        {
            _currentGridWidth = gridWidth;
            _currentGridHeight = gridHeight;

            return true;
        }

        return false;
    }

    InfiniteScrollEntry CreateEntry()
    {
        var entry = Instantiate<GameObject>(ElementPrefab);
        return entry.GetComponent<InfiniteScrollEntry>();
    }

    public void RegisterTooltipObject(RectTransform tooltip, Text text)
    {
        _tooltipRT = tooltip;
        _tooltipText = text;
    }

    /// <summary>
    /// Creates grid entries.
    /// Destroys any previous entry.
    /// </summary>
    void RearrangeElements()
    {
        if(!_initialized)
            Initialize();

        if (_entries.Count > 0)
        {
            foreach (var entry in _entries)
            {
                Destroy(entry.gameObject);
            }

            _entries.Clear();
        }

        if (_landscape)
        {
            _topOffset = BufferLineCount * (ElementSize + Spacing.y);
        }
        else
        {
            _topOffset = - BufferLineCount * (ElementSize + Spacing.x);
        }

        int lineCursor = (int)_topOffset;
        int height = (int)Content.rect.height;
        int width = (int)Content.rect.width;

        int alignmentOffset = 0;

        if (_landscape)
        {
            // To align element objects horizontally to middle of the panel.
            alignmentOffset = (int) ((width - (_currentGridWidth * (ElementSize + Spacing.x))) / 2f);
        }
        else
        {
            alignmentOffset = (int) ((height - (_currentGridHeight * (ElementSize + Spacing.y))) / 2f);
        }


        int elementTypeCursor = 0;

        if (_landscape)
        {
            while (lineCursor > -height - BufferLineCount * (ElementSize + Spacing.y))
            {
                for (int i = 0; i < _currentGridWidth; i++)
                {
                    var entry = _entryPool.GetObject();
                    entry.RegisterTooltip(_tooltipRT, _tooltipText);
                    entry.TransformHandle.anchoredPosition =
                        new Vector2(alignmentOffset + i * (ElementSize + Spacing.x), lineCursor);
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

                    entry.ViewDragged += OnDrag;

                    _entries.Add(entry);

                    elementTypeCursor = (elementTypeCursor + 1) % _availableContentList.Count;
                }
                lineCursor -= (int) (ElementSize + Spacing.y);
            }
        }
        else
        {
            while (lineCursor < width + BufferLineCount * (ElementSize + Spacing.x))
            {
                for (int i = 0; i < _currentGridHeight; i++)
                {
                    var entry = _entryPool.GetObject();
                    entry.RegisterTooltip(_tooltipRT, _tooltipText);
                    entry.TransformHandle.anchoredPosition =
                        new Vector2(lineCursor, -alignmentOffset - i * (ElementSize + Spacing.y));
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
                    entry.ViewDragged += OnDrag;

                    _entries.Add(entry);

                    elementTypeCursor = (elementTypeCursor + 1) % _availableContentList.Count;
                }
                lineCursor += (int) (ElementSize + Spacing.x);
            }
            
        }

        if (_landscape)
        {
            _currentLineCount = (int) ((Mathf.Floor((height / (ElementSize + Spacing.y))) + 2 * BufferLineCount));
        }
        else
        {
            _currentLineCount = (int) ((Mathf.Floor((width / (ElementSize + Spacing.x))) + 2 * BufferLineCount));
        }
    }


    /// <summary>
    /// Offsets the current elements by given offset.
    /// </summary>
    /// <param name="offset"></param>
    void OffsetElements(float offset)
    {
        var rect = Content.rect;

        offset %= (_landscape) ? rect.height : rect.width;

        foreach (var element in _entries)
        {
            //element.TransformHandle.Translate(0, yOffset, 0);
            var pos = element.TransformHandle.anchoredPosition; //Translate(0, yOffset, 0);

            if (_landscape)
            {
                pos.y += offset;
            }
            else
            {
                pos.x += offset;
            }

            element.TransformHandle.anchoredPosition = pos;
        }

        _topOffset += offset;

        float _bottomOffset = (_landscape) ? _topOffset - _currentLineCount * (ElementSize + Spacing.y) :
            _topOffset + _currentLineCount * (ElementSize + Spacing.x);


        int replacedRows = 0;


        if (_landscape)
        {
            if (offset > 0 && _topOffset >= BufferLineCount * (ElementSize + Spacing.y))
            {
                while (_topOffset > BufferLineCount * (ElementSize + Spacing.y))
                {
                    _topOffset -= (ElementSize + Spacing.y);
                    _bottomOffset -= (ElementSize + Spacing.y);
                    replacedRows++;
                }
            }
            else if (offset < 0 && _bottomOffset <= -rect.height - BufferLineCount * (ElementSize + Spacing.y))
            {
                while (_bottomOffset < -rect.height - BufferLineCount * (ElementSize + Spacing.y))
                {
                    _topOffset += (ElementSize + Spacing.y);
                    _bottomOffset += (ElementSize + Spacing.y);
                    replacedRows++;
                }
            }
        }
        else
        {
            if (offset > 0 && _bottomOffset >= rect.width + BufferLineCount * (ElementSize + Spacing.x))
            {
                while (_bottomOffset > rect.width + BufferLineCount * (ElementSize + Spacing.x))
                {
                    _topOffset -= (ElementSize + Spacing.x);
                    _bottomOffset -= (ElementSize + Spacing.x);
                    replacedRows++;
                }
            }
            else if (offset < 0 && _topOffset <= -BufferLineCount * (ElementSize + Spacing.x))
            {
                while (_topOffset < -BufferLineCount * (ElementSize + Spacing.x))
                {
                    _topOffset += (ElementSize + Spacing.x);
                    _bottomOffset += (ElementSize + Spacing.x);
                    replacedRows++;
                }
            }
            
        }

        if (replacedRows > 0)
        {
            int alignmentOffset = 0;
            int cursor = 0;

            if (_landscape)
            {
                alignmentOffset = (int) ((rect.width - (_currentGridWidth * (ElementSize + Spacing.x))) / 2f);
                cursor = (int) ((offset > 0) ? _bottomOffset + (replacedRows - 1) * (ElementSize + Spacing.y) : _topOffset);
            }
            else
            {
                alignmentOffset = (int) ((rect.height - (_currentGridHeight * (ElementSize + Spacing.y))) / 2f);
                cursor = (int) ((offset > 0) ? _topOffset : _bottomOffset - (replacedRows - 1) * (ElementSize + Spacing.x));
            }

            int elementCursor = 0;
            int elementTypeCursor = 0;

            if (offset < 0)
            {
                if (_landscape)
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
                    elementTypeCursor = (_entries[_entries.Count - 1].Element.ContentType + 1) %
                                        _availableContentList.Count;
                }
            }
            else
            {
                if (_landscape)
                {
                    elementTypeCursor = (_entries[_entries.Count - 1].Element.ContentType + 1) %
                                        _availableContentList.Count;
                }
                else
                {
                    elementCursor = _entries.Count - _currentGridHeight * replacedRows;
                    elementTypeCursor = _entries[0].Element.ContentType - _currentGridHeight * replacedRows;
                    while (elementTypeCursor < 0) elementTypeCursor += _availableContentList.Count;
                }
            }

            int gridCount = (_landscape) ? _currentGridWidth : _currentGridHeight;
            var lineElements = _entries.GetRange(elementCursor, gridCount * replacedRows);
            _entries.RemoveRange(elementCursor, gridCount * replacedRows);


            int rowInProgress = 0;
            while (replacedRows > 0)
            {
                // Set cursor to the end of the row to be removed.
                for (int i = 0; i < gridCount; i++)
                {
                    var entry = lineElements[gridCount * rowInProgress + i];

                    // Release the current entry.
                    entry.ViewDragged -= OnDrag;
                    _entryPool.ReleaseObject(entry);


                    // Get a new entry from the pool.
                    entry = _entryPool.GetObject();
                    entry.ViewDragged += OnDrag;

                    entry.RegisterTooltip(_tooltipRT, _tooltipText);
                    entry.TransformHandle.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ElementSize);
                    entry.TransformHandle.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ElementSize);
                    entry.TransformHandle.anchoredPosition = (_landscape) ? 
                        new Vector2(alignmentOffset + i * (ElementSize + Spacing.x), cursor) :
                        new Vector2(cursor, -alignmentOffset - i * (ElementSize + Spacing.y));
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


                    elementTypeCursor = (elementTypeCursor + 1) % _availableContentList.Count;
                    if (_landscape)
                    {
                        _entries.Insert((offset < 0) ? (rowInProgress * gridCount) + i : _entries.Count, entry);
                    }
                    else
                    {
                        _entries.Insert((offset < 0) ? _entries.Count : (rowInProgress * gridCount) + i, entry);
                    }
                }

                replacedRows--;

                if (_landscape)
                {
                    cursor -= (int) (ElementSize + Spacing.y);
                }
                else
                {
                    cursor += (int) (ElementSize + Spacing.x);
                }

                rowInProgress++;
            }
        }
    }
}
