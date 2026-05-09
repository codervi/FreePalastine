namespace ForFreePalestine.Models
{
    public enum UserRoles
    {
        SuperUser = 1,   // Her şeye yetkili (Sen)
        Chef = 2,        // İçerik yöneten, onaylayan
        Assistant = 3,   // Veri girişi yapan
        StandardUser = 4 // Sadece izleyen/yorum yapan
    }
}
