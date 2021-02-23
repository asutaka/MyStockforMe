using System;

namespace MyStock.Models
{
    public class CoPhieuTrackingModel
    {
        public long Id { get; set; }
        public long IdCoPhieu { get; set; }
        public DateTime NgayThem { get; set; }
        public string MoTa { get; set; }
        public int MucDoUuTien { get; set; }
        public int TrangThai { get; set; }
    }
}
