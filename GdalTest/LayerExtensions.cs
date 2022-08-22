using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSGeo.OGR;
using System.Runtime.InteropServices;


namespace EM.GIS.GdalExtensions
{
    /// <summary>
    /// 图层扩展方法
    /// </summary>
    public static class LayerExtensions
    {
        /// <summary>
        /// 获取名称
        /// </summary>
        /// <param name="layer">图层</param>
        /// <returns>名称</returns>
        [DllImport(FeatureExtensions.GdalDllName, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr OGR_L_GetName(HandleRef layer);
        /// <summary>
        /// 获取名称
        /// </summary>
        /// <param name="layer">图层</param>
        /// <returns>名称</returns>
        public static string GetNameUTF8(this Layer layer)
        {
            var layerRef = Layer.getCPtr(layer);
            IntPtr strPtr = OGR_L_GetName(layerRef);
            string value = strPtr.IntPtrTostring(Encoding.UTF8);
            return value;
        }
    }
}
