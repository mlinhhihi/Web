using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLBanHangGauBong_65131773.Models
{
    public class DangKyKH
    {
        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Tên tài khoản không được để trống")]
        public string TenTK { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu ít nhất 6 ký tự")]
        public string MatKhau { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required]
        public string Sdt { get; set; }

        public string GioiTinh { get; set; }
        public string DiaChi { get; set; }
    }
}