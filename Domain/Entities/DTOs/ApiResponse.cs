using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.DTOs
{
    public class ApiResponse<T>
    {
        public string Status { get; set; } // "success" o "error"
        public string Message { get; set; } // Mensaje de éxito o error
        public T Data { get; set; } // Un objeto genérico que puede ser cualquier cosa o null

        // Constructor para inicializar la respuesta
        public ApiResponse(string status, string message, T data = default)
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}
