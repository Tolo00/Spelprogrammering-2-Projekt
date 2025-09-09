using System;

namespace Arosoul.Essentials {

    public static class FloatExtensions {
        public static float MapValue(this float value, float oldMin, float oldMax, float newMin, float newMax) {
            // Ensure the ranges are valid
            if (oldMin == oldMax)
                throw new ArgumentException("The input range must have different min and max values.");
            if (newMin == newMax)
                throw new ArgumentException("The output range must have different min and max values.");

            // Map the value
            return (value - oldMin) / (oldMax - oldMin) * (newMax - newMin) + newMin;
        }
    }

}
