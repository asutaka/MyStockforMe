using System;

namespace MyStock.Models
{
    public class CoPhieuModel
    {
        public long Id { get; set; }
        public string MaChungKhoan { get; set; }
        public string TenCongTy { get; set; }
        public long IdSan { get; set; }//Id Sàn
        public DateTime NgayNiemYet { get; set; }
        public long KhoiLuongNiemYet { get; set; }
        public long KhoiLuongThucTe { get; set; }
    }
}
