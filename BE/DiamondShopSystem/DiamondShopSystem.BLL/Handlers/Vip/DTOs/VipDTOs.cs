namespace DiamondShopSystem.BLL.Handlers.Vip.DTOs
{
    public class VipDto
    {
        public Guid VipId { get; set; }
        public Guid? UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Tier { get; set; } = string.Empty;
    }

    public class VipGetResponseDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public VipDto? Vip { get; set; }
    }

    public class VipCreateRequestDto
    {
        public Guid? UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Tier { get; set; } = string.Empty;
    }

    public class VipCreateResponseDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public VipDto? Vip { get; set; }
    }

    public class VipUpdateRequestDto
    {
        public DateTime? EndDate { get; set; }
        public string Tier { get; set; } = string.Empty;
    }

    public class VipUpdateResponseDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public VipDto? Vip { get; set; }
    }

    public class VipDeleteResponseDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }

    public class VipListResponseDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public List<VipDto> Vips { get; set; } = new List<VipDto>();
    }
}