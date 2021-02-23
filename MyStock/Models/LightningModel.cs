using System;

namespace MyStock.Models
{
    public class LightningModel
    {
        public long Id { get; set; }
        public string MaCK { get; set; }
        public float TC { get; set; }
        public float Tran { get; set; }
        public float San { get; set; }
        public long TongKL { get; set; }
        public float TongGT { get; set; }
        public float Gia3_Mua { get; set; }
        public long KL3_Mua { get; set; }
        public float Gia2_Mua { get; set; }
        public long KL2_Mua { get; set; }
        public float Gia1_Mua { get; set; }
        public long KL1_Mua { get; set; }
        public float Gia { get; set; }
        public long KL { get; set; }
        public float ChenhLech { get; set; }
        public float ChenhLechPhanTram { get; set; }
        public float Gia1_Ban { get; set; }
        public long KL1_Ban { get; set; }
        public float Gia2_Ban { get; set; }
        public long KL2_Ban { get; set; }
        public float Gia3_Ban { get; set; }
        public long KL3_Ban { get; set; }
        public float Cao { get; set; }
        public float TB { get; set; }
        public float Thap { get; set; }
        public long Mua_Du { get; set; }
        public long Ban_Du { get; set; }
        public long Mua_DTNN { get; set; }
        public long Ban_DTNN { get; set; }
        public long Du_DTNN { get; set; }
        public DateTime Update_Time { get; set; }
    }
}
