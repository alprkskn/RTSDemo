using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace RTSDemo
{
    public delegate void UnitProductionEventHandler(Type product, IInfoPanelElement entity);

    public class InformationPanel : MonoBehaviour
    {
        public event UnitProductionEventHandler UnitProduced;

        [SerializeField] private Text _entityLabel;
        [SerializeField] private Image _entityImage;
        [SerializeField] private RectTransform _productionPanel;
        [SerializeField] private GameObject _productEntryPrefab;

        private List<Button> _productionButtons;

        public void SetEntity(IInfoPanelElement entity)
        {
            clearButtons();
            if (entity == null)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                this.gameObject.SetActive(true);
                _entityLabel.text = entity.GetInfoTitle();
                _entityImage.sprite = entity.GetThumbnailImage();

                _productionPanel.gameObject.SetActive(entity.HasProduction());

                if (entity.HasProduction())
                {
                    _productionButtons = new List<Button>();
                    foreach (var product in entity.GetProductList())
                    {
                        var go = Instantiate(_productEntryPrefab);
                        go.GetComponent<RectTransform>().SetParent(_productionPanel, false);

                        // Getting name by removing "Model" from the typename.
                        // Again, no error check.
                        go.GetComponent<Image>().sprite =
                            ResourcesManager.Instance.GetSprite(product.Name.Substring(0, product.Name.Length - 5));
                        var button = go.GetComponent<Button>();

                        Type type = product;

                        button.onClick.AddListener(() =>
                        {
                            if (UnitProduced != null)
                            {
                                UnitProduced(type, entity);
                            }
                            EventSystem.current.SetSelectedGameObject(null);
                        });

                        _productionButtons.Add(button);
                    }
                }
            }
        }

        private void clearButtons()
        {
            if (_productionButtons != null)
            {
                foreach (var productionButton in _productionButtons)
                {
                    Destroy(productionButton.gameObject);
                }

                _productionButtons = null;
            }
        }


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}