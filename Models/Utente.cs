namespace EsercizioSettimana11Marzo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Utente")]
    public partial class Utente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Utente()
        {
            Ordines = new HashSet<Ordine>();
        }

        [Key]
        public int IdUtente { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoUtente { get; set; } = "USER";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ordine> Ordines { get; set; }
    }
}
