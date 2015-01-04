using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace Epicycle.Math.Geometry
{
    public sealed class Vector2Test : AssertionHelper
    {
        [Test]
        public void Ccw_returns_true_on_ccw_Vector2_triples()
        {
            var u = new Vector2(1, 0);
            var v = new Vector2(0, 1);
            var w = new Vector2(-1, -1);

            Expect(Vector2Utils.AreCcw(u, v, w));

            u = new Vector2(0.1, 1);
            v = new Vector2(0, 1);
            w = new Vector2(-0.1, 1);

            Expect(Vector2Utils.AreCcw(u, v, w));
        }

        [Test]
        public void Ccw_returns_false_on_cw_Vector2_triples()
        {
            var u = new Vector2(1, 0);
            var v = new Vector2(0, 1);
            var w = new Vector2(-1, -1);

            Expect(!Vector2Utils.AreCcw(v, u, w));

            u = new Vector2(0.1, 1);
            v = new Vector2(0, 1);
            w = new Vector2(-0.1, 1);

            Expect(!Vector2Utils.AreCcw(w, v, u));
        }
    }
}
