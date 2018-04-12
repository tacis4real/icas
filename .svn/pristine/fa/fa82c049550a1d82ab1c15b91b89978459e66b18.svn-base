using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ICASStacks.Common;

namespace ICASStacks.DataContract.BioEnroll
{
    public class LocalArea
    {

        public LocalArea()
        {
            Beneficiaries = new HashSet<Beneficiary>();
        }


        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LocalAreaId { get; set; }

        [Required(ErrorMessage = "State Information is required")]
        [CheckNumber(0, ErrorMessage = "Invalid State Information")]
        public int StateId { get; set; }
        public string Name { get; set; }
        public int Status  { get; set; }


        public ICollection<Beneficiary> Beneficiaries { get; set; }
        public virtual State State { get; set; }
    }
}
