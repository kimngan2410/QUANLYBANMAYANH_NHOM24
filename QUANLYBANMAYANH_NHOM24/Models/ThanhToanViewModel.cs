using QUANLYBANMAYANH_NHOM24.Models;
using System.ComponentModel.DataAnnotations;

public class ThanhToanViewModel
{
    [Required]
    public string Name { get; set; }

    [Required]
    [Phone]
    public string Phone { get; set; }

    public string Address { get; set; }
    public string Note { get; set; }

    [Required]
    public int PaymentMethodId { get; set; } // ID phương thức thanh toán

    public List<GioHangViewModel> GioHang { get; set; } = new List<GioHangViewModel>(); // Khởi tạo danh sách rỗng

    public int UserId { get; set; } // ID người dùng
}
