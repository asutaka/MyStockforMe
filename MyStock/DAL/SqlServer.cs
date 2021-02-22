using MyStock.Models;
using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;

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
                return int.Parse(dt.Rows[0][0].ToString()) > 0;
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
                return long.Parse(dt.Rows[0][0].ToString());
            }
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
