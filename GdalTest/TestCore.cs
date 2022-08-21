using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSGeo.GDAL;
using OSGeo.OGR;

namespace GdalTest
{
    internal class TestCore
    {
        public TestCore()
        {
            Console.WriteLine("TestCore");

            Gdal.AllRegister();

            Gdal.SetConfigOption("AWS_HTTPS", "NO"); 
            Gdal.SetConfigOption("GDAL_DISABLE_READDIR_ON_OPEN", "YES");
            Gdal.SetConfigOption("AWS_VIRTUAL_HOSTING", "FALSE");
            Gdal.SetConfigOption("AWS_S3_ENDPOINT", "localhost:9000"); // 地址
            Gdal.SetConfigOption("AWS_SECRET_ACCESS_KEY", "minioadmin"); // 密码
            Gdal.SetConfigOption("AWS_ACCESS_KEY_ID", "minioadmin"); // 账号
            
            string filePath = @"/vsis3/testbucket/dem.tif";
            Dataset ds = (Dataset)Gdal.Open(filePath, Access.GA_ReadOnly);
            string dsPrj = ds.GetProjection();
            double[] trans = new double[6];
            ds.GetGeoTransform(trans);


            Console.WriteLine("Done");
        }
    }
}
