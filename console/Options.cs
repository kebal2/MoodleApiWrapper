using CommandLine;

public class Options
{
    [Option('h', "host", Required = true, HelpText = "Moodle host url.")]
    public string Host { get; set; }
    
    [Option('t', "token", Required = true, HelpText = "Moodle host api token.")]
    public string Token { get; set; }
    
    [Option('c', "course-count", Required = false, HelpText = "Course count.")]
    public int? CourseCount { get; set; }
}