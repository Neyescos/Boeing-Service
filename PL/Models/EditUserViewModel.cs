using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace PL.Models
{
    public class EditUserViewModel
    {
        public UserViewModel User { get; set; }
        public List<CheckBoxPlaneModel> Planes { get; set; }
        public List<SelectListItem> SelectedIds { get; set; }
        public EditUserViewModel()
        {
            Planes = new List<CheckBoxPlaneModel>();
        }
    }
}
