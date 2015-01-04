using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epicycle.Math.Geometry.Polytopes
{
    // polyhedral surface, can be open or closed
    // guarantees combinatorically consistent state, doesn't guarantee lack of self-intersections
    public interface IPolysurface : IPartialPolysurface
    {
           
    }
}
