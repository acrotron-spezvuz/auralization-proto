using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auralization.API.Models
{
    /// <summary>
    /// Source of a sound
    /// </summary>
    public struct SoundSource
    {
        /// <summary>
        /// Type of source
        /// </summary>
        public SourceTypes source { get; set; }

        /// <summary>
        /// Height
        /// </summary>
        public double height { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public double lat { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        public double lon { get; set; }

        /// <summary>
        /// Velocity X
        /// </summary>
        public double Vx { get; set; }

        /// <summary>
        /// Velocity Y
        /// </summary>
        public double Vy { get; set; }

        /// <summary>
        /// Velocity Z
        /// </summary>
        public double Vz { get; set; }
    }
}
