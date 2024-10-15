namespace JsonBenchmarks.Dto;

public record SubsetStaplingRange
{
    public uint StartPage { get; set; }
    public uint EndPage { get; set; }

    public SubsetStaplingRange() { }

    public SubsetStaplingRange(uint startPage, uint endPage)
    {
        StartPage = startPage;
        EndPage = endPage;
    }
}
