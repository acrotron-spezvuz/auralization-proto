using System.ComponentModel.DataAnnotations;

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
        [Required]
        public SourceTypes source { get; set; }

        /// <summary>
        /// Height
        /// </summary>
        public double height { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        [Range(-90, 90)]
        public double lat { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        [Range(-180, 180)]
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
