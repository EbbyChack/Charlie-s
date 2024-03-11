namespace EsercizioSettimana11Marzo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DettaglioOrdine")]
    public partial class DettaglioOrdine
    {
        [Key]
        public int IdDettaglioOrdine { get; set; }

        public int IdOrdine { get; set; }

        public int IdArticolo { get; set; }

        public int Quantita { get; set; }

        public virtual Articolo Articolo { get; set; }

        public virtual Ordine Ordine { get; set; }
    }
}
