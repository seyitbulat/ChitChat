public class Hub
{
	public long Id { get; set; }
	public string Hubname { get; set; }
	public bool Status { get; set; }

	public DateTime? AddedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
	public DateTime? DeletedDate { get; set; } // Nullable to represent optional dates

	public bool IsActive { get; set; }
}
