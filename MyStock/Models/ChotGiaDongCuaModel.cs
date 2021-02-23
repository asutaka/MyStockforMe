using System;

namespace MyStock.Models
{
    public class ChotGiaDongCuaModel
    {
        public long Id { get; set; }
        public long IdCoPhieu { get; set; }
        public DateTime NgayGiaoDich { get; set; }
        public float GiaThamChieu { get; set; }
        public float GiaMoCua { get; set; }
        public float GiaDongCua { get; set; }
        public float GiaCaoNhat { get; set; }
        public float GiaThapNhat { get; set; }
        public float GiaTran { get; set; }
        public float GiaSan { get; set; }
        public long KhoiLuongGiaoDich { get; set; }
        public float GiaMoCuaFix { get; set; }
        public float GiaDongCuaFix { get; set; }
        public float GiaCaoNhatFix { get; set; }
        public float GiaThapNhatFix { get; set; }
        public long Mua_DTNN { get; set; }
        public long Ban_DTNN { get; set; }
    }
}
