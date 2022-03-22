using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;
using FluentAssertions;
using Xunit;

namespace ConsoleAppFramework.Integration.Test;

public class PublishTrimmedAppTest
{
	private static string GetSolutionDirectoryPath()
	{
		var current = new DirectoryInfo(Environment.CurrentDirectory);
		while (!current.GetFiles("*.sln").Any()) current = current.Parent ?? throw new DirectoryNotFoundException();
		return current.FullName;
	}

	[Fact]
	public async Task Publish_Trimmed_Assembly()
	{
		var output = new StringBuilder();
		
		var result = await Cli
			.Wrap("dotnet")
			.WithArguments("publish tests/ConsoleAppFramework.Test.TrimmedAssembly --self-contained --runtime win-x64")
			.WithValidation(CommandResultValidation.None)
			.WithWorkingDirectory(GetSolutionDirectoryPath())
			.WithStandardOutputPipe(PipeTarget.ToStringBuilder(output))
			.ExecuteBufferedAsync();

		result.ExitCode.Should().Be(0, $"Publish command should complete successfully. Output: [{output}]");
		result.StandardError.Should().BeEmpty();
	}
}