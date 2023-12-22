using System.ComponentModel.DataAnnotations;

namespace PriceAnalyzer.Dto;

public record ParseRequest([Required] string Url, [Required] int? Amount);