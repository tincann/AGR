using RayTracer.World;

namespace RayTracer.Shading.Models
{
    public class MonteCarloLightingModel
    {
        private readonly Scene _scene;

        public MonteCarloLightingModel(Scene scene)
        {
            _scene = scene;
        }
    }
}
