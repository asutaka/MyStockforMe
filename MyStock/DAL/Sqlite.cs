using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.IO;

namespace MyStock.DAL
{
    public static class Sqlite
    {
        //private static SQLiteConnection cnn;
        ////private static string connectionString = "Server=.;Database=Stock;Trusted_Connection=True;";
        //private static string connectionString = $"Data Source={Directory.GetCurrentDirectory()}//Db//sqlite.db;Version=3;datetimeformat=CurrentCulture";
        ////private string strInsertChuong = "";
        ////private string strUpdateChuong = "";
        ////private string strDeleteChuong = "";
        //private static string strSelectChuong = "SELECT * FROM tblChuong ORDER BY OrderBy";
        //private static string strSelectMaxChuong = "SELECT MAX(Id) FROM tblChuong";

        //private static string strLookup = "SELECT Id,TenChuong FROM tblChuong";

        ////private string strInsertAnh = "";
        ////private string strUpdateAnh = "";
        ////private string strDeleteAnh = "";
        //private static string strSelectAnh = "SELECT * FROM tblAnh ORDER BY OrderBy DESC";
        //private static string strSelectMaxAnh = "SELECT MAX(Id) FROM tblAnh";

        ////private string strInsertGhiChu = "";
        ////private string strUpdateGhiChu = "";
        ////private string strDeleteGhiChu = "";
        //private static string strSelectGhiChu = "SELECT * FROM tblGhichu ORDER BY OrderBy DESC";
        //private static string strSelectMaxGhiChu = "SELECT MAX(Id) FROM tblGhichu";
        //public static SQLiteConnection Connect()
        //{
        //    try
        //    {
        //        if (cnn == null)
        //            cnn = new SQLiteConnection(connectionString);
        //        if (cnn.State == ConnectionState.Closed)
        //            cnn.Open();
        //        Console.WriteLine("Connection is Opened ! ");
        //        return cnn;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Connection is not Open ! ");
        //        throw;
        //    }
        //}

        //public static DataTable SelectChuong()
        //{
        //    using (var ds = new DataSet())
        //    {
        //        var command = new SQLiteCommand(strSelectChuong, Connect());
        //        var adapter = new SQLiteDataAdapter();
        //        adapter.SelectCommand = command;
        //        adapter.Fill(ds, "tblChuong");
        //        adapter.Dispose();
        //        command.Dispose();
        //        cnn.Close();
        //        DataTable dt = ds.Tables[0];
        //        return dt;
        //    }
        //}

        //public static int GetMaxIndexChuong()
        //{
        //    using (var ds = new DataSet())
        //    {
        //        var command = new SQLiteCommand(strSelectMaxChuong, Connect());
        //        var adapter = new SQLiteDataAdapter();
        //        adapter.SelectCommand = command;
        //        adapter.Fill(ds, "tblChuong");
        //        adapter.Dispose();
        //        command.Dispose();
        //        cnn.Close();
        //        DataTable dt = ds.Tables[0];
        //        var tmp = dt.Rows[0][0];
        //        return (dt == null || string.IsNullOrWhiteSpace(dt.Rows[0][0].ToString())) ? 1 : int.Parse(dt.Rows[0][0].ToString()) + 1;
        //    }
        //}

        //public static DataTable Search(string query)
        //{
        //    using (var ds = new DataSet())
        //    {
        //        var command = new SQLiteCommand(query, Connect());
        //        var adapter = new SQLiteDataAdapter();
        //        adapter.SelectCommand = command;
        //        adapter.Fill(ds, "tblChuong");
        //        adapter.Dispose();
        //        command.Dispose();
        //        cnn.Close();
        //        DataTable dt = ds.Tables[0];
        //        return dt;
        //    }
        //}

        //public static DataTable SelectLookupEdit()
        //{
        //    using (var ds = new DataSet())
        //    {
        //        var command = new SQLiteCommand(strLookup, Connect());
        //        var adapter = new SQLiteDataAdapter();
        //        adapter.SelectCommand = command;
        //        adapter.Fill(ds, "tblChuong");
        //        adapter.Dispose();
        //        command.Dispose();
        //        cnn.Close();
        //        DataTable dt = ds.Tables[0];
        //        return dt;
        //    }
        //}

        //public static DataTable SelectAnh()
        //{
        //    using (var ds = new DataSet())
        //    {
        //        var command = new SQLiteCommand(strSelectAnh, Connect());
        //        var adapter = new SQLiteDataAdapter();
        //        adapter.SelectCommand = command;
        //        adapter.Fill(ds, "tblAnh");
        //        adapter.Dispose();
        //        command.Dispose();
        //        cnn.Close();
        //        DataTable dt = ds.Tables[0];
        //        return dt;
        //    }
        //}

