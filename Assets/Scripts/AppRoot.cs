﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTSDemo
{
    // This is a simple singleton to behave as a single
    // entry point to the game.
    public class AppRoot : Singleton<AppRoot>
    {
        [SerializeField] private GameObject _canvasPrefab;
        // Whole window is rendered by a single canvas.
        // This will keep the Transform of that.
        private RectTransform _canvasTransform;
        private Canvas _canvas;
        private GameView _gameView;

        public Canvas Canvas
        {
            get { return _canvas; }
        }

        public RectTransform CanvasTransform
        {
            get { return _canvasTransform; }
        }

        public GameView GameView
        {
            get { return _gameView; }
        }


        private Dictionary<string, ControllerBase> _controllerDict;


        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            _controllerDict = new Dictionary<string, ControllerBase>();

            // Populate the _controllerDict
            foreach (var controller in this.GetComponentsInChildren<ControllerBase>())
            {
                var baseName = controller.BaseName;

                if (baseName == "")
                    continue;

                _controllerDict.Add(baseName, controller);
            }

            // Initialize the canvas
            _canvasTransform = Instantiate(_canvasPrefab).GetComponent<RectTransform>();
            _canvasTransform.name = "CanvasTransform";
            _canvas = _canvasTransform.GetComponent<Canvas>();
        }

        // Since application will immediately start the game
        // This callback method could be used for the initialization.
        void Start()
        {
            // Exclusive to this class. I'm creating Game Model and View manually.
            // Since these will be created only once. There is no need for other means.
            GameModel game = new GameModel();

            // View should be portrait
            if (Screen.height > Screen.width * 1.5f)
            {
                _canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(540, 960);
                _gameView = ViewFactory.CreateViewForModel<GamePortraitView>(game);
            }
            else
            {
                _canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(680, 540);
                _gameView = ViewFactory.CreateViewForModel<GameView>(game);
            }
            _gameView.transform.SetParent(_canvasTransform, false);

            game.MapSize = new Vector2(40, 40);
            game.Units = new List<UnitModel>();
            game.Buildings = new List<BuildingModel>();
            game.AvailableBuildingTypes = new List<Type>()
            {
                typeof(BarracksModel), typeof(PowerPlantModel),
                typeof(CommandCenterModel), typeof(SupplyCenterModel),
                typeof(HouseModel)
            };


        }

        void Update()
        {

        }

        public ControllerBase GetController(string controllerName)
        {
            if (_controllerDict.ContainsKey(controllerName))
            {
                return _controllerDict[controllerName];
            }

            return null;
        }
    }
}