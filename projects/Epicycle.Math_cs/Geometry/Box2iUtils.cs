// [[[[INFO>
// Copyright 2015 Epicycle (http://epicycle.org, https://github.com/open-epicycle)
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// For more information check https://github.com/open-epicycle/Epicycle.Math-cs
// ]]]]

namespace Epicycle.Math.Geometry
{
    public static class Box2iUtils
    {
        public enum BoxFitMode
        {
            PreserveRatio,
            Stretch,
        }

        public static Box2i FitBox(Vector2i area, Vector2i areaToFit, BoxFitMode fitMode)
        {
            if ((area.X <= 0) || (area.Y <= 0))
            {
                return Box2i.Empty;
            }

            var boxDimensions = FindBoxDimensions(area, areaToFit, fitMode);
            var offset = (area - boxDimensions) / 2;

            return new Box2i(offset, boxDimensions);
        }

        private static Vector2i FindBoxDimensions(Vector2i area, Vector2i areaToFit, BoxFitMode fitMode)
        {
            var boxWidth = area.X;
            var boxHeight = area.Y;

            if ((areaToFit.X > 0) && (areaToFit.Y > 0) && (fitMode != BoxFitMode.Stretch))
            {
                var areaRatio = ((double)area.X) / ((double)area.Y);
                var areaToFitRatio = ((double)areaToFit.X) / ((double)areaToFit.Y);

                if (areaRatio > areaToFitRatio)
                {
                    boxWidth = (int)System.Math.Round(area.Y * areaToFitRatio);
                }
                else
                {
                    boxHeight = (int)System.Math.Round(area.X / areaToFitRatio);
                }
            }

            return new Vector2i(boxWidth, boxHeight);
        }
    }
}
