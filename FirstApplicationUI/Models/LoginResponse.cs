using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FirstApplicationUI.Models
{
    public class LoginVm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class ForgotPasswordVm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class ResetPasswordVm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public UserVm User { get; set; }
    }

    public class UserVm
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
    }
    public class EmployeeResponseDto
    {
        public int EmployeeId { get; set; }

        public string? FirstName { get; set; }

        public string LastName { get; set; } = null!;
        public string EmploymentStatus { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public DateOnly HireDate { get; set; }

        public string? JobTitle { get; set; }

        public string? DepartmentName { get; set; }

        public string? ManagerName { get; set; }

        // Salary field
        public decimal? Salary { get; set; }
    }

    public class AdminDashboardViewModel
    {
        public string? Username { get; set; }

        public IEnumerable<EmployeeResponseDto>? Employees { get; set; }
    }
    public class HRDashboardViewModel
    {
        public string? Username { get; set; }

        public IEnumerable<EmployeeResponseDto>? Employees { get; set; }
    }

}
