namespace CineMa.Models.ViewModel
{
    public class RegisterClientViewModel
    {
        // CLIENTE
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateOnly Birthday { get; set; }
        public string Senha { get; set; }

        // ENDEREÇO
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Road { get; set; }
        public string State { get; set; }
        public int Number { get; set; }
        public string Descripton { get; set; }
    }
}
