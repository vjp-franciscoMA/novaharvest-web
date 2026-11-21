using Microsoft.AspNetCore.Mvc.RazorPages;
using NovaHarvest.Web.Models;

namespace NovaHarvest.Web.Pages;

public class SurplusModel : PageModel
{
    private const decimal SurplusDiscount = 0.30m;
    private const int BoxKg = 5;
    private const int ShelfLifeDays = 7;
    private const int LowStockKg = 50;

    public Surplus Surplus { get; } = new Surplus();

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

    // Para esta parte de los cálculos me he ayudado de la IA porque no sabía muy bien cómo hacerlo.
    public void OnGet()
    {
        Surplus.Product = "Cereza Picota";
        Surplus.Producer = "Huerta Los Castaños";
        Surplus.Village = "Cabezuela del Valle";
        Surplus.HarvestDate = DateTime.Today.AddDays(-2);
        Surplus.AvailableKg = 343;
        Surplus.ReservedKg = 120;
        Surplus.PricePerKg = 3.20m;
        Surplus.Note = "<b>Recoger antes de las 14:00</b>";

        RemainingKg = Surplus.AvailableKg - Surplus.ReservedKg;

        ConsumeBefore = Surplus.HarvestDate.AddDays(ShelfLifeDays);
        DaysToConsume = (ConsumeBefore - DateTime.Today).Days;

        SurplusPrice = Surplus.PricePerKg * (1 - SurplusDiscount);

        FullBoxes = RemainingKg / BoxKg;
        LooseKg = RemainingKg % BoxKg;

        SurplusValue = SurplusPrice * RemainingKg;

        HasStock = RemainingKg > 0;

        Status = !HasStock ? "Agotado" : RemainingKg < LowStockKg ? "¡Últimos kilos!" : "Disponible";

        ProducerNote = Surplus.Note ?? "Sin observaciones";

        Urgent = DaysToConsume <= 2 && HasStock;
    }
}