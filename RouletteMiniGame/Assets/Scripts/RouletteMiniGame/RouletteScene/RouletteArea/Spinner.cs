using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RouletteMiniGame.RouletteScene.RouletteArea
{
    public class Spinner : IDisposable
    {
        private readonly CancellationTokenSource _lifetimeCts = new();

        public async UniTask<int> SpinAsync(List<RouletteItemPresenter> rouletteItems, float initialSpinSpeed,
            Vector2Int minLoopRange, float decelerationRate)
        {
            var speed = initialSpinSpeed;
            var totalBoxes = rouletteItems.Count;

            var (finalBoxIndex, possibleIndexCount) = GetRandomBoxIndexExcludingPrevious(rouletteItems, totalBoxes);
            Debug.Log("Final box index is: " + finalBoxIndex);

            var loopCount = possibleIndexCount == 1 ? 0 : UnityEngine.Random.Range(minLoopRange.x, minLoopRange.y);
            var totalIterations = totalBoxes * loopCount + finalBoxIndex;
            for (var i = 0; i < totalIterations; i++)
            {
                var currentBoxIndex = i % totalBoxes;
                rouletteItems[currentBoxIndex].HighlightItemAsync().Forget();
                await UniTask.Delay(TimeSpan.FromSeconds(speed), cancellationToken: _lifetimeCts.Token);
                speed += decelerationRate;
            }

            await rouletteItems[finalBoxIndex].SelectItemAsync();
            return finalBoxIndex;
        }
        
        private static (int, int) GetRandomBoxIndexExcludingPrevious(List<RouletteItemPresenter> rouletteItems, int totalCount)
        {
            var possibleIndices = new List<int>();
            for (var i = 0; i < totalCount; i++)
            {
                if (!rouletteItems[i].IsCollected())
                {
                    possibleIndices.Add(i);
                }
            }

            // Randomly select a new box index from the remaining possible indices
            var newBoxIndex = possibleIndices[UnityEngine.Random.Range(0, possibleIndices.Count)];
            return (newBoxIndex, possibleIndices.Count);
        }

        public void Dispose()
        {
            _lifetimeCts.Cancel();
        }
    }
}