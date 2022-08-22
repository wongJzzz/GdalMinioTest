using OSGeo.OGR;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace EM.GIS.GdalExtensions
{
    /// <summary>
    /// 要素扩展
    /// </summary>
    public static class FeatureExtensions
    {
        /// <summary>
        /// gdal文件名
        /// </summary>
        public const string GdalDllName = "gdal305.dll";
        /// <summary>
        /// gdal编码名称
        /// </summary>
        public const string GdalEncoding = "GBK";

        /// <summary>
        /// 获取要素指定字段的字符串值
        /// </summary>
        /// <param name="featureHandle">要素句柄</param>
        /// <param name="fieldIndex">字段索引</param>
        /// <returns>字符串值</returns>
        [DllImport(GdalDllName, CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr OGR_F_GetFieldAsString(HandleRef featureHandle, int fieldIndex);

        /// <summary>
        /// 设置要素指定字段的值为字符串
        /// </summary>
        /// <param name="featureHandle">要素句柄</param>
        /// <param name="fieldIndex">字段索引</param>
        /// <param name="value">值</param>
        [DllImport(GdalDllName, CallingConvention = CallingConvention.Cdecl)]
        public extern static void OGR_F_SetFieldString(HandleRef featureHandle, int fieldIndex, IntPtr value);
        /// <summary>
        /// 获取要素指定字段的字符串值
        /// </summary>
        /// <param name="feature">要素</param>
        /// <param name="fieldIndex">字段索引</param>
        /// <returns>字符串值</returns>
        public static string GetFieldAsStringUTF8(this Feature feature, int fieldIndex)
        {
            HandleRef handle = Feature.getCPtr(feature);
            IntPtr intptr = OGR_F_GetFieldAsString(handle, fieldIndex);
            string value = intptr.IntPtrTostring(Encoding.UTF8);
            return value;
        }
    }
}

