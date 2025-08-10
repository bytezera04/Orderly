namespace Orderly.Shared.Dtos
{
    public class OrdersTableDto
    {
        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();

        public bool IsReceived { get; set; } = false;

        public bool CanManageStatus { get; set; } = false;

        public bool CanCancel { get; set; } = false;

        public string SortColumn { get; set; } = string.Empty;

        public string SortDirection { get; set; } = string.Empty;

        public string PagePath { get; set; } = string.Empty;
    }
}
