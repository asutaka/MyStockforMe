using System;

namespace MyStock.Models
{
    public class BanCoPhieuModel
    {
        public long Id { get; set; }
        public long IdCoPhieu { get; set; }
        public DateTime NgayBan { get; set; }
        public float GiaBan { get; set; }
        public long SoLuongBan { get; set; }
        public float SoTien { get; set; }
    }
}
