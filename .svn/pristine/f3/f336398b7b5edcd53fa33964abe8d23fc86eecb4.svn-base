using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ICAS.Models
{
    public class CustomViewModel
    {
        public string Vehicle { get; set; }

        public List<SelectListItem> Vehicles { get; private set; }

        public CustomViewModel()
        {

            var germanGroup = new SelectListGroup { Name = "German Cars" };
            var swedishGroup = new SelectListGroup { Name = "Swedish Cars" };

            Vehicles = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "audi",
                    Text = "Audi",
                    Group = germanGroup
                },
                new SelectListItem
                {
                    Value = "mercedes",
                    Text = "Mercedes",
                    Group = germanGroup
                },
                new SelectListItem
                {
                    Value = "saab",
                    Text = "Saab",
                    Group = swedishGroup
                },
                new SelectListItem
                {
                    Value = "volvo",
                    Text = "Volvo",
                    Group = swedishGroup
                }
            };
        }
    }
}