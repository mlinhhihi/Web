using System.ComponentModel.DataAnnotations;

namespace QLBanHangGauBong_65131773.Models
{
    public class DangNhapKH
    {
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản")]
        public string TenTK { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; } 
    }
}
