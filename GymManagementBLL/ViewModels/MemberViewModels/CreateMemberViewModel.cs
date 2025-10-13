using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class CreateMemberViewModel
    {
        [Required(ErrorMessage ="Name Is Required")]
        [StringLength( 50 , MinimumLength = 2, ErrorMessage = "Name Must Be Between 2 And 50 Char")]
        [RegularExpression(@"^[a-zA-Z\s]+$" , ErrorMessage = " Name Can Contain Only Letters And Spaces")]
        public string Name { get; set; } = null!;


        [Required(ErrorMessage = "Email Is Required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email Must Be Between 5 And 100 Char")]
        [DataType(DataType.EmailAddress)]  // يظهر ليه كل الايميلات اللي دخلها قبل كدا Email عشان لما ييجي يكتي ال
        [EmailAddress(ErrorMessage ="Invalid Email Format")] // غلط syntax عشان لو دخل ال
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "Phone Is Required")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$" , ErrorMessage = "Phone Number Must Be Valid Egyptian PhoneNumber")]
        [Phone(ErrorMessage ="Invalid Phone Format")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;


        [Required(ErrorMessage = "Data Of Birth Is Required")]
        [DataType(DataType.Date)]
        public DateOnly DataOfBirth { get; set; }


        [Required(ErrorMessage = "Gender  Is Required")]
        public Gender Gender { get; set; }


        [Required(ErrorMessage = "Building Number Is Required")]
        [Range(1,9000 , ErrorMessage = "Building Number Must Be Between 1 and 900")]
        public int BuildingNumber { get; set; }


        [Required(ErrorMessage = "Street Is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street Must Be Between 2 And 30 Char")]
        public string Street { get; set; } = null!;


        [Required(ErrorMessage = "City Is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "City Must Be Between 2 And 30 Char")]
        [RegularExpression(@"^[a-zA-Z\s]+$" , ErrorMessage = "City Can Contain Only Letters And Spaces")]
        public string City { get; set; } = null!;


        [Required(ErrorMessage = "Health Record Is Required")]
        public HealthRecordViewModel HealthRecordViewModel { get; set; } = null!;

    }
}
