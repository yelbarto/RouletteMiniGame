using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using RouletteMiniGame.RouletteScene.RouletteArea.RouletteItemStates;
using UnityEngine;

namespace RouletteMiniGame.RouletteScene.RouletteArea
{
    public class RouletteItemModel
    {
        public InventoryType Type { get; private set; }
        public Sprite Sprite { get; private set; }
        public int Index { get; private set; }
        public RouletteItemStateBase State { get; private set; }
        public RouletteItemModel(InventoryType inventoryType, int index)
        {
            Type = inventoryType;
            Index = index;
        }

        public void SetSprite(Sprite sprite)
        {
            Sprite = sprite;
        }
        
        public async UniTask ChangeState(RouletteItemStateBase state, CancellationToken token)
        {
            if (State != null && !State.CanChangeState(state.Type))
            {
                throw new Exception($"Tried change state when the state is not idle. Current State: {State}. New State: {state}");
            }
            State = state;
            await State.EnterState(token);
        }
    }
}