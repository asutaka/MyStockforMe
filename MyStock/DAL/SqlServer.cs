using MyStock.Common;
using MyStock.Models;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyStock.DAL
{
    public static class SqlServer
    {
        private static SqlConnection cnn;
        private static string connectionString = "Server=.;Database=MyStock;Trusted_Connection=True;MultipleActiveResultSets=True;";
        public static SqlConnection Connect()
        {
            try
            {
                if (cnn == null)
                    cnn = new SqlConnection(connectionString);
                if (cnn.State == ConnectionState.Closed)
                    cnn.Open();
                Console.WriteLine("Connection is Opened ! ");
                return cnn;
            }
            catch 
            {
                Console.WriteLine("Connection is not Open ! ");
                throw;
            }
        }
        public static bool Execute(string query)//insert,update,delete
        {
            new SqlCommand(query, Connect()).ExecuteNonQuery();
            return true;
        }
        public static DataTable GetData(string query)//select
        {
            using (var dt = new DataTable())
            {
                var command = new SqlCommand(query, Connect());
                var adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                adapter.Fill(dt);
                adapter.Dispose();
                command.Dispose();
                return dt;
            }
        }
        public static bool CheckExists(string query)
        {
            using (var dt = new DataTable())
            {
                var command = new SqlCommand(query, Connect());
                var adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                adapter.Fill(dt);
                adapter.Dispose();
                command.Dispose();
                return dt.Rows.Count > 0;
            }
        }
        public static long GetMaxIndex(string query)
        {
            using (var dt = new DataTable())
            {
                var command = new SqlCommand(query, Connect());
                var adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                adapter.Fill(dt);
                adapter.Dispose();
                command.Dispose();
                return ((dt.Rows.Count == 0) || string.IsNullOrWhiteSpace(dt.Rows[0][0].ToString())) ? 1 : (1 + long.Parse(dt.Rows[0][0].ToString()));
            }
        }
        public static void ImportCsvFile(string path)
        {
            var lstModel = new List<ChotGiaDongCuaModel>();
            var index = GetMaxIndex("SELECT MAX(ID) FROM ChotGiaDongCua");
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (values[0].Equals("<Ticker>"))
                        continue;

                    lstModel.Add(new ChotGiaDongCuaModel
                    {
                        Id = index++,
                        IdCoPhieu = StaticValue.dicCoPhieu.First(x => x.Value == values[0].Trim()).Key,
                        NgayGiaoDich = DateTime.ParseExact(values[1], "yyyyMMdd", CultureInfo.InvariantCulture),
                        GiaThamChieu = 0,
                        GiaMoCuaFix = float.Parse(values[2]),
                        GiaCaoNhatFix = float.Parse(values[3]),
                        GiaThapNhatFix = float.Parse(values[4]),
                        GiaDongCuaFix = float.Parse(values[5]),
                        KhoiLuongGiaoDich = long.Parse(values[6]),
                        GiaMoCua = float.Parse(values[7]),
                        GiaCaoNhat = float.Parse(values[8]),
                        GiaThapNhat = float.Parse(values[9]),
                        GiaDongCua = float.Parse(values[10]),
                        Mua_DTNN = long.Parse(values[12]),
                        Ban_DTNN = long.Parse(values[13])
                    }); 
                }
            }
            index = 0;
            var queryInsert = "INSERT INTO [dbo].[ChotGiaDongCua]		        "
                                + "           ([Id]                         "
                                + "           ,[IdCoPhieu]                  "
                                + "           ,[NgayGiaoDich]               "
                                + "           ,[GiaThamChieu]               "
                                + "           ,[GiaMoCua]                   "
                                + "           ,[GiaDongCua]                 "
                                + "           ,[GiaCaoNhat]                 "
                                + "           ,[GiaThapNhat]                "
                                + "           ,[GiaTran]                    "
                                + "           ,[GiaSan]                     "
                                + "           ,[KhoiLuongGiaoDich]          "
                                + "           ,[GiaMoCuaFix]                "
                                + "           ,[GiaDongCuaFix]              "
                                + "           ,[GiaCaoNhatFix]              "
                                + "           ,[GiaThapNhatFix]             "
                                + "           ,[Mua_DTNN]                   "
                                + "           ,[Ban_DTNN]                   "
                                + "           ,[Ngay_Tao])                  "
                                + "     VALUES                              ";
            var queryList = new StringBuilder().AppendLine(queryInsert);
            foreach (var model in lstModel)
            {
                var query =       $"           ('{model.Id}'                "
                                + $"           ,'{model.IdCoPhieu}'         "
                                + $"           ,CONVERT(DATE, '{model.NgayGiaoDich}', 103) "
                                + $"           ,'{model.GiaThamChieu}'       "
                                + $"           ,'{model.GiaMoCua}'           "
                                + $"           ,'{model.GiaDongCua}'         "
                                + $"           ,'{model.GiaCaoNhat}'         "
                                + $"           ,'{model.GiaThapNhat}'        "
                                + $"           ,'{model.GiaTran}'            "
                                + $"           ,'{model.GiaSan}'             "
                                + $"           ,'{model.KhoiLuongGiaoDich}'  "
                                + $"           ,'{model.GiaMoCuaFix}'        "
                                + $"           ,'{model.GiaDongCuaFix}'      "
                                + $"           ,'{model.GiaCaoNhatFix}'      "
                                + $"           ,'{model.GiaThapNhatFix}'     "
                                + $"           ,'{model.Mua_DTNN}'           "
                                + $"           ,'{model.Ban_DTNN}'           "
                                + $"           ,CONVERT(DATETIME, '{DateTime.Now}', 103)            ),"; 
                queryList.AppendLine(query);
                index++;
                if(index % 500 == 0)
                {
                    queryList.Remove(queryList.Length - 3, 3);
                    var tmp = queryList.ToString();
                    Execute(queryList.ToString());
                    queryList.Clear();
                    queryList.AppendLine(queryInsert);
                }
            }
            if(lstModel.Count()% 500 > 0)
            {
                queryList.Remove(queryList.Length - 3, 3);
                Execute(queryList.ToString());
            }
            MessageBox.Show("Import Success!");
        }
        public static void ImportCsvFolder(string path)
        {
            DirectoryInfo d = new DirectoryInfo(path);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.csv"); //Getting CSV files
            foreach (FileInfo file in Files)
            {
                var pathFile = $"{path}{file.Name}";
                var lstModel = new List<ChotGiaDongCuaModel>();
                var index = GetMaxIndex("SELECT MAX(ID) FROM ChotGiaDongCua");
                using (var reader = new StreamReader(pathFile))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        if (values[0].Equals("<Ticker>"))
                            continue;

                        lstModel.Add(new ChotGiaDongCuaModel
                        {
                            Id = index++,
                            IdCoPhieu = StaticValue.dicCoPhieu.First(x => x.Value == values[0].Trim()).Key,
                            NgayGiaoDich = DateTime.ParseExact(values[1], "yyyyMMdd", CultureInfo.InvariantCulture),
                            GiaThamChieu = 0,
                            GiaMoCuaFix = float.Parse(values[2]),
                            GiaCaoNhatFix = float.Parse(values[3]),
                            GiaThapNhatFix = float.Parse(values[4]),
                            GiaDongCuaFix = float.Parse(values[5]),
                            KhoiLuongGiaoDich = long.Parse(values[6]),
                            GiaMoCua = float.Parse(values[7]),
                            GiaCaoNhat = float.Parse(values[8]),
                            GiaThapNhat = float.Parse(values[9]),
                            GiaDongCua = float.Parse(values[10]),
                            Mua_DTNN = long.Parse(values[12]),
                            Ban_DTNN = long.Parse(values[13])
                        });
                    }
                }
                index = 0;
                var queryInsert = "INSERT INTO [dbo].[ChotGiaDongCua]		        "
                                    + "           ([Id]                         "
                                    + "           ,[IdCoPhieu]                  "
                                    + "           ,[NgayGiaoDich]               "
                                    + "           ,[GiaThamChieu]               "
                                    + "           ,[GiaMoCua]                   "
                                    + "           ,[GiaDongCua]                 "
                                    + "           ,[GiaCaoNhat]                 "
                                    + "           ,[GiaThapNhat]                "
                                    + "           ,[GiaTran]                    "
                                    + "           ,[GiaSan]                     "
                                    + "           ,[KhoiLuongGiaoDich]          "
                                    + "           ,[GiaMoCuaFix]                "
                                    + "           ,[GiaDongCuaFix]              "
                                    + "           ,[GiaCaoNhatFix]              "
                                    + "           ,[GiaThapNhatFix]             "
                                    + "           ,[Mua_DTNN]                   "
                                    + "           ,[Ban_DTNN]                   "
                                    + "           ,[Ngay_Tao])                  "
                                    + "     VALUES                              ";
                var queryList = new StringBuilder().AppendLine(queryInsert);
                foreach (var model in lstModel)
                {
                    var query = $"           ('{model.Id}'                "
                                    + $"           ,'{model.IdCoPhieu}'         "
                                    + $"           ,CONVERT(DATE, '{model.NgayGiaoDich}', 103) "
                                    + $"           ,'{model.GiaThamChieu}'       "
                                    + $"           ,'{model.GiaMoCua}'           "
                                    + $"           ,'{model.GiaDongCua}'         "
                                    + $"           ,'{model.GiaCaoNhat}'         "
                                    + $"           ,'{model.GiaThapNhat}'        "
                                    + $"           ,'{model.GiaTran}'            "
                                    + $"           ,'{model.GiaSan}'             "
                                    + $"           ,'{model.KhoiLuongGiaoDich}'  "
                                    + $"           ,'{model.GiaMoCuaFix}'        "
                                    + $"           ,'{model.GiaDongCuaFix}'      "
                                    + $"           ,'{model.GiaCaoNhatFix}'      "
                                    + $"           ,'{model.GiaThapNhatFix}'     "
                                    + $"           ,'{model.Mua_DTNN}'           "
                                    + $"           ,'{model.Ban_DTNN}'           "
                                    + $"           ,CONVERT(DATETIME, '{DateTime.Now}', 103)            ),";
                    queryList.AppendLine(query);
                    index++;
                    if (index % 500 == 0)
                    {
                        queryList.Remove(queryList.Length - 3, 3);
                        var tmp = queryList.ToString();
                        Execute(queryList.ToString());
                        queryList.Clear();
                        queryList.AppendLine(queryInsert);
                    }
                }
                if (lstModel.Count() % 500 > 0)
                {
                    queryList.Remove(queryList.Length - 3, 3);
                    Execute(queryList.ToString());
                }
            }
            MessageBox.Show("Import Success!");
        }
        public static void ImportExcelFile(string path)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile("FilePath", ExcelVersion.Version2016);
            //var sheetName = "HNX";
            //using (OleDbConnection conn = new OleDbConnection())
            //{
            //    DataTable dt = new DataTable();
            //    string Import_FileName = path;
            //    string fileExtension = Path.GetExtension(Import_FileName);
            //    if (fileExtension == ".xls")
            //        conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";
            //    if (fileExtension == ".xlsx")
            //        conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;'";
            //    using (OleDbCommand comm = new OleDbCommand())
            //    {
            //        comm.CommandText = "Select * from [" + sheetName + "$]";
            //        comm.Connection = conn;
            //        using (OleDbDataAdapter da = new OleDbDataAdapter())
            //        {
            //            da.SelectCommand = comm;
            //            da.Fill(dt);
            //            return dt;
            //        }
            //    }
            //}
        }
        public static void ImportExcelFolder(string path)
        {
           
        }




        public static string GetPath(this string fileName)
        {
            return $"{Directory.GetCurrentDirectory()}//Images//{fileName}";
        }
        public static DataTable ReadExcelFile()
        {
            var path = "C:\\MaChungKhoan.xlsx";
            var sheetName = "HNX";
            using (OleDbConnection conn = new OleDbConnection())
            {
                DataTable dt = new DataTable();
                string Import_FileName = path;
                string fileExtension = Path.GetExtension(Import_FileName);
                if (fileExtension == ".xls")
                    conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";
                if (fileExtension == ".xlsx")
                    conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;'";
                using (OleDbCommand comm = new OleDbCommand())
                {
                    comm.CommandText = "Select * from [" + sheetName + "$]";
                    comm.Connection = conn;
                    using (OleDbDataAdapter da = new OleDbDataAdapter())
                    {
                        da.SelectCommand = comm;
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        public static void ImportDataMaCK(DataTable dt)
        {
            int index = 1952;
            foreach (var item in dt.AsEnumerable())
            {
                var model = new CoPhieuModel();
                model.Id = index++;
                model.MaChungKhoan = item["Mã CK"].ToString().Trim();
                if (CheckExists($"select count(1) from CoPhieu where MaChungKhoan = '{model.MaChungKhoan}'"))
                    continue;
                model.TenCongTy = item["Tên DN niêm yết"].ToString().Trim();
                var tmp1 = item["KL đăng ký NY"].ToString().Trim().Split(',')[0].Replace(".","");
                var tmp2 = item["Khối lượng lưu hành"].ToString().Trim().Split(',')[0].Replace(".", "");
                model.KhoiLuongNiemYet = long.Parse(tmp1);
                model.KhoiLuongThucTe = long.Parse(tmp2);
                if(!string.IsNullOrWhiteSpace(item["Ngày niêm yết"].ToString().Trim()))
                    model.NgayNiemYet = DateTime.Parse(item["Ngày niêm yết"].ToString().Trim());
                if (string.IsNullOrWhiteSpace(model.MaChungKhoan))
                    continue;
                var query = $"INSERT INTO [dbo].[CoPhieu]([Id],[MaChungKhoan],[TenCongTy],[IdSan],[NgayNiemYet],[KhoiLuongNiemYet],[KhoiLuongThucTe])VALUES" +
                            $"('{model.Id}','{model.MaChungKhoan}',N'{model.TenCongTy}','2', convert(datetime, '{model.NgayNiemYet.Date}', 103),'{model.KhoiLuongNiemYet}', '{model.KhoiLuongThucTe}')";
                //var query = string.Empty;
                //if(model.NgayNiemYet > DateTime.MinValue)
                //    query = $"UPDATE COPHIEU SET TenCongTy = N'{model.TenCongTy}',KhoiLuongNiemYet = '{model.KhoiLuongNiemYet}', KhoiLuongThucTe = '{model.KhoiLuongThucTe}', NgayNiemYet = convert(datetime, '{model.NgayNiemYet.Date}', 103), TMP = '2' WHERE MaChungKhoan = '{model.MaChungKhoan}'";
                //else
                //    query = $"UPDATE COPHIEU SET TenCongTy = N'{model.TenCongTy}',KhoiLuongNiemYet = '{model.KhoiLuongNiemYet}', KhoiLuongThucTe = '{model.KhoiLuongThucTe}', TMP = '2' WHERE MaChungKhoan = '{model.MaChungKhoan}'";
                Execute(query);
            }
            //foreach (var item in dt.AsEnumerable())
            //{
            //    var model = new CoPhieuModel();
            //    model.Id = index++;
            //    model.MaChungKhoan = item["MÃ CK"].ToString().Trim();
            //    model.TenCongTy = item["TÊN CÔNG TY"].ToString().Trim();
            //    var san = item["SÀN"].ToString().Trim();
            //    switch(san)
            //    {
            //        case "HSX":
            //            model.IdSan = 1;
            //            break;
            //        case "HNX":
            //            model.IdSan = 2;
            //            break;
            //        case "Upcom":
            //            model.IdSan = 3;
            //            break;
            //        case "OTC":
            //            model.IdSan = 4;
            //            break;
            //    }
            //    if (string.IsNullOrWhiteSpace(model.MaChungKhoan))
            //        continue;
            //    var query = $"INSERT INTO [dbo].[CoPhieu]([Id],[MaChungKhoan],[TenCongTy],[IdSan],[NgayNiemYet],[KhoiLuongNiemYet],[KhoiLuongThucTe])VALUES"+
            //                $"('{model.Id}','{model.MaChungKhoan}',N'{model.TenCongTy}','{model.IdSan}', null, null, null)";
            //    Execute(query);
            //}
        }
        
    }
}
