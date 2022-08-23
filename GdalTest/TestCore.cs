using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OSGeo.GDAL;
using OSGeo.OGR;
using OSGeo.OSR;


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
            tiffPath = @"/vsis3/test/停车场.tif";

            string dsPrj;
            double[] trans;
            ReadGeoTIFF(tiffPath, 0, out dsPrj, out trans);

            string gdbPath = @"/vsis3/test/众联慧图.gdb";
            string name;
            int layerCount;
            string[] layerName;
            ReadFileGDB(gdbPath, out name, out layerCount, out layerName);


            string shpPath = @"/vsis3/test/广东省区域范围_非正式.shp";
            ReadShapefile(shpPath);

            DataSource ds = Ogr.Open(@"/vsis3/test/广东省区域范围_非正式.prj", 0);
            

            Console.WriteLine("Done!");
            //Console.ReadKey();
        }

        public void ReadShapefileDir(string dirPath)
        {

        }

        public void ReadShapefile(string shpPath)
        {
            DataSource ds = Ogr.Open(shpPath, 0);

            string dName = ds.GetDriver().GetName();
            Layer lyr = ds.GetLayerByIndex(0);
            var c = lyr.GetName();
            ds.GetName();
            SpatialReference pro = lyr.GetSpatialRef();
            string a = pro.GetName();


            Envelope envelope = new Envelope();
            var b = lyr.GetExtent(envelope, 1);

            var d = lyr.GetLayerDefn();
            var e = d.GetName();
            var f = d.GetFieldCount();
            for (int i = 0; i < f; i++)
            {
                Console.WriteLine(d.GetFieldDefn(i).GetName());
            }


            Dictionary<string, object> map = new Dictionary<string, object>();
            map.Add("MinX", envelope.MinX.ToString());
            map.Add("MinY", envelope.MinY.ToString());
            map.Add("MaxY", envelope.MaxY.ToString());
            map.Add("MaxX", envelope.MaxX.ToString());


            GetSpatialRefence(pro);

            

            Console.WriteLine("Done!");
        }

        public Dictionary<string, string> GetSpatialRefence(SpatialReference spatialRefence)
        {
            Dictionary<string, string> spatialInfo = new Dictionary<string, string>();
            Dictionary<string, string> pcsRef = new Dictionary<string, string>();
            Dictionary<string, string> gcsRef = new Dictionary<string, string>();

            string json;
            string[] jsonArray = {};
            spatialRefence.ExportToPROJJSON(out json, jsonArray);

            pcsRef.Add("投影坐标系", spatialRefence.GetName());
            pcsRef.Add("投影", "");
            pcsRef.Add("WKID", spatialRefence.GetAuthorityCode("PROJCS"));
            pcsRef.Add("ESRI WKID", "");
            pcsRef.Add("授权", spatialRefence.GetAuthorityName("PROJCS"));
            pcsRef.Add("线性单位", spatialRefence.GetLinearUnitsName() + "(" + spatialRefence.GetLinearUnits().ToString() + ")");
            pcsRef.Add("东偏移量", "");
            pcsRef.Add("北偏移量", "");
            pcsRef.Add("中央经线", "");
            pcsRef.Add("标准纬线", "");
            pcsRef.Add("辅助球体类型", "");


            gcsRef.Add("地理坐标系", spatialRefence.GetName());
            gcsRef.Add("WKID", spatialRefence.GetAuthorityCode("GEOGCS"));
            gcsRef.Add("授权", spatialRefence.GetAuthorityName("GEOGCS"));
            gcsRef.Add("角度单位", spatialRefence.GetAngularUnitsName() + "(" + spatialRefence.GetAngularUnits().ToString() + ")");
            gcsRef.Add("本初子午线", "");
            gcsRef.Add("基准面", "");
            gcsRef.Add("参考椭球体", "");
            gcsRef.Add("长半轴", spatialRefence.GetSemiMajor().ToString());
            gcsRef.Add("短半轴", "");
            gcsRef.Add("扁率", spatialRefence.GetInvFlattening().ToString());

            return spatialInfo;
        }

        private static void ReadFileGDB(string gdbPath, out string name, out int layerCount, out string[] layerName)
        {            
            DataSource dSouce = Ogr.Open(gdbPath, 0); // 0 : 只读;  1: 读写
            name = dSouce.GetName();
            layerCount = dSouce.GetLayerCount();
            layerName = new string[layerCount];
            //Console.WriteLine();
            //Console.WriteLine(name,"UTF8");
            //Console.WriteLine(layerCount);
            //Console.WriteLine();
            for (int i = 0; i < layerCount; i++)
            {
                Layer layer = dSouce.GetLayerByIndex(i);
                layerName[i] = layer.GetName();
                //Console.WriteLine(layerName[i]);
            }
            //Console.WriteLine();
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
            //Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "NO");
            //ConfigEncoding();

            //Gdal.SetConfigOption("SHAPE_ENCODING", "CP936");
        }

        /// <summary>
        /// 读取S3上的tif
        /// </summary>
        /// <param name="s3objectPath"></param>
        /// <param name="dsPrj"></param>
        /// <param name="trans"></param>
        public void ReadGeoTIFF(string s3objectPath, int detail ,out string dsPrj, out double[] trans)
        {
            Dataset ds = Gdal.Open(s3objectPath, Access.GA_ReadOnly);
            trans = new double[6];
            dsPrj = ds.GetProjection();
            ds.GetGeoTransform(trans);
            string desc = ds.GetDescription();
            int width = ds.RasterXSize;
            int height = ds.RasterYSize;
            int bands = ds.RasterCount;

            List<string> options = new List<string>();
            options.Add("-json");
            options.Add("-proj4");
            if (detail == 1)
            {
                // 计算栅格每个波段的最大最小值 耗时！
                options.Add("-mm");
                // 显示栅格统计值,如果没有,则强制计算统计值(均值,最大最小值,标准差等) 耗时！
                options.Add("-stats");
                // 显示栅格统计值,如果没有,则强制计算.但是是非精确计算,基于缩略图或者部分数据进行计算,
                // 如果不需要精确值或者希望快速返回,请使用此参数代替-stats 耗时
                options.Add("-approx_stats");
                // 显示所有波段的直方图信息 耗时
                options.Add("-hist");
                // 强制计算所有波段的校验值 耗时
                options.Add("-checksum");
            }

            string[] infoOptions = options.ToArray();
            GDALInfoOptions gDALInfoOptions = new GDALInfoOptions(infoOptions);

            string info = Gdal.GDALInfo(ds, gDALInfoOptions);
            


            Console.WriteLine("ReadGeoTIFF DONE!");
            
        }


    }
}
