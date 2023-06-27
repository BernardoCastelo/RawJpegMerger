using System;
using System.Text.RegularExpressions;
using static RawJpegMerger.Constants;

namespace RawJpegMerger;

public class FileReadWritter
{
	private readonly string _rawFolder = "";
	private readonly IList<string> _rawFiles;
	private readonly string searchRegex = "IMG_(\\d+)";

	public FileReadWritter(string rawFolder)
	{
		_rawFolder = rawFolder;
		_rawFiles = Directory.GetFiles(_rawFolder);
	}

	/// <summary>
	/// Replaces the file in the nonRawFullPath with the one with similar name in the raw folder.
	/// </summary>
	/// <param name="jpegPath"></param>
	/// <returns>True if the file was found and replaced. False if the raw file wasn't found.</returns>
	public ReplaceWithRawIfFoundResults ReplaceWithRawIfFound(string nonRawFullPath)
	{
		var fileInfo = new FileInfo(nonRawFullPath);

		if (!fileInfo.Exists)
			return ReplaceWithRawIfFoundResults.FileNotFound;

		var filename = fileInfo.Name.Substring(0, fileInfo.Name.Length - 4);

        var regex = new Regex(searchRegex);
        var rawFilePath = _rawFiles.FirstOrDefault(rawFile => regex.Match(rawFile).Value == filename);

		if (rawFilePath == null)
		{
			return ReplaceWithRawIfFoundResults.RawFileNotFound;
		}

        var rawFile = new FileInfo(rawFilePath);

		var newPath = nonRawFullPath.Substring(0, nonRawFullPath.LastIndexOf('.'));

		var rawExtencion = rawFile.Extension;

        try
		{
            rawFile.MoveTo($"{newPath}{rawFile.Extension}");
			fileInfo.Delete();
        }
		 catch (Exception e)
		{
            return ReplaceWithRawIfFoundResults.RawCopyFailed;
        }

		return ReplaceWithRawIfFoundResults.Success;
    }

	public IList<string> GetAllFileNames(string path)
	{
		return Directory.GetFiles(path, "*.JPG", SearchOption.AllDirectories);
	}
}

