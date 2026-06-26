using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.PlanViewModel
{
    public class UpdatePlanViewModel
    {
        public string Name { get; set; } = default!;
        [Required (ErrorMessage = "Description Is Required")]
        [StringLength(200 , MinimumLength = 5 , ErrorMessage = "Description Is Must Betwwen 5 and 200 Characters")]

        public string Description { get; set; } = default!;
        [Required(ErrorMessage = "Duration Days Is Required")]
        [Range(1 , 365 , ErrorMessage = "Duration Must Between 1 and 365 days")]
        public int DurationDays { get; set; }
        [Required(ErrorMessage = "Price Is Required")]
        [Range(0.01 , 10000 , ErrorMessage = "Price Must Be Greater Than 0")]
        public decimal Price { get; set; }
    }
}
