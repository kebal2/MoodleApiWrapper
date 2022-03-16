using CommandLine;

public class Options
{
    [Option('h', "host", Required = true, HelpText = "Moodle host url.")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string Host { get; set; }

    [Option('t', "token", Required = true, HelpText = "Moodle host api token.")]
    public string Token { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [Option('c', "course-count", Required = false, HelpText = "Course count.")]
    public int? CourseCount { get; set; }
}