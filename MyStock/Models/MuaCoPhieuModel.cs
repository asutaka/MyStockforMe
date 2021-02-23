using System;

namespace MyStock.Models
{
    public class MuaCoPhieuModel
    {
        public long Id { get; set; }
        public long IdCoPhieu { get; set; }
        public DateTime NgayMua { get; set; }
        public float GiaMua { get; set; }
        public long SoLuongMua { get; set; }
        public float SoTien { get; set; }
        public float Margin { get; set; }
    }
}
