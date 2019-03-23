using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Data
{
    public class Todo
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(512)]
        public string Description { get; set; }

        [Required]
        [StringLength(64)]
        public string UserName { get; set; }

        public bool IsComplete { get; set; }

        public DateTimeOffset AddedAt { get; set; }

        public DateTimeOffset LastUpdatedAt { get; set; }
    }
}