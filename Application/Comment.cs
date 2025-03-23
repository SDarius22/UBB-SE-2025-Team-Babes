using System;

public class Comment
{

    [Key]
    public long Id { get; set; }

    [ForeignKey("UserId")]
    public long UserId { get; set; }
    [ForeignKey("PostId")]
    public long PostId { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedDate { get; set; }

    public Comment()
	{
	}
}
