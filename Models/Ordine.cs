namespace EsercizioSettimana11Marzo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ordine")]
    public partial class Ordine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ordine()
        {
            DettaglioOrdines = new HashSet<DettaglioOrdine>();
        }

        [Key]
        public int IdOrdine { get; set; }

        public decimal TotaleOrdine { get; set; }

        [Required]
        [StringLength(50)]
        public string Indirizzo { get; set; }

        [Required]
        public string Note { get; set; }

        public bool IsEvaso { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataOrdine { get; set; }

        public int IdUtente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DettaglioOrdine> DettaglioOrdines { get; set; }

        public virtual Utente Utente { get; set; }
    }
}
