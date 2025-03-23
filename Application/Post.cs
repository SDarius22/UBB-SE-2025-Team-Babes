using System;

public class Post
{
    [Key]
    public long Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    [ForeignKey("UserId")]
    public long UserId { get; set; }

    public PostVisibility Visibility { get; set; }

    public PostTag Tag { get; set; }

    public Post()
	{
	}
}
