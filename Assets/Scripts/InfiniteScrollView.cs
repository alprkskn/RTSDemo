using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfiniteScrollView : MonoBehaviour
{
    public class Element
    {
        public RectTransform Body;
        public string Content;
        public int ContentType;
    }

    public const float ElementSize = 32f;

    public RectTransform Content;
    public GameObject ElementPrefab;

    public Vector2 Spacing;

    public int BufferRowCount; // How many rows will be kept above and below the bounds as a buffer.

    private List<Element> _elements;
    private List<string> _availableContentList;
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
    void Start()
    {
        _availableContentList = new List<string>() { "LOL", "BIR", "KI", "UC" };
        _elements = new List<Element>();
        _topOffset = BufferRowCount * (ElementSize + Spacing.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (AdjustGrid())
        {
            RearrangeElements();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OffsetElements((Input.GetKey(KeyCode.LeftShift) ? -1 : 1) * 512);
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
                var rectTransform = element.GetComponent<RectTransform>();
                rectTransform.position = new Vector2(alignmentOffset + i * (ElementSize + Spacing.x), heightCursor);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ElementSize);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ElementSize);
                rectTransform.SetParent(Content, false);

                element.GetComponentInChildren<Text>().text = _availableContentList[elementTypeCursor];

                _elements.Add(new Element()
                {
                    Body = rectTransform,
                    Content = _availableContentList[elementTypeCursor],
                    ContentType = elementTypeCursor
                });

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

        foreach (var element in _elements)
        {
            element.Body.Translate(0, yOffset, 0);
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
                elementCursor = _elements.Count - _currentGridWidth * replacedRows;

                // if yOffset is negative, meaning its a scroll down
                // new element's cursor is a bit tricky to find due
                // to the dynamic nature of the grid width.
                // Otherwise we keep using the cursor as where it was.
                elementTypeCursor = _elements[0].ContentType - _currentGridWidth * replacedRows;
                while (elementTypeCursor < 0) elementTypeCursor += _availableContentList.Count;
            }
            else
            {
                elementTypeCursor = (_elements[_elements.Count - 1].ContentType + 1) % _availableContentList.Count;
            }

            var rowElements = _elements.GetRange(elementCursor, _currentGridWidth * replacedRows);
            _elements.RemoveRange(elementCursor, _currentGridWidth * replacedRows);

            int rowInProgress = 0;
            while (replacedRows > 0)
            {
                // Set cursor to the end of the row to be removed.
                for (int i = 0; i < _currentGridWidth; i++)
                {
                    //var cursor = (yOffset < 0) ? _elements.Count - 1 : elementCursor - i;
                    var element = rowElements[_currentGridWidth * rowInProgress + i];

                    element.Body.anchoredPosition = new Vector2(alignmentOffset + i * (ElementSize + Spacing.x), heightCursor);
                    element.Body.SetParent(Content, false);

                    element.Body.GetComponentInChildren<Text>().text = _availableContentList[elementTypeCursor];
                    elementTypeCursor = (elementTypeCursor + 1) % _availableContentList.Count;

                    //_elements.RemoveAt(cursor);
                    _elements.Insert((yOffset < 0) ? (rowInProgress * _currentGridWidth) + i : _elements.Count, element);
                }

                replacedRows--;
                heightCursor -= (int)(ElementSize + Spacing.y);
                rowInProgress++;
                //_topOffset += ((yOffset < 0) ? 1 : -1) * (ElementSize + Spacing.y);
            }
        }
    }
}
