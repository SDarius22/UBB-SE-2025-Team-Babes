﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Entities;

public class Post
{
    [Key]
    public long Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    [ForeignKey("UserId")]
    public long UserId { get; set; }
    //PostVisibility

    //PostTag
}

