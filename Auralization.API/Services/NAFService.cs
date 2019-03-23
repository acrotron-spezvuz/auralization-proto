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

        /// <summary>
        /// Auralization of sound sources based on text in NAFSnaP format.
        /// </summary>
        /// <param name="content">a string to accept NAFSnaP-formatted content.</param>
        /// <returns>a string with location of a generated sound file or an error description, 
        /// same as the "Auralization" service response.</returns>
        [DllImport("NAFLib", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string AuralizeFromContent(string content);
    }
}
