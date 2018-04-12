using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICASStacks.DataContract
{

    [Table("ICASDB.StateOfLocation")]
    public class StateOfLocation
    {
        [ScaffoldColumn(false)]
        public int StateOfLocationId { get; set; }

        public string Name { get; set; }
    }
}
