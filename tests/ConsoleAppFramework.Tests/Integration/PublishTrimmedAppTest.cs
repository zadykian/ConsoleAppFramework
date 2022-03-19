using System;
using System.IO;
using System.Linq;
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
		var result = await Cli
			.Wrap("dotnet")
			.WithArguments("publish tests/ConsoleAppFramework.Test.TrimmedAssembly --runtime win-x64")
			.WithValidation(CommandResultValidation.None)
			.WithWorkingDirectory(GetSolutionDirectoryPath())
			.ExecuteBufferedAsync();

		result.ExitCode.Should().Be(0, "publish command should complete successfully");
		result.StandardError.Should().BeEmpty();
	}
}