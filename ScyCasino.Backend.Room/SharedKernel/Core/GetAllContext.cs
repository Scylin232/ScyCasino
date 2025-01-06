﻿namespace SharedKernel.Core;

public class GetAllContext
{
    public int Page { get; set; } = 1;
    public int Count { get; set; } = 10;
    
    public string? SortColumn { get; set; }
    public bool IsAscending { get; set; } = true;
    
    public string? SearchTerm { get; set; }
    public string? SearchContent { get; set; }
}