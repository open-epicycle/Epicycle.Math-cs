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

using NUnit.Framework;

namespace Epicycle.Math.Geometry
{
    [TestFixture]
    public sealed class IntervalTest : AssertionHelper
    {
        private const double _tolerance = 1e-9;

        [Test]
        public void Distance2_between_value_and_Interval_is_correct()
        {
            var interval = new Interval(3.14, 5.77);

            Expect(IntervalUtils.Distance2(interval, 4), Is.EqualTo(0).Within(_tolerance));

            Expect(IntervalUtils.Distance2(interval, -1), Is.EqualTo(4.14 * 4.14).Within(_tolerance));
            Expect(IntervalUtils.Distance2(interval, 6), Is.EqualTo(0.23 * 0.23).Within(_tolerance));
        }
    }
}
