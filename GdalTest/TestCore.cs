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
            Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES");
            Gdal.AllRegister();
            Gdal.SetConfigOption("AWS_S3_ENDPOINT", "127.0.0.1:9000");
            Gdal.SetConfigOption("AWS_SECRET_ACCESS_KEY", "minioadmin");
            Gdal.SetConfigOption("AWS_ACCESS_KEY_ID", "minioadmin");
            Gdal.SetConfigOption("CPL_VSIL_USE_TEMP_FILE_FOR_RANDOM_WRITE", "YES");

            string filePath = @"/vsis3/testbucket/dem.tif";
            Dataset ds = (Dataset)Gdal.Open(filePath, Access.GA_ReadOnly);

        }
    }
}
