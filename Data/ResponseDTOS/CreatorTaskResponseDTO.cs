namespace Kruggers_Backend.Data.ResponseDTOS
{
    /**
     * The purpose of this DTO is to encapsulate the details of a creator task response Info. 
     * It is designed to transfer data related to tasks assigned to creators.
     */
    public class CreatorTaskResponseDTO
    {
        public string AssignedTo { get; set; }
        public string RequestedBy { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public StatusDTO Status { get; set; }
    }
}
