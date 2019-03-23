using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TodoApi.Models
{
    public class EditTodoModel
    {
        [Required, StringLength(512)]
        public string Description { get; set; }

        [BindRequired]
        public bool IsComplete { get; set; }
    }
}