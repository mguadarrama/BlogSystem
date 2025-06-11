using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BlogCore.Domain;

namespace Tests.Benchmark;

[MemoryDiagnoser]
public class AuthorValidationBenchmarks
{
    private const string ValidSsn = "12345678901";
    private const string InvalidSsn = "1234567890a";
    private const string EmptySsn = "";
    private const string NullSsn = null;
    private const string ShortSsn = "123";
    private const string LongSsn = "123456789012";

    [Benchmark]
    public void ValidateValidSsn()
    {
        try
        {
            var author = new Author("Ellen", "Sano", ValidSsn);
        }
        catch (DomainException)
        {
            // Expected to not throw
        }
    }

    [Benchmark]
    public void ValidateInvalidSsn()
    {
        try
        {
            var author = new Author("Ellen", "Sano", InvalidSsn);
        }
        catch (DomainException)
        {
            // Expected to throw
        }
    }

    [Benchmark]
    public void ValidateEmptySsn()
    {
        try
        {
            var author = new Author("Ellen", "Sano", EmptySsn);
        }
        catch (DomainException)
        {
            // Expected to throw
        }
    }

    [Benchmark]
    public void ValidateNullSsn()
    {
        try
        {
            var author = new Author("Ellen", "Sano", NullSsn);
        }
        catch (DomainException)
        {
            // Expected to throw
        }
    }

    [Benchmark]
    public void ValidateShortSsn()
    {
        try
        {
            var author = new Author("Ellen", "Sano", ShortSsn);
        }
        catch (DomainException)
        {
            // Expected to throw
        }
    }

    [Benchmark]
    public void ValidateLongSsn()
    {
        try
        {
            var author = new Author("Ellen", "Sano", LongSsn);
        }
        catch (DomainException)
        {
            // Expected to throw
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<AuthorValidationBenchmarks>();
    }
} 