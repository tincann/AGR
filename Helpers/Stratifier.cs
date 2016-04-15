using System.Diagnostics;
using OpenTK;

namespace RayTracer.Helpers
{
    public class Stratifier
    {
        private readonly RNG _rng;
        private readonly int _width;
        private readonly int _height;
        private readonly float _invWidth;
        private readonly float _invHeight;

        public Stratifier(RNG rng, int width, int height)
        {
            _rng = rng;
            _width = width;
            _height = height;
            _invWidth = 1f/_width;
            _invHeight = 1f/_height;
        }

        public Vector2 GetRandomPointInStratum(int stratumId)
        {
            Debug.Assert(stratumId < _width * _height);

            var x = (stratumId%_width) * _invWidth;
            var y = (int)(stratumId/_height) * _invHeight;
            var r0 = _rng.RandomFloat() / _width;
            var r1 = _rng.RandomFloat() / _height;
            return new Vector2(x + r0, y + r1);
        }
    }
}
