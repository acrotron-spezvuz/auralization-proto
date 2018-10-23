using Auralization.API.Models;
using System.Runtime.InteropServices;

namespace Auralization.API.Services
{
    /// <summary>
    /// NAF Service 
    /// </summary>
    internal static class NAFService
    {
        /// <summary>
        /// Auralization of sound sources
        /// </summary>
        /// <param name="sources"></param>
        /// <returns></returns>
        [DllImport("NAFLib", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string Auralization(SoundSource[] sources);

    }
}
