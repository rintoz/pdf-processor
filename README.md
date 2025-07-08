# PDF Processor

Description:

PDFProcessor - Utility to process the PDF documents.

```
Usage:
  PDFProcessor [options]

Options:
  --inputfilepath <inputfilepath>    Fully qualified file path to the input PDF document.
  --outputfilepath <outputfilepath>  Fully qualified file path to the output PDF document.
  --watermark <watermark>            Watermark for the PDF document.
  --guid <guid>                      Enable guid stamp on the PDF document.
  --zipinputfolder <zipinputfolder>  Fully qualified folder path to zip.
  --zipoutputfile <zipoutputfile>    Output zip file name.
```

Sample

```
PDFProcessor.exe --zipinputfolder "C:\Users\Username\Downloads\Images" --zipoutputfile ImageOutput.zip
```
