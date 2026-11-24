using Microsoft.AspNetCore.Mvc.RazorPages;
using NovaHarvest.Web.Models;

namespace NovaHarvest.Web.Pages;

public class SurplusModel : PageModel
{
    // Defino el descuento, los kilos por caja, los días de conservación y el límite de stock.
    private const decimal SurplusDiscount = 0.30m;
    private const int BoxKg = 5;
    private const int ShelfLifeDays = 7;
    private const int LowStockKg = 50;

    // Creo el único objeto donde guardo los datos del excedente.
    public Surplus Surplus { get; } = new Surplus();

    // Guardo los resultados que después utilizo para mostrar la información en la página.
    public int RemainingKg { get; private set; }
    public DateTime ConsumeBefore { get; private set; }
    public int DaysToConsume { get; private set; }
    public decimal SurplusPrice { get; private set; }
    public int FullBoxes { get; private set; }
    public int LooseKg { get; private set; }
    public decimal SurplusValue { get; private set; }
    public string Status { get; private set; } = "";
    public string ProducerNote { get; private set; } = "";
    public bool HasStock { get; private set; }
    public bool Urgent { get; private set; }

    public void OnGet()
    {
        // Introduzco los datos iniciales del excedente.
        Surplus.Product = "Cereza Picota";
        Surplus.Producer = "Huerta Los Castaños";
        Surplus.Village = "Cabezuela del Valle";
        Surplus.HarvestDate = DateTime.Today.AddDays(-2);
        Surplus.AvailableKg = 343;
        Surplus.ReservedKg = 120;
        Surplus.PricePerKg = 3.20m;
        Surplus.Note = "<b>Recoger antes de las 14:00</b>";

        // Calculo los kilos que quedan después de restar los reservados.
        RemainingKg = Surplus.AvailableKg - Surplus.ReservedKg;

        // Calculo la fecha límite sumando los días de conservación a la fecha de cosecha.
        ConsumeBefore = Surplus.HarvestDate.AddDays(ShelfLifeDays);

        // Calculo los días que quedan hasta la fecha límite de consumo.
        DaysToConsume = (ConsumeBefore - DateTime.Today).Days;

        // Calculo el precio rebajado aplicando el descuento del 30 % al precio original.
        SurplusPrice = Surplus.PricePerKg * (1 - SurplusDiscount);

        // Calculo cuántas cajas completas de 5 kg puedo preparar.
        FullBoxes = RemainingKg / BoxKg;

        // Calculo los kilos que sobran después de llenar las cajas.
        LooseKg = RemainingKg % BoxKg;

        // Calculo el valor total del excedente utilizando el precio con descuento.
        SurplusValue = SurplusPrice * RemainingKg;

        // Compruebo si quedan kilos disponibles para permitir o deshabilitar la reserva.
        HasStock = RemainingKg > 0;

        // Indico si está agotado, quedan pocos kilos o sigue estando disponible.
        Status = !HasStock ? "Agotado" : RemainingKg < LowStockKg ? "¡Últimos kilos!" : "Disponible";

        // Utilizo la nota del productor o un texto alternativo si está vacía.
        ProducerNote = Surplus.Note ?? "Sin observaciones";

        // Compruebo si quedan dos días o menos y todavía hay stock para marcarlo como urgente.
        Urgent = DaysToConsume <= 2 && HasStock;
    }
}