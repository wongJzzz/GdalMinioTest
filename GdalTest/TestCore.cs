using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OSGeo.GDAL;
using OSGeo.OGR;
using EM.GIS.GdalExtensions;

namespace GdalTest
{
    internal class TestCore
    {
        public TestCore()
        {
            Console.WriteLine("TestCore");


            string AWS_S3_ENDPOINT = "192.168.1.93:9000";
            string AWS_SECRET_ACCESS_KEY = "minioadmin";
            string AWS_ACCESS_KEY_ID = "minioadmin";

            SetConfig(AWS_S3_ENDPOINT, AWS_SECRET_ACCESS_KEY, AWS_ACCESS_KEY_ID);

            string tiffPath = @"/vsis3/test/dem.tif";
            string dsPrj;
            double[] trans;
            //ReadGeoTIFF(tiffPath, out dsPrj, out trans);


            string gdbPath = @"/vsis3/test/众联慧图.gdb";
            //Dataset ds = Gdal.Open(gdbPath, Access.GA_ReadOnly);
            DataSource dSouce = Ogr.Open(gdbPath, 0); // 0 : 只读;  1: 读写
            string name = dSouce.GetName();
            int laycount = dSouce.GetLayerCount();
            string[] layerName = new string[laycount];
            for (int i = 0; i < laycount; i++) {
                Layer layer = dSouce.GetLayerByIndex(i);
                layerName[i] = layer.GetName();
            }
            Console.WriteLine(System.Text.Encoding.Default.EncodingName);

            
            Console.WriteLine("Done!");
        }


        /// <summary>
        /// 配置编码
        /// </summary>
        private static void ConfigEncoding()
        {
            // 为了支持中文路径，如果默认编码非UTF8，请添加下面这句代码
            if (Encoding.Default.EncodingName != Encoding.UTF8.EncodingName || Encoding.Default.CodePage != Encoding.UTF8.CodePage)
            {
                var filenameConfig = Gdal.GetConfigOption("GDAL_FILENAME_IS_UTF8", string.Empty);
                if (filenameConfig != "NO")
                {
                    Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "NO");
                }
            }
            try
            {
                Encoding gbk = Encoding.GetEncoding(FeatureExtensions.GdalEncoding);
            }
            catch (Exception e)//如果无法获取GBK编码，则需注册编码
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            }
        }

        public void SetConfig(string AWS_S3_ENDPOINT, string AWS_SECRET_ACCESS_KEY, string AWS_ACCESS_KEY_ID)
        {
            Gdal.AllRegister();
            Ogr.RegisterAll();
            Gdal.SetConfigOption("AWS_HTTPS", "NO");
            //Gdal.SetConfigOption("SHAPE_ENCODING", "");
            //Gdal.SetConfigOption("GDAL_DISABLE_READDIR_ON_OPEN", "YES");
            Gdal.SetConfigOption("AWS_VIRTUAL_HOSTING", "FALSE"); // host s3
            Gdal.SetConfigOption("AWS_S3_ENDPOINT", AWS_S3_ENDPOINT); // 地址
            Gdal.SetConfigOption("AWS_SECRET_ACCESS_KEY", AWS_SECRET_ACCESS_KEY); // 密码
            Gdal.SetConfigOption("AWS_ACCESS_KEY_ID", AWS_ACCESS_KEY_ID); // 账号
            //Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES");
            //ConfigEncoding();
        }

        /// <summary>
        /// 读取S3上的tif
        /// </summary>
        /// <param name="s3objectPath"></param>
        /// <param name="dsPrj"></param>
        /// <param name="trans"></param>
        public void ReadGeoTIFF(string s3objectPath, out string dsPrj, out double[] trans)
        {
            Dataset ds = Gdal.Open(s3objectPath, Access.GA_ReadOnly);
            trans = new double[6];
            dsPrj = ds.GetProjection();
            ds.GetGeoTransform(trans);
        }


    }
}
