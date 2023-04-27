namespace DatosPacientes.DTOs
{
    public class DireccionDTO
    {
        /// <summary>
        /// Este es el identificador unico de la tabla direccion
        /// </summary>
        public int Codigo { get; set; }
        /// <summary>
        /// Almacena la descripción de la dirección
        /// </summary>

        public string? DireccionCompleta { get; set; }
        /// <summary>
        /// Almacena el código postal de la persona
        /// </summary>
        public string? Referencia { get; set; }
    }
        
}
