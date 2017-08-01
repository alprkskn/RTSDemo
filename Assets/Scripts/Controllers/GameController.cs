using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    public class GameController : ControllerBase
    {
        #region PrivateFields

        private ISelectable _currentSelection = null;

        #endregion
        public override void InitModelView(ModelBase model, ViewBase view)
        {
            base.InitModelView(model, view);

            GameView gameView = (GameView) view;

            gameView.BoardClickRegistered += OnBoardClickRegistered;
        }

        private void OnBoardClickRegistered(GameView sender, int coordX, int coordY, UnityEngine.EventSystems.PointerEventData.InputButton btn)
        {
            var model = (GameModel) this._viewToModel[sender];

            foreach (var selectable in model.Selectables)
            {
                // Check given coords with Rectangles of selectables.
                if (selectable.CheckOverlap(coordX, coordY))
                {
                    if (_currentSelection != null)
                    {
                        _currentSelection.OnDeSelection();
                    }

                    selectable.OnSelection();
                    _currentSelection = selectable;
                    break;
                }
            }

        }
    }
}