        //public static int GetMaxIndexAnh()
        //{
        //    using (var ds = new DataSet())
        //    {
        //        var command = new SQLiteCommand(strSelectMaxAnh, Connect());
        //        var adapter = new SQLiteDataAdapter();
        //        adapter.SelectCommand = command;
        //        adapter.Fill(ds, "tblAnh");
        //        adapter.Dispose();
        //        command.Dispose();
        //        cnn.Close();
        //        DataTable dt = ds.Tables[0];
        //        return (dt == null || string.IsNullOrWhiteSpace(dt.Rows[0][0].ToString())) ? 1 : int.Parse(dt.Rows[0][0].ToString()) + 1;
        //    }
        //}

        //public static DataTable SelectGhiChu()
        //{
        //    using (var ds = new DataSet())
        //    {
        //        var command = new SQLiteCommand(strSelectGhiChu, Connect());
        //        var adapter = new SQLiteDataAdapter();
        //        adapter.SelectCommand = command;
        //        adapter.Fill(ds, "tblGhiChu");
        //        adapter.Dispose();
        //        command.Dispose();
        //        cnn.Close();
        //        DataTable dt = ds.Tables[0];
        //        return dt;
        //    }
        //}

        //public static int GetMaxIndexGhiChu()
        //{
        //    using (var ds = new DataSet())
        //    {
        //        var command = new SQLiteCommand(strSelectMaxGhiChu, Connect());
        //        var adapter = new SQLiteDataAdapter();
        //        adapter.SelectCommand = command;
        //        adapter.Fill(ds, "tblGhiChu");
        //        adapter.Dispose();
        //        command.Dispose();
        //        cnn.Close();
        //        DataTable dt = ds.Tables[0];
        //        return (dt == null || string.IsNullOrWhiteSpace(dt.Rows[0][0].ToString())) ? 1 : int.Parse(dt.Rows[0][0].ToString()) + 1;
        //    }
        //}

        //public static bool Execute(string query)
        //{
        //    var cmd = new SQLiteCommand(Connect());
        //    cmd.CommandText = query;
        //    cmd.ExecuteNonQuery();
        //    return true;
        //}

        //public static string GetPath(this string fileName)
        //{
        //    return $"{Directory.GetCurrentDirectory()}//Images//{fileName}";
        //}

        //public static DataTable ReadExcelFile()
        //{
        //    var path = "C:\\tmp.xlsx";
        //    var sheetName = "All";
        //    using (OleDbConnection conn = new OleDbConnection())
        //    {
        //        DataTable dt = new DataTable();
        //        string Import_FileName = path;
        //        string fileExtension = Path.GetExtension(Import_FileName);
        //        if (fileExtension == ".xls")
        //            conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 8.0;HDR=YES;'";
        //        if (fileExtension == ".xlsx")
        //            conn.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Import_FileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;'";
        //        using (OleDbCommand comm = new OleDbCommand())
        //        {
        //            comm.CommandText = "Select * from [" + sheetName + "$]";
        //            comm.Connection = conn;
        //            using (OleDbDataAdapter da = new OleDbDataAdapter())
        //            {
        //                da.SelectCommand = comm;
        //                da.Fill(dt);
        //                return dt;
        //            }
        //        }
        //    }
        //}
        //public static void ImportDataMaCK(DataTable dt)
        //{
        //    int index = 1;
        //    foreach (var item in dt.AsEnumerable())
        //    {
        //        var model = new MaChungKhoanModel();
        //        model.Id = index++;
        //        model.MaCK = item["Mã CK"].ToString();
        //        if (string.IsNullOrWhiteSpace(model.MaCK))
        //            continue;
        //        var khoiLuongDangKyStr = item["KL đăng ký NY"].ToString().Replace(".", "");
        //        var khoiLuongLuuHanhStr = item["Khối lượng lưu hành"].ToString().Replace(".", "");
        //        var isSuccess = DateTime.TryParse(item["Ngày niêm yết"].ToString(), out var dtime);

        //        model.MaISIN = item["Mã ISIN"].ToString();
        //        model.MaFIGI = item["Mã FIGI"].ToString();
        //        model.TenDoanhNghiepNiemYet = item["Tên DN niêm yết"].ToString();
        //        model.KhoiLuongDangKyNiemYet = long.Parse(khoiLuongDangKyStr.Substring(0, khoiLuongDangKyStr.IndexOf(",")));
        //        model.KhoiLuongLuuHanh = long.Parse(khoiLuongLuuHanhStr.Substring(0, khoiLuongLuuHanhStr.IndexOf(",")));
        //        if (isSuccess)
        //            model.NgayNiemYet = dtime;

        //        var query = $"INSERT INTO [tblMaChungKhoan]([Id],[MaCK],[MaISIN],[MaFIGI],[TenDoanhNghiepNiemYet],[KhoiLuongDangKyNiemYet],[KhoiLuongLuuHanh],[NgayNiemYet]) " +
        //            $"VALUES ('{model.Id}','{model.MaCK.Trim()}','{model.MaISIN.Trim()}','{model.MaFIGI.Trim()}','{model.TenDoanhNghiepNiemYet.Trim()}','{model.KhoiLuongDangKyNiemYet}','{model.KhoiLuongLuuHanh}','{model.NgayNiemYet}')";
        //        Execute(query);
        //    }
        //}
    }
}
