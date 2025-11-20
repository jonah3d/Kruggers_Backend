namespace Kruggers_Backend.Data.ResponseDTOS
{
    /**
     * The purpose of this DTO is to encapsulate the details of a consumer task request Info. 
     * It is designed to transfer data related to tasks requested by consumers.
     */

    public class ConsumerTaskResponseDTO
    {
        
        public int TaskId { get; set; }
        public string RequestedBy { get; set; }
        public string AssignedTo { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public StatusDTO Status { get; set; }
    }
}
