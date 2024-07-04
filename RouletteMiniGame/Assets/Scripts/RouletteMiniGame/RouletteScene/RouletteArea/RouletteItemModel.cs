using System;
using RouletteMiniGame.RouletteScene.RouletteArea.RouletteItemStates;
using UnityEngine;

namespace RouletteMiniGame.RouletteScene.RouletteArea
{
    public class RouletteItemModel
    {
        public InventoryType Type { get; private set; }
        public Sprite Sprite { get; private set; }
        public int Index { get; private set; }
        public RouletteItemStateBase State { get; private set; } = new IdleState();
        
        public RouletteItemModel(InventoryType inventoryType, int index)
        {
            Type = inventoryType;
            Index = index;
        }

        public void SetSprite(Sprite sprite)
        {
            Sprite = sprite;
        }
        
        public void ChangeState(RouletteItemStateBase state)
        {
            if (!State.CanChangeState(state.Type))
            {
                throw new Exception($"Tried change state when the state is not idle. Current State: {State}. New State: {state}");
            }
            State = state;
        }
    }
}