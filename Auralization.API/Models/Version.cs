using System.ComponentModel.DataAnnotations;

namespace Auralization.API.Models
{
    public struct Version
    {
        [Required]
        public string api { get; }

        [Required]
        public string naf { get; }

        public (string name, string version)[] plugins { get; }

        public Version(string api, string naf, (string name, string version)[] plugins) : this()
        {
            this.api = api;
            this.naf = naf;
            this.plugins = plugins;
        }
    }
}
