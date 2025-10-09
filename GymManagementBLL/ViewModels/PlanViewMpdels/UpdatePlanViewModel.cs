using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.PlanViewMpdels
{
    public class UpdatePlanViewModel
    {

        public string PlanName { get; set; } = null!;

        [Required(ErrorMessage = " Description Is Requierd")]
        [StringLength( 200, MinimumLength = 5 , ErrorMessage = " Description Must Be Between 5 and 200 ")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = " Duration Days Is Requierd")]
        [Range(1 , 365 , ErrorMessage = "Duration Days Must Be Between 1 and 365")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = " Price Days Is Requierd")]
        [Range(0.1, 10000, ErrorMessage = "Price Days Must Be Between 0.1 and 10000")]
        public decimal Price {  get; set; }


    }
}
