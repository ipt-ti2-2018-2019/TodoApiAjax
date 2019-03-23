using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class AddTodoModel
    {
        [Required, StringLength(512)]
        public string Description { get; set; }
    }
}