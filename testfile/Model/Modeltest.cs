// using System.ComponentModel.DataAnnotations;

// public class User
// {
//     [Key]
//     public int UserId { get; set; }

//     // ✅ Không được để trống
//     [Required(ErrorMessage = "Họ tên không được để trống")]
//     public string FullName { get; set; }

//     // ✅ Kiểm tra định dạng email hợp lệ
//     [Required(ErrorMessage = "Email là bắt buộc")]
//     [EmailAddress(ErrorMessage = "Email không hợp lệ")]
//     public string Email { get; set; }

//     // ✅ Giới hạn độ dài chuỗi từ 6 đến 20 ký tự
//     [Required]
//     [StringLength(20, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 20 ký tự")]
//     public string Password { get; set; }

//     // ✅ Kiểm tra số điện thoại hợp lệ (Việt Nam nên dùng thêm Regex nếu cần kỹ)
//     [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
//     public string Phone { get; set; }

//     // ✅ Giới hạn khoảng giá trị (tuổi từ 18 đến 60)
//     [Range(18, 60, ErrorMessage = "Tuổi phải từ 18 đến 60")]
//     public int Age { get; set; }

//     // ✅ Kiểm tra định dạng URL
//     [Url(ErrorMessage = "Đường dẫn không hợp lệ")]
//     public string Website { get; set; }

//     // ✅ So sánh 2 thuộc tính (mật khẩu nhập lại)
//     [Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
//     public string ConfirmPassword { get; set; }

//     // ✅ VerifiKey theo yêu cầu đề BTL (6 ký tự, kết thúc bằng số)
//     [Required(ErrorMessage = "Vui lòng nhập VerifiKey")]
//     [StringLength(6, ErrorMessage = "VerifiKey phải đúng 6 ký tự")]
//     [RegularExpression(@"^[A-Za-z0-9]{5}\d$", ErrorMessage = "VerifiKey phải có 6 ký tự và kết thúc bằng số")]
//     public string VerifiKey { get; set; }

//     // ✅ Cấm binding field này từ form
//     [BindNever]
//     public bool IsAdmin { get; set; }

//     // ✅ Chỉ phục vụ backend, không hiển thị form scaffold
//     [ScaffoldColumn(false)]
//     public DateTime CreatedAt { get; set; }
// }

// using System.ComponentModel.DataAnnotations;

// public class InputModel
// {
//     // ✅ Yêu cầu: 6 ký tự, kết thúc bằng số
//     [RegularExpression(@"^[A-Za-z0-9]{5}\d$", ErrorMessage = "Phải có 6 ký tự, kết thúc bằng số")]
//     public string VerifiKey1 { get; set; }

//     // ✅ Yêu cầu: Bắt đầu bằng 3 ký tự đặc biệt, sau đó là chữ/số
//     [RegularExpression(@"^[!@#$%^&*]{3}[A-Za-z0-9]+$", ErrorMessage = "Phải bắt đầu bằng 3 ký tự đặc biệt")]
//     public string VerifiKey2 { get; set; }

//     // ✅ Yêu cầu: Kết thúc bằng 3 ký tự đặc biệt
//     [RegularExpression(@"^[A-Za-z0-9]+[!@#$%^&*]{3}$", ErrorMessage = "Phải kết thúc bằng 3 ký tự đặc biệt")]
//     public string VerifiKey3 { get; set; }

//     // ✅ Yêu cầu: Có ít nhất 1 chữ, 1 số, 1 ký tự đặc biệt (mạnh kiểu mật khẩu)
//     [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[!@#$%^&*]).{6,}$", ErrorMessage = "Phải có ít nhất 1 chữ, 1 số và 1 ký tự đặc biệt")]
//     public string StrongKey { get; set; }

//     // ✅ Yêu cầu: Chỉ chứa chữ IN HOA và số, độ dài 4–8 ký tự
//     [RegularExpression(@"^[A-Z0-9]{4,8}$", ErrorMessage = "Chỉ gồm IN HOA và số (4–8 ký tự)")]
//     public string CodeUpperCase { get; set; }

//     // ✅ Yêu cầu: Không được chứa ký tự đặc biệt nào
//     [RegularExpression(@"^[A-Za-z0-9]*$", ErrorMessage = "Không được chứa ký tự đặc biệt")]
//     public string AlphaNumericOnly { get; set; }

//     // ✅ Yêu cầu: Chỉ số và đúng 5 chữ số
//     [RegularExpression(@"^\d{5}$", ErrorMessage = "Phải là 5 chữ số")]
//     public string FiveDigitCode { get; set; }

//     // ✅ Yêu cầu: Email phải kết thúc bằng @fpt.edu.vn
//     [RegularExpression(@"^[A-Za-z0-9._%+-]+@fpt\.edu\.vn$", ErrorMessage = "Email phải là @fpt.edu.vn")]
//     public string FptEmail { get; set; }

//     // ✅ Yêu cầu: Tên không chứa số hoặc ký tự đặc biệt (chỉ chữ và dấu cách)
//     [RegularExpression(@"^[A-Za-zÀ-ỹ\s]+$", ErrorMessage = "Tên chỉ chứa chữ và dấu cách")]
//     public string FullName { get; set; }
// }